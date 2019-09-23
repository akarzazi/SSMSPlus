using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMSPlusCore.Ui.Search
{
    public class TextFragment
    {
        public string Value { get; private set; }

        public TextFragmentType FragmentType { get; private set; }

        public TextFragment(string value, TextFragmentType fragmentType)
        {
            Value = value ?? ""; // ?? throw new ArgumentNullException("Cannot assign Null to TextFragment"); ;
            FragmentType = fragmentType;
        }

        public static TextFragment Primary(string value)
        {
            return new TextFragment(value, TextFragmentType.Primary);
        }

        public static TextFragment Highlight(string value)
        {
            return new TextFragment(value, TextFragmentType.Highlight);
        }

        public static TextFragment Secondary(string value)
        {
            return new TextFragment(value, TextFragmentType.Secondary);
        }
    }
}
