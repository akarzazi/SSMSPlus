using SSMSPlusCore.Di;
using SSMSPlusCore.Integration.Connection;
using SSMSPlusCore.Ui;
using System;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Data;

namespace SSMSPlusSearch.UI
{
    /// <summary>
    /// Interaction logic for Simple.xaml
    /// </summary>
    public partial class SchemaSearchControl : UserControl, IDisposable
    {
        public SchemaSearchControl()
        {
            InitializeComponent();
            Loaded += SchemaSearchControl_Loaded;
        }

        private void SchemaSearchControl_Loaded(object sender, RoutedEventArgs e)
        {
            Filter.Focus();
            Loaded -= SchemaSearchControl_Loaded;
        }

        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new EmptyAutomationPeer(this);
        }

        private SchemaSearchControlVM ViewModel => this.DataContext as SchemaSearchControlVM;

        public void Initialize(DbConnectionString cnxStr)
        {
            var viewModel = ServiceLocator.GetRequiredService<SchemaSearchControlVM>();
            this.DataContext = viewModel;
            this.Dispatcher.Invoke(() => viewModel.InitializeDb(cnxStr));
        }

        public void Dispose()
        {
            BindingOperations.ClearAllBindings(this);
            ViewModel?.Free();
            this.DataContext = null;
        }
    }
}
