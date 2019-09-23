using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMSPlusCore.Ui.Controls.ComboCheckBox
{
    public class ComboCheckBoxItem<T> : ViewModelBase, IComboCheckBoxItem
    {
        private string _text;
        public string Text
        {
            get => _text;
            set => SetField(ref _text, value);
        }

        private bool _isChecked;
        public bool IsChecked
        {
            get => _isChecked;
            set => SetField(ref _isChecked, value);
        }

        private T _value;
        public T Value
        {
            get => _value;
            set => SetField(ref _value, value);
        }

        object IComboCheckBoxItem.Value { get => Value; set => Value = (T)value; }
    }
}
