using System.Data.Common;
using System.IO;

using Dapper;

using Microsoft.Data.Sqlite;

using SSMSPlusCore.App;
using SSMSPlusCore.Dab.Dapper;

namespace SSMSPlusCore.Database
{
    public class SqliteConnectionFactory
    {
        static SqliteConnectionFactory()
        {
            SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());
            SqlMapper.AddTypeHandler(new NullableLongHandler());
        }

        private IWorkingDirProvider _workingDirProvider;
        public SqliteConnectionFactory(IWorkingDirProvider workingDirProvider)
        {
            _workingDirProvider = workingDirProvider;
        }

        public DbConnection GetConnection()
        {
            return new SqliteConnection("Filename=" + Path.Combine(_workingDirProvider.GetWorkingDir(), "SSMS-Plus.sqlite"));
        }
    }
}
