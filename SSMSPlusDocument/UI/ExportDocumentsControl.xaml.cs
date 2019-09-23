namespace SSMSPlusDocument.UI
{
    using SSMSPlusCore.Di;
    using SSMSPlusCore.Integration.Connection;
    using SSMSPlusCore.Ui;
    using System;
    using System.Windows;
    using System.Windows.Automation.Peers;
    using System.Windows.Controls;
    using System.Windows.Data;

    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class ExportDocumentsControl : UserControl, IDisposable
    {
        public ExportDocumentsControl()
        {
            InitializeComponent();
            Loaded += SchemaSearchControl_Loaded;
        }

        private void SchemaSearchControl_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= SchemaSearchControl_Loaded;
        }

        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new EmptyAutomationPeer(this);
        }

        private ExportDocumentsControlVM ViewModel => this.DataContext as ExportDocumentsControlVM;

        public void Initialize(DbConnectionString cnxStr)
        {
            var viewModel = ServiceLocator.GetRequiredService<ExportDocumentsControlVM>();
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
