using SSMSPlusCore.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SSMSPlusCore.Ui.Controls.ComboCheckBox
{
    public class ComboCheckBoxViewModel<T> : ViewModelBase, IComboCheckBoxViewModel
    {
        public event EventHandler SelectionChanged;

        public ICommand CheckAllCommand { get; private set; }
        public ICommand CheckOneCommand { get; private set; }

        public BindingList<ComboCheckBoxItem<T>> Items { get; private set; }

        private string _text;
        public string Text
        {
            get => _text;
            private set => SetField(ref _text, value);
        }

        private bool _isAllChecked;
        public bool IsAllChecked
        {
            get => _isAllChecked;
            private set => SetField(ref _isAllChecked, value);
        }

        private bool _isAllVisible = true;
        public bool IsAllVisible
        {
            get => _isAllVisible;
            set => SetField(ref _isAllVisible, value);
        }

        public Func<string> ComputeTextFunc { get; set; }

        IEnumerable<IComboCheckBoxItem> IComboCheckBoxViewModel.Items => Items;

        public HashSet<T> GetSelectedValues()
        {
            return Items.Where(p => p.IsChecked).Select(p => p.Value).ToHashSet();
        }
        public ComboCheckBoxViewModel()
        {
            CheckAllCommand = new Command<bool>(OnCheckAllCommand);
            CheckOneCommand = new Command<bool>(OnCheckOneCommand);

            Items = new BindingList<ComboCheckBoxItem<T>>();

            Init();
        }

        private void Init()
        {            
            IsAllChecked = ComputeIsAllChecked();
            Items.ListChanged += new ListChangedEventHandler(Items_ListChanged);
            Text = ComputeText();
        }

        private string ComputeText()
        {
            if (ComputeTextFunc != null)
                return ComputeTextFunc();

            var selectionText = Items.Where(p => p.IsChecked).Select(p => p.Text);
            var itemsText = string.Join(", ", selectionText);
            if (IsAllChecked)
            {
                return "All : " + itemsText;
            }
            else
            {
                return itemsText;
            }
        }

        private bool ComputeIsAllChecked()
        {
            return Items.All(p => p.IsChecked);
        }

        private void Items_ListChanged(object sender, ListChangedEventArgs e)
        {       
            IsAllChecked = ComputeIsAllChecked();
            Text = ComputeText();
        }

        private void OnCheckAllCommand(bool isChecked)
        {
            Items.ForEach(p => p.IsChecked = isChecked);
            RaiseSelectionChanged();
        }

        private void OnCheckOneCommand(bool isChecked)
        {
            RaiseSelectionChanged();
        }

        private void RaiseSelectionChanged()
        {
            SelectionChanged?.Invoke(this, new EventArgs());
        }

    }
}
