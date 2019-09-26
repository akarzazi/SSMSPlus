namespace SSMSPlusHistory.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using SSMSPlusCore.Database;
    using SSMSPlusHistory.Entities;
    using SSMSPlusHistory.Services.Filtering;

    public class QueryItemRepository
    {
        protected readonly Db _db;

        public QueryItemRepository(Db db)
        {
            _db = db ?? throw new ArgumentException(nameof(db));
        }

        public async Task Insert(IEnumerable<QueryItem> queryItems)
        {
            string sql = "INSERT INTO 'History.QueryItem' (ExecutionDateUtc,Query,Server,Database) Values (@ExecutionDateUtc,@Query,@Server,@Database);";
            await _db.ExecuteAsync(sql, queryItems);
        }

        public async Task<QueryItem[]> FindItems(FilterContext filterContext)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(@"SELECT * FROM 'History.QueryItem' 
WHERE ExecutionDateUtc >= @FromUtc AND ExecutionDateUtc <= @ToUtc");

            if (!string.IsNullOrEmpty(filterContext.QuerySearch))
                stringBuilder.AppendLine("AND Query like @QuerySearch");

            if (!string.IsNullOrEmpty(filterContext.DbSearch))
                stringBuilder.AppendLine("AND Database like @DbSearch");

            if (!string.IsNullOrEmpty(filterContext.ServerSearch))
                stringBuilder.AppendLine("AND Server like @ServerSearch");

            stringBuilder.AppendLine("ORDER BY Id DESC LIMIT 1000");

            return await _db.SelectAsync<QueryItem>(
                stringBuilder.ToString(),
                new
                {
                    filterContext.FromUtc,
                    filterContext.ToUtc,
                    QuerySearch = "%" + filterContext.QuerySearch + "%",
                    DbSearch = "%" + filterContext.DbSearch + "%",
                    ServerSearch = "%" + filterContext.ServerSearch + "%",
                });
        }
    }
}
