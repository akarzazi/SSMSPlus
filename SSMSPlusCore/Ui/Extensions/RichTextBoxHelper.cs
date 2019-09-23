namespace SSMSPlusCore.Ui.Extensions
{
    using SSMSPlusCore.Ui.Text;
    using SSMSPlusCore.Ui.Utils;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Documents;

    public class RichTextBoxHelper : DependencyObject
    {
        public static FlowDocument GetBindedDocument(DependencyObject obj)
        {
            return (FlowDocument)obj.GetValue(BindedDocumentProperty);
        }

        public static void SetBindedDocument(DependencyObject obj, FlowDocument value)
        {
            obj.SetValue(BindedDocumentProperty, value);
        }

        public static readonly DependencyProperty BindedDocumentProperty =
            DependencyProperty.RegisterAttached("BindedDocument", typeof(FlowDocument), typeof(RichTextBoxHelper), new PropertyMetadata(null, OnBindedDocumentChanged));

        private static void OnBindedDocumentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is RichTextBox target))
                return;

            FlowDocument doc = e.NewValue as FlowDocument;
            if (doc == null)
            {
                target.Document = new FlowDocument();
                return;
            }

            target.Document = XamlHelper.XamlClone(doc);
        }

        public static RunStream GetRunStream(DependencyObject obj)
        {
            return (RunStream)obj.GetValue(RunStreamProperty);
        }

        public static void SetRunStream(DependencyObject obj, RunStream value)
        {
            obj.SetValue(RunStreamProperty, value);
        }

        public static readonly DependencyProperty RunStreamProperty =
            DependencyProperty.RegisterAttached("RunStream", typeof(RunStream), typeof(RichTextBoxHelper), new PropertyMetadata(null, OnRunStreamChanged));

        private static void OnRunStreamChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is RichTextBox target))
                return;

            void OnMessage(object sender, Run run)
            {
                var para = GetOrCreateParagraph(target);
                para.Inlines.Add(XamlHelper.XamlClone(run));
                target.ScrollToEnd();
            }

            if (e.OldValue != null)
            {
                var coll = (RunStream)e.OldValue;
                coll.OnMessage -= OnMessage;
            }

            if (e.NewValue != null)
            {
                var coll = (RunStream)e.NewValue;
                coll.OnMessage += OnMessage;
            }
        }

        private static Paragraph GetOrCreateParagraph(RichTextBox target)
        {
            var para = target?.Document?.Blocks?.FirstBlock as Paragraph;
            if (para == null)
            {
                para = new Paragraph();
                target.Document = new FlowDocument(para);
            }
            return para;
        }

    }
}
