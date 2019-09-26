//using SSMSPlusCore.Ui.Search;
//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Media;
//using System.Windows.Threading;

//namespace SSMSPlusCore.Ui.Converters
//{
//    public class HighlightToFlowDocumentConverter : IValueConverter
//    {
//        private static SolidColorBrush brush = new SolidColorBrush(Colors.Gold);
//        public static Run Mat;
//        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
//        {
//            var highlight = value as HighlightContext;
//            var text = highlight.Text;
//            var search = highlight.Search;
//            if (string.IsNullOrEmpty(search) || string.IsNullOrEmpty(text))
//                return new FlowDocument(new Paragraph(new Run(text)));


//            var list = new List<Run>();

//            var index = -1;
//            var startIndex = 0;
//            do
//            {
//                index = text.IndexOf(search, startIndex, StringComparison.InvariantCultureIgnoreCase);
//                if (index == -1)
//                {
//                    var sub = text.Substring(startIndex);
//                    list.Add(new Run(sub));
//                }
//                else
//                {
//                    var sub = text.Substring(startIndex, index - startIndex);
//                    list.Add(new Run(sub));

//                    var match = new Run(text.Substring(index, search.Length));
//                    Mat = match;
//                    //match.Dispatcher.Invoke(() =>
//                    //{
//                    //    Thread.Sleep(1000);
//                    //    match.BringIntoView();
//                    //});

//                    match.Background = brush;
//                    list.Add(match);

//                    startIndex = index + search.Length;
//                }
//            }
//            while (index != -1);

//            var para = new Paragraph();
//            foreach (var item in list)
//            {
//                para.Inlines.Add(item);
//            }
//            return new FlowDocument(para);
//        }

//        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
//        {
//            throw new NotSupportedException();
//        }
//    }
//}