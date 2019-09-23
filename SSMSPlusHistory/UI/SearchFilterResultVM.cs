namespace SSMSPlusHistory.UI
{
    using SSMSPlusCore.Ui.Search;
    using SSMSPlusHistory.Entities.Search;
    using SSMSPlusHistory.Services.Filtering;

    public class SearchFilterResultVM
    {
        public SearchFilterResultVM(ScriptSearchTarget searchResult, FilterContext filterContext)
        {
            SearchResult = searchResult;
            FilterContext = filterContext;
        }

        public ScriptSearchTarget SearchResult { get; private set; }
        public FilterContext FilterContext { get; private set; }

        public TextFragments ServerHighlight
        {
            get
            {
                return SearchResult.ServerHighlight(FilterContext.ServerSearch);
            }
        }

        public TextFragments DatabaseHighlight
        {
            get
            {
                return SearchResult.DatabaseHighlight(FilterContext.DbSearch);
            }
        }

        public TextFragments QueryHighlight
        {
            get
            {
                return SearchResult.QueryHighlight(FilterContext.QuerySearch);
            }
        }

        public TextFragments SmallQueryHighlight
        {
            get
            {
                return SearchResult.SmallQueryHighlight(FilterContext.QuerySearch);
            }
        }

    }
}
