using SSMSPlusCore.Utils;
using SSMSPlusSearch.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
namespace SSMSPlusSearch.Services.Filtering
{
    public class FilterResultService
    {
        private static Dictionary<MatchOn, Func<ISearchTarget, string, bool>> matchPredicates = new Dictionary<MatchOn, Func<ISearchTarget, string, bool>>();

        static FilterResultService()
        {
            matchPredicates.Add(MatchOn.Name, (p, str) => p.MatchsName(str));
            matchPredicates.Add(MatchOn.Definition, (p, str) => p.MatchsDefinition(str));
        }

        public static IEnumerable<ISearchTarget> Filter(IEnumerable<ISearchTarget> source, FilterContext filterContext)
        {
            source = source.Where(p => filterContext.Schemas.Contains(p.SchemaName));

            source = source.Where(p => filterContext.Categories.Contains(p.TypeCategory));

            if (!string.IsNullOrEmpty(filterContext.Search))
            {
                if (filterContext.MatchesOn.Count == 0)
                    return Enumerable.Empty<ISearchTarget>();

                Func<ISearchTarget, string, bool> predicate = BuildMatchOnPredicate(filterContext.MatchesOn);
                source = source.Where(p => predicate(p, filterContext.Search));
            }

            return source;
        }

        private static Func<ISearchTarget, string, bool> BuildMatchOnPredicate(HashSet<MatchOn> matchOns)
        {
            var predicates = matchOns.Select(p => matchPredicates[p]).ToArray();
            bool predicate(ISearchTarget r, string str)
            {
                bool orResult = false;
                foreach (var predic in predicates)
                {
                    orResult = predic(r, str) || orResult;
                }
                return orResult;
            }
            return predicate;
        }
    }
}
