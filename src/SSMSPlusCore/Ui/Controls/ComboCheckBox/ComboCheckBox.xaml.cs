using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace SSMSPlusCore.Ui.Controls.ComboCheckBox
{
    /// <summary>
    /// Interaction logic for ComboCheckBox.xaml
    /// </summary>
    public partial class ComboCheckBox : UserControl
    {
        public static readonly DependencyProperty ViewModelProperty =
         DependencyProperty.Register(
        "ViewModel",
        typeof(IComboCheckBoxViewModel),
        typeof(ComboCheckBox),
        new PropertyMetadata(null));

        public IComboCheckBoxViewModel ViewModel
        {
            get => (IComboCheckBoxViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        public ComboCheckBox()
        {
            InitializeComponent();
        }

        private void StackPanel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!(e.OriginalSource is CheckBox))
                e.Handled = true;
        }
    }
}
