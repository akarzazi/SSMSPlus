using SSMSPlusSearch.Entities;
using System.Collections.Generic;

namespace SSMSPlusSearch.Services.Filtering
{
    public class FilterContext
    {
        public string Search { get; set; }
        public HashSet<MatchOn> MatchesOn { get; set; }
        public HashSet<DbSimplifiedType> Categories { get; set; }
        public HashSet<string> Schemas { get; set; }

        public FilterContext(string search, HashSet<MatchOn> matchesOn, HashSet<DbSimplifiedType> categories, HashSet<string> schemas)
        {
            this.Search = search;
            this.MatchesOn = matchesOn;
            this.Categories = categories;
            this.Schemas = schemas;
        }
    }
}
