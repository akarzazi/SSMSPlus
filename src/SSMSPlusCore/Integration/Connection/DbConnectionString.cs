namespace SSMSPlusCore.Integration.Connection
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Text.RegularExpressions;

    public class DbConnectionString : IEquatable<DbConnectionString>
    {
        public string ConnectionString { get; }
        public string Database { get; }
        public string Server { get; }

        public string DisplayName => Database + " @ " + Server;

        public DbConnectionString(string connectionString, string database)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(NormalizeToLegacyConnectionString(connectionString));
            if (database != null)
            {
                builder.InitialCatalog = database;
            }

            ConnectionString = builder.ToString();
            Database = builder.InitialCatalog;
            Server = builder.DataSource;
        }

        public DbConnectionString(string connectionString)
            : this(connectionString, null)
        {
        }

        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 23 + (Server == null ? 0 : Server.GetHashCode());
            hash = hash * 23 + (Database == null ? 0 : Database.GetHashCode());
            return hash;
        }

        public bool Equals(DbConnectionString other)
        {
            return string.Equals(Server, other.Server, StringComparison.InvariantCultureIgnoreCase)
                &&
                string.Equals(Database, other.Database, StringComparison.InvariantCultureIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is DbConnectionString cnx))
                return false;

            return this.Equals(cnx);
        }

        private static readonly (string @new, string old)[] SqlPropertyRenames = new (string, string)[]
        {
            ("Application Intent", "ApplicationIntent"),
            ("Connect Retry Count", "ConnectRetryCount"),
            ("Connect Retry Interval", "ConnectRetryInterval"),
            ("Pool Blocking Period", "PoolBlockingPeriod"),
            ("Multiple Active Result Sets", "MultipleActiveResultSets"),
            ("Multi Subnet Failover", "MultiSubnetFailover"),
            ("Transparent Network IP Resolution", "TransparentNetworkIPResolution"),
            ("Trust Server Certificate", "TrustServerCertificate")
        };

        public static string NormalizeToLegacyConnectionString(string connectionString)
        {
            if (!string.IsNullOrWhiteSpace(connectionString))
            {
                foreach (var replacement in SqlPropertyRenames)
                {
                    connectionString = Regex.Replace(connectionString, replacement.@new, replacement.old, RegexOptions.IgnoreCase);
                }
            }

            return connectionString;
        }
    }
}
