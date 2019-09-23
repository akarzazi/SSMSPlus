namespace SSMSPlusCore.SqlServer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class DbHelper
    {
        private static HashSet<string> _systemDbs = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase) { "master", "model", "msdb", "tempdb" };

        public static bool IsSystemDb(string db)
        {
            return _systemDbs.Contains(db);
        }

        public static IReadOnlyCollection<string> GetSystemDbs()
        {
            return _systemDbs.ToArray();
        }
    }
}
