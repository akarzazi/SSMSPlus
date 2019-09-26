using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SSMSPlusCore.Ui.Controls.LoadingIndicator
{
    /// <summary>
    /// Interaction logic for LoadingIndicator.xaml
    /// </summary>
    public partial class LoadingIndicator : UserControl
    {
        public static readonly DependencyProperty TitleProperty =
     DependencyProperty.Register(
    "Title",
    typeof(string),
    typeof(LoadingIndicator),
    new PropertyMetadata(null));

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public LoadingIndicator()
        {
            InitializeComponent();
        }
    }
}
