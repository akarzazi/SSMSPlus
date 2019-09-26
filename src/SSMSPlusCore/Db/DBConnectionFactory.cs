using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Text;
using Microsoft.Data.Sqlite;
using SSMSPlusCore.App;

namespace SSMSPlusCore.Database
{
    public class SqliteConnectionFactory
    {
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
