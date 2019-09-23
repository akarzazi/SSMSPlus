namespace Demo.Services
{
    using SSMSPlusCore.Integration.Connection;
    using SSMSPlusSearch.Entities;
    using SSMSPlusSearch.Services;
    using System;
    using System.Threading.Tasks;

    public class DemoDbIndexer : IDbIndexer
    {
        public const int DBId = 2;
        async Task<int> IDbIndexer.DbExistsAsync(DbConnectionString dbConnectionString)
        {
            await Task.Delay(1000);
            return DBId;
        }

        async Task<int> IDbIndexer.IndexAsync(DbConnectionString dbConnectionString)
        {
            await Task.Delay(2000);
            return DBId;
        }

        async Task<int> IDbIndexer.ReIndexAsync(DbConnectionString dbConnectionString)
        {
            await Task.Delay(2000);
            return DBId;
        }
    }
}
