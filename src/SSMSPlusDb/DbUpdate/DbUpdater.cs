using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Reflection;

using Dapper;

using Microsoft.Extensions.Logging;

using SSMSPlusCore.App;
using SSMSPlusCore.Database;
using SSMSPlusDb.DbUpdate.Entities;

namespace SSMSPlusDb.DbUpdate
{
    public class DbUpdater
    {
        private SqliteConnectionFactory _dbConnectionFactory;
        private IVersionProvider _versionProvider;
        private Assembly _resourcesAssembly;

        private ILogger _logger;

        public DbUpdater(ILogger<DbUpdater> logger, SqliteConnectionFactory cnxFactory, IVersionProvider versionProvider)
        {
            _logger = logger;
            _dbConnectionFactory = cnxFactory;
            _versionProvider = versionProvider;
            _resourcesAssembly = typeof(DbUpdater).Assembly;
        }

        public void UpdateDb()
        {
            var targetBuild = _versionProvider.GetBuild();
            var currentBuild = GetCurrentBuild();

            if (targetBuild < currentBuild)
            {
                var exeption = new Exception($"Target Build version is lower than current, current {@currentBuild}, target {@targetBuild}");
                _logger.LogError(exeption, "Cannot Update DB");
                throw exeption;
            }

            if (targetBuild == currentBuild)
            {
                _logger.LogInformation("Build version is up to date, current {@currentBuild}, target {@targetBuild}", currentBuild, targetBuild);
                return;
            }

            _logger.LogInformation("Build version is out of date, current {@currentBuild}, target {@targetBuild}", currentBuild, targetBuild);

            var resourcesPrefix = "SSMSPlusDb.DbUpdate.Versions.";
            string[] resNames = _resourcesAssembly.GetManifestResourceNames().Where(p => p.StartsWith(resourcesPrefix)).OrderBy(p => p).ToArray();

            do
            {
                currentBuild++;
                var buildPrefix = resourcesPrefix + "V" + currentBuild.ToString("0000") + ".";
                var buildResources = resNames.Where(p => p.StartsWith(buildPrefix)).ToArray();
                UpdateVersion(currentBuild, buildResources);
            } while (currentBuild < targetBuild);

            _logger.LogInformation("Build version update {@currentBuild} (finished)", currentBuild);
        }

        private int GetCurrentBuild()
        {
            using (var connection = _dbConnectionFactory.GetConnection())
            {
                connection.Open();

                var sql = "SELECT count(*) FROM sqlite_master where type = 'table' and Name = 'SSMSPlus.BuildVersion'";
                var result = connection.QuerySingle<int>(sql);
                if (result == 0)
                    return 0;
                else
                {
                    var sqlBuildNumber = "SELECT BuildNumber FROM 'SSMSPlus.BuildVersion' ORDER BY BuildNumber DESC LIMIT 1 ";
                    var buildNumber = connection.QuerySingleOrDefault<int>(sqlBuildNumber);
                    return buildNumber;
                }
            }
        }

        private void UpdateVersion(int buildNumber, string[] resourceNames)
        {
            _logger.LogInformation("Updating to {@buildNumber}", buildNumber);

            using (var connection = _dbConnectionFactory.GetConnection())
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var buildVersionScriptsLog = new List<BuildVersionScript>();
                        foreach (var resourceName in resourceNames)
                        {
                            _logger.LogInformation("Executing resource {@resourceName}", resourceName);

                            var sql = GetEmbeddedResourceContent(resourceName);
                            connection.Execute(sql, transaction: transaction);

                            buildVersionScriptsLog.Add(BuildVersionScript.CreateNow(buildNumber, resourceName, sql));
                            _logger.LogInformation("Executing resource {@resourceName} (finished)", resourceName);
                        }

                        HistorizeBuildVersion(buildNumber, buildVersionScriptsLog, connection, transaction);

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        _logger.LogError(ex, "Error while updating to {@buildNumber}", buildNumber);
                        throw;
                    }
                }
            }

            _logger.LogInformation("Updating to {@buildNumber} (finished)", buildNumber);
        }

        private void HistorizeBuildVersion(int buildNumber, IEnumerable<BuildVersionScript> scriptsLog, DbConnection connection, DbTransaction transaction)
        {
            string sql = "INSERT INTO 'SSMSPlus.BuildVersion' (BuildNumber,InstallDateUtc) Values (@BuildNumber,@InstallDateUtc);";
            connection.Execute(
                sql,
                new { BuildNumber = buildNumber, InstallDateUtc = DateTime.UtcNow },
                transaction: transaction);

            string sqlScripts = @"INSERT INTO 'SSMSPlus.BuildVersionScripts'    (BuildNumber,ScriptName,ScriptContent,InstallDateUtc) 
                                                                            Values (@BuildNumber,@ScriptName,@ScriptContent,@InstallDateUtc);";
            connection.Execute(sqlScripts, scriptsLog, transaction: transaction);
        }

        public static string GetEmbeddedResourceContent(string resourceName)
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            using (Stream stream = asm.GetManifestResourceStream(resourceName))
            {
                using (StreamReader source = new StreamReader(stream))
                {
                    return source.ReadToEnd();
                }
            }
        }

    }
}
