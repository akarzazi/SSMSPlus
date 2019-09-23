using SSMSPlusCore.Utils;
using System;
using System.Linq;

namespace SSMSPlusCore.Ui.Search
{
    public static class TextSearchHelper
    {
        public static TextFragments CreateHighlightSearch(string text, string search)
        {
            var fragments = new TextFragments();
            if (string.IsNullOrEmpty(text))
            {
                return fragments;
            }

            if (string.IsNullOrEmpty(search))
            {
                fragments.AddPrimary(text);
                return fragments;
            }

            var index = -1;
            var startIndex = 0;
            do
            {
                index = text.IndexOf(search, startIndex, StringComparison.InvariantCultureIgnoreCase);
                if (index == -1)
                {
                    var sub = text.Substring(startIndex);
                    fragments.AddPrimary(sub);
                }
                else
                {
                    var sub = text.Substring(startIndex, index - startIndex);
                    fragments.AddPrimary(sub);

                    var match = text.Substring(index, search.Length);
                    fragments.AddHighlight(match);

                    startIndex = index + search.Length;
                }
            }
            while (index != -1);

            return fragments;
        }

        public static TextFragments HighlightPrimary(this TextFragments textFragments, string search)
        {
            var fragments = new TextFragments();

            foreach (var original in textFragments.Fragments)
            {
                if (original.FragmentType == TextFragmentType.Primary)
                {
                    fragments.Add(CreateHighlightSearch(original.Value, search));
                }
                else
                {
                    fragments.Add(original);
                }
            }

            return fragments;
        }

        public static bool PrimaryContainsText(this TextFragments textFragments, string search)
        {
            return textFragments.Fragments.Where(p => p.FragmentType == TextFragmentType.Primary).Any(p => p.Value.ContainsText(search) == true);
        }
    }
}
