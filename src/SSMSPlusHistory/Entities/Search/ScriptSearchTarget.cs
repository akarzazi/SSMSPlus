namespace SSMSPlusHistory.Entities.Search
{
    using SSMSPlusCore.Ui.Search;
    using SSMSPlusCore.Utils;
    using System;

    public class ScriptSearchTarget
    {
        public QueryItem QueryItem { get; }

        public ScriptSearchTarget(QueryItem queryItem)
        {
            QueryItem = queryItem;
        }

        public DateTime ExecutionDateUtc => QueryItem.ExecutionDateUtc;
        public string ExecutionDateLocalStr => ExecutionDateUtc.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss");

        public TextFragments ServerFragment => new TextFragments(TextFragment.Primary(QueryItem.Server));
        public TextFragments DatabaseFragment => new TextFragments(TextFragment.Primary(QueryItem.Database));
        public TextFragments QueryFragment => new TextFragments(TextFragment.Primary(QueryItem.Query));

        public TextFragments SmallQueryFragment
        {
            get
            {
                var smallDef = QueryItem.Query?.Trim().Truncate(300).RemoveLineReturns();
                return new TextFragments(TextFragment.Primary(smallDef));
            }
        }

        public TextFragments ServerHighlight(string search)
        {
            return ServerFragment.HighlightPrimary(search);
        }

        public TextFragments DatabaseHighlight(string search)
        {
            return DatabaseFragment.HighlightPrimary(search);
        }

        public TextFragments QueryHighlight(string search)
        {
            return QueryFragment.HighlightPrimary(search);
        }

        public TextFragments SmallQueryHighlight(string search)
        {
            return SmallQueryFragment.HighlightPrimary(search);
        }
    }
}
