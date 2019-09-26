using Demo.Settings;
using SSMSPlusCore.Di;
using SSMSPlusCore.Integration.Connection;
using SSMSPlusCore.Ui.Extensions;
using SSMSPlusPreferences.UI;
using System.Windows;

namespace Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DemoDbSettings settings = ServiceLocator.GetRequiredService<DemoDbSettings>();
            var dbConnectionStr = new DbConnectionString(settings.ConnectionString, settings.DbName);

            var tested = 0;
            if (tested == 0)
            {
                var histo = new SSMSPlusHistory.UI.HistoryControl();
                Panel.Children.Add(histo);
            }
            else if (tested == 1)
            {
                var search = new SSMSPlusSearch.UI.SchemaSearchControl();
                search.Initialize(dbConnectionStr);
                Panel.Children.Add(search);
            }
            else if (tested == 2)
            {
                var docs = new SSMSPlusDocument.UI.ExportDocumentsControl();
                docs.Initialize(dbConnectionStr);
                Panel.Children.Add(docs);
            }
            else if (tested == 3)
            {
                var ui = new PreferencesWindow();
                ui.ShowAndActivate();
            }
        }
    }
}
