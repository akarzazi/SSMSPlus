using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMSPlusCore.Ui.Search
{
    public class TextFragments
    {
        private List<TextFragment> _fragments { get; set; }

        public IReadOnlyList<TextFragment> Fragments => _fragments;

        public TextFragments()
        {
            _fragments = new List<TextFragment>();
        }

        public TextFragments(IEnumerable<TextFragment> fragments)
            : this()
        {
            _fragments.AddRange(fragments);
        }

        public TextFragments(TextFragment fragment)
        : this()
        {
            _fragments.Add(fragment);
        }

        public void Add(TextFragment fragment)
        {
            if (string.IsNullOrEmpty(fragment.Value))
                return;

            _stringViewCache = null;
            _fragments.Add(fragment);
        }

        public void Add(IEnumerable<TextFragment> fragments)
        {
            foreach (var fragment in fragments)
            {
                Add(fragment);
            }
        }

        public void Add(TextFragments fragments)
        {
            Add(fragments.Fragments);
        }

        public void AddPrimary(string text)
        {
            Add(TextFragment.Primary(text));
        }

        public void AddSecondary(string text)
        {
            Add(TextFragment.Secondary(text));
        }

        public void AddHighlight(string text)
        {
            Add(TextFragment.Highlight(text));
        }

        public string AsString => ToString();

        string _stringViewCache;
        public override string ToString()
        {
            return _stringViewCache ?? (_stringViewCache = string.Join("", _fragments.Select(v => v.Value)));
        }
    }
}
