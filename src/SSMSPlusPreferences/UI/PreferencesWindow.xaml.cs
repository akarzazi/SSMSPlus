using SSMSPlusCore.Di;
using SSMSPlusCore.Ui;
using System.ComponentModel;
using System.Windows;
using System.Windows.Automation.Peers;

namespace SSMSPlusPreferences.UI
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class PreferencesWindow : Window
    {
        public PreferencesWindow()
        {
            InitializeComponent();
            var viewModel = ServiceLocator.GetRequiredService<PreferencesWindowVM>();
            this.DataContext = viewModel;
        }

        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new EmptyAutomationPeer(this);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            this.Visibility = Visibility.Hidden;
        }
    }
}
