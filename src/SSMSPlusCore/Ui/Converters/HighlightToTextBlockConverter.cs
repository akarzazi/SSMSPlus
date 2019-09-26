using SSMSPlusCore.Ui.Search;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

namespace SSMSPlusCore.Ui.Converters
{
    public class HighlightToTextBlockConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var highlight = value as TextFragments;
            var tb = new TextBlock();
            foreach (var fragment in highlight.Fragments)
            {
                var run = RunFragment.Create(fragment);
                tb.Inlines.Add(run);
            }
            return tb;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }

}
