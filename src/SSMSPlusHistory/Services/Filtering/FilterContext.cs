namespace SSMSPlusHistory.Services.Filtering
{
    using System;

    public class FilterContext
    {
        public string QuerySearch { get; set; }
        public string ServerSearch { get; set; }
        public string DbSearch { get; set; }
        public DateTime FromUtc { get; set; }
        public DateTime ToUtc { get; set; }

        public FilterContext(string querySearch, string serverSearch, string dbSearch, DateTime fromUtc, DateTime toUtc)
        {
            QuerySearch = querySearch;
            ServerSearch = serverSearch;
            DbSearch = dbSearch;
            FromUtc = fromUtc;
            ToUtc = toUtc;
        }
    }
}
