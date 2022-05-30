using InputKit.Shared;

namespace SandboxMAUI.Pages;

public partial class SelectionViewPage : ContentPage
{
	public SelectionViewPage()
	{
		InitializeComponent();
        picker.ItemsSource = Enum.GetValues(typeof(SelectionType));
        picker.SelectedItem = SelectionType.Button;

        labelPositionPicker.ItemsSource = Enum.GetValues(typeof(LabelPosition));
        labelPositionPicker.SelectedItem = LabelPosition.Before;

        columnNumberPicker.SelectedItem = columnNumberPicker.Items.FirstOrDefault(x => x == "2");
    }

    private void Picker_SelectedIndexChanged(object sender, EventArgs e)
    {
        selectionView.SelectionType = (SelectionType)picker.SelectedItem;
    }

    private void LabelPositionChanged(object sender, EventArgs e)
    {
        if (sender is Picker pkr)
        {
            selectionView.LabelPosition = (LabelPosition)pkr.SelectedItem;
        }
    }

    private void ColumnNumberPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (sender is Picker picker)
        {
            selectionView.ColumnNumber = Convert.ToInt32(picker.SelectedItem);
        }
    }
}