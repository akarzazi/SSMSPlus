using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;

namespace SSMSPlusCore.Ui.Extensions
{
    public class EnhancedDataGrid : DataGrid
    {
        private List<SortDescription> _sortDescriptions = new List<SortDescription>();

        protected override void OnSorting(DataGridSortingEventArgs eventArgs)
        {
            base.OnSorting(eventArgs);
            SaveSorting();
        }
        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);
            RestoreSorting(newValue);
        }

        private void RestoreSorting(IEnumerable newItemSource)
        {
            if (newItemSource == null)
                return;

            ICollectionView view = CollectionViewSource.GetDefaultView(newItemSource);
            view.SortDescriptions.Clear();

            foreach (SortDescription sortDescription in _sortDescriptions)
            {
                view.SortDescriptions.Add(sortDescription);

                DataGridColumn column = Columns.FirstOrDefault(c => c.SortMemberPath == sortDescription.PropertyName);
                if (column != null)
                    column.SortDirection = sortDescription.Direction;
            }
        }

        private void SaveSorting()
        {
            if (ItemsSource == null)
                return;

            ICollectionView view = CollectionViewSource.GetDefaultView(ItemsSource);
            _sortDescriptions = new List<SortDescription>(view.SortDescriptions);
        }
    }
}
