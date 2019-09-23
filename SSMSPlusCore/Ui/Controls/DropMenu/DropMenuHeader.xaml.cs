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

namespace SSMSPlusCore.Ui.Controls.DropMenu
{
    /// <summary>
    /// Interaction logic for DropMenuHeader.xaml
    /// </summary>
    public partial class DropMenuHeader : UserControl
    {
        public static readonly DependencyProperty ItemNameProperty =
             DependencyProperty.Register(
            "ItemName",
            typeof(string),
            typeof(DropMenuHeader),
            new PropertyMetadata(null));

        public string ItemName
        {
            get => (string)GetValue(ItemNameProperty);
            set => SetValue(ItemNameProperty, value);
        }

        public DropMenuHeader()
        {
            InitializeComponent();
        }
    }
}
