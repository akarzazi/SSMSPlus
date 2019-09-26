using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace SSMSPlusCore.Ui.Search
{
    public class RunFragment : Run
    {
        public static readonly DependencyProperty TextFragmentTypeProperty = DependencyProperty.Register(
         "TextFragmentType",
         typeof(TextFragmentType),
         typeof(RunFragment),
         new PropertyMetadata(TextFragmentType.Primary));

        public TextFragmentType TextFragmentType
        {
            get { return (TextFragmentType)GetValue(TextFragmentTypeProperty); }
            set { SetValue(TextFragmentTypeProperty, value); }
        }

        public RunFragment(string text, TextFragmentType fragmentType) : base(text)
        {
            this.TextFragmentType = fragmentType;
        }

        public static RunFragment Create(TextFragment fragment)
        {
            return new RunFragment(fragment.Value, fragment.FragmentType);
        }
    }
}
