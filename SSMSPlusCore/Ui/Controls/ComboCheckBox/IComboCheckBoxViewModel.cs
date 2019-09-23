using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;

namespace SSMSPlusCore.Ui.Controls.ComboCheckBox
{
    public interface IComboCheckBoxViewModel
    {
        ICommand CheckAllCommand { get; }
        ICommand CheckOneCommand { get; }
        Func<string> ComputeTextFunc { get; set; }
        bool IsAllChecked { get; }
        bool IsAllVisible { get; set; }
        IEnumerable<IComboCheckBoxItem> Items { get; }
        string Text { get; }

        event EventHandler SelectionChanged;

    }
}