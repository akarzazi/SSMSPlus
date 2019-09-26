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
using SSMSPlusCore.Di;
using SSMSPlusHistory.UI.VM;

namespace SSMSPlusHistory.UI
{
    /// <summary>
    /// Interaction logic for Simple.xaml
    /// </summary>
    public partial class HistoryControl : UserControl
    {
        public HistoryControl()
        {
            InitializeComponent();
            this.DataContext = ServiceLocator.GetRequiredService<HistoryControlVM>();
            this.Loaded += HistoryControl_Loaded;
        }

        private void HistoryControl_Loaded(object sender, RoutedEventArgs e)
        {
            QueryFilter.Focus();
            this.Loaded -= HistoryControl_Loaded;
        }
    }
}
