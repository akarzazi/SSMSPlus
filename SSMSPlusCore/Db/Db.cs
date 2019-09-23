using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using SSMSPlusCore.Dab.Dapper;

namespace SSMSPlusCore.Database
{
    public class Db
    {
        static Db()
        {
            SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());
            SqlMapper.AddTypeHandler(new NullableLongHandler());
        }

        private readonly SqliteConnectionFactory _dbConnectionFactory;

        public Db(SqliteConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory ?? throw new ArgumentNullException(nameof(dbConnectionFactory));
        }

        public async Task<T> CommandAsync<T>(Func<DbConnection, DbTransaction, int, Task<T>> command)
        {
            using (var connection = _dbConnectionFactory.GetConnection())
            {
                await connection.OpenAsync();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var result = await command(connection, transaction, 120);
                        transaction.Commit();
                        return result;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public async Task<int> ExecuteAsync(string sql, object parameters)
        {
            return await CommandAsync(async (conn, trn, timeout) =>
            {
                return await conn.ExecuteAsync(sql, parameters, trn, timeout);
            });
        }

        public async Task UseOpenConnection(Func<DbConnection, Task> action)
        {
            using (var connection = _dbConnectionFactory.GetConnection())
            {
                await connection.OpenAsync();
                await action(connection);
            }
        }

        public async Task<T> QuerySingleOrDefault<T>(string sql, object parameters)
        {
            using (var connection = _dbConnectionFactory.GetConnection())
            {
                await connection.OpenAsync();
                return (await connection.QuerySingleOrDefaultAsync<T>(sql, parameters));
            }
        }

        public async Task<T[]> SelectAsync<T>(string sql, object parameters)
        {
            using (var connection = _dbConnectionFactory.GetConnection())
            {
                await connection.OpenAsync();
                return (await connection.QueryAsync<T>(sql, parameters)).ToArray();
            }
        }
    }
}
