using SSMSPlusCore.Ui.Converters;
using SSMSPlusCore.Ui.Search;
using SSMSPlusCore.Utils;
using SSMSPlusSearch.Entities;
using SSMSPlusSearch.Services.Filtering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMSPlusSearch.UI
{
    public class SearchFilterResultVM
    {
        public ISearchTarget SearchResult { get; private set; }
        public FilterContext FilterContext { get; private set; }

        public SearchFilterResultVM(ISearchTarget searchResult, FilterContext filterContext)
        {
            this.SearchResult = searchResult;
            this.FilterContext = filterContext;
        }

        public TextFragments NameHighlight
        {
            get
            {
                if (FilterContext.MatchesOn.Contains(MatchOn.Name))
                {
                    return SearchResult.RichNameHighlight(FilterContext.Search);
                }
                else
                {
                    return SearchResult.RichName;
                }
            }
        }

        public TextFragments DefinitionHighlight
        {
            get
            {
                if (FilterContext.MatchesOn.Contains(MatchOn.Definition))
                {
                    return SearchResult.RichSmallDefinitionHighlight(FilterContext.Search);
                }
                else
                {
                    return SearchResult.RichSmallDefinition;
                }
            }
        }

        public TextFragments FullPreviewHighlight
        {
            get
            {
                if (FilterContext.MatchesOn.Contains(MatchOn.Definition))
                {
                    return SearchResult.RichFullDefinitionHighlight(FilterContext.Search);
                }
                else
                {
                    return SearchResult.RichFullDefinition;
                }
            }
        }
    }
}
