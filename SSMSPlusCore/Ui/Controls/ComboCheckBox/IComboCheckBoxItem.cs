namespace SSMSPlusCore.Ui.Controls.ComboCheckBox
{
    public interface IComboCheckBoxItem
    {
        bool IsChecked { get; set; }
        string Text { get; set; }
        object Value { get; set; }
    }
}