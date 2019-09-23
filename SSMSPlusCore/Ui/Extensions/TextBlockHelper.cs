namespace SSMSPlusCore.Ui.Extensions
{
    using SSMSPlusCore.Ui.Utils;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;

    public class TextBlockHelper : DependencyObject
    {
        public static FlowDocument GetBindableInlines(DependencyObject obj)
        {
            return (FlowDocument)obj.GetValue(BindableInlinesProperty);
        }

        public static void SetBindableInlines(DependencyObject obj, FlowDocument value)
        {
            obj.SetValue(BindableInlinesProperty, value);
        }

        public static readonly DependencyProperty BindableInlinesProperty =
            DependencyProperty.RegisterAttached("BindableInlines", typeof(FlowDocument), typeof(TextBlockHelper), new PropertyMetadata(null, OnBindableInlinesChanged));

        private static void OnBindableInlinesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is TextBlock Target))
                return;

            FlowDocument doc = e.NewValue as FlowDocument;
            var para = doc?.Blocks?.FirstBlock as Paragraph;
            if (para == null)
                return;

            Target.Inlines.Clear();
            foreach (var item in para.Inlines)
            {
                Target.Inlines.Add(XamlHelper.XamlClone(item));
            }
        }
    }
}
