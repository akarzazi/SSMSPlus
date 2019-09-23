namespace SSMSPlusCore.Ui.Extensions
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    public static class GridExtensions
    {
        public static string GetStructure(Grid grid)
        {
            return (string)grid.GetValue(StructureProperty);
        }

        public static void SetStructure(Grid grid, string value)
        {
            grid.SetValue(StructureProperty, value);
        }

        public static readonly DependencyProperty StructureProperty =
            DependencyProperty.RegisterAttached("Structure", typeof(string), typeof(GridExtensions), new PropertyMetadata("*|*", OnStructureChanged));

        private static void OnStructureChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Grid grid)
            {
                var converter = new GridLengthConverter();
                grid.RowDefinitions.Clear();
                grid.ColumnDefinitions.Clear();

                if (e.NewValue is string structureString)
                {
                    var rowsAndColumns = structureString.Split('|');
                    if (rowsAndColumns.Length == 2)
                    {
                        var rows = rowsAndColumns[0].Split(',');
                        foreach (var row in rows)
                        {
                            grid.RowDefinitions.Add(new RowDefinition { Height = (GridLength)converter.ConvertFromString(row) });
                        }

                        var columns = rowsAndColumns[1].Split(',');
                        foreach (var column in columns)
                        {
                            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = (GridLength)converter.ConvertFromString(column) });
                        }
                    }
                }
            }
        }

        public static DependencyProperty DoubleClickCommandProperty =
           DependencyProperty.RegisterAttached("DoubleClickCommand", typeof(ICommand), typeof(GridExtensions), new PropertyMetadata(DoubleClick_PropertyChanged));

        public static void SetDoubleClickCommand(UIElement element, ICommand value)
        {
            element.SetValue(DoubleClickCommandProperty, value);
        }

        public static ICommand GetDoubleClickCommand(UIElement element)
        {
            return (ICommand)element.GetValue(DoubleClickCommandProperty);
        }

        private static void DoubleClick_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var row = d as DataGridRow;
            if (row == null) return;

            if (e.NewValue != null)
            {
                row.AddHandler(DataGridRow.MouseDoubleClickEvent, new RoutedEventHandler(DataGrid_MouseDoubleClick));
            }
            else
            {
                row.RemoveHandler(DataGridRow.MouseDoubleClickEvent, new RoutedEventHandler(DataGrid_MouseDoubleClick));
            }
        }

        private static void DataGrid_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            var row = sender as DataGridRow;

            if (row != null)
            {
                var cmd = GetDoubleClickCommand(row);
                if (cmd.CanExecute(row.Item))
                    cmd.Execute(row.Item);
            }
        }
    }
}
