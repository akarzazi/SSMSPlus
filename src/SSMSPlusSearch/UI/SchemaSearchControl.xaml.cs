using SSMSPlusCore.Di;
using SSMSPlusCore.Integration.Connection;
using SSMSPlusCore.Ui;
using System;
using System.ComponentModel;
using System.Linq;
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
        private DataGridColumn currentSortColumn;
        private ListSortDirection currentSortDirection = ListSortDirection.Ascending;

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

        #region Grid Events

        /// <summary>
        /// Initializes the current sort column and direction.
        /// </summary>
        /// <param name="sender">The products data grid.</param>
        /// <param name="e">Ignored.</param>
        private void objectsGrid_Loaded(object sender, RoutedEventArgs e)
        {
            DataGrid dataGrid = (DataGrid)sender;

            // The current sorted column must be specified in XAML.
            currentSortColumn = dataGrid.Columns.Where(c => c.SortDirection.HasValue).Single();
            currentSortDirection = currentSortColumn.SortDirection.Value;
        }

        /// <summary>
        /// Sets the sort direction for the current sorted column since the sort direction
        /// is lost when the DataGrid's ItemsSource property is updated.
        /// </summary>
        /// <param name="sender">The parts data grid.</param>
        /// <param name="e">Ignored.</param>
        private void objectsGrid_TargetUpdated(object sender, DataTransferEventArgs e)
        {
            if (currentSortColumn != null)
            {
                currentSortColumn.SortDirection = currentSortDirection;
            }
        }

        /// <summary>
        /// Custom sort the datagrid since the actual records are stored in the
        /// server, not in the items collection of the datagrid.
        /// </summary>
        /// <param name="sender">The parts data grid.</param>
        /// <param name="e">Contains the column to be sorted.</param>
        private void objectsGrid_Sorting(object sender, DataGridSortingEventArgs e)
        {
            //Save current sort column and sort direction
            currentSortColumn = e.Column;
            currentSortDirection = (e.Column.SortDirection != ListSortDirection.Ascending) ? ListSortDirection.Ascending : ListSortDirection.Descending;

            //Get Current sort column name. e.g. SearchResult.Name => Name
            ViewModel.CurrentSortColumn = currentSortColumn.SortMemberPath.Split('.')[1];
            ViewModel.CurrentSortDirection = currentSortDirection;
        } 
        #endregion
    }
}
