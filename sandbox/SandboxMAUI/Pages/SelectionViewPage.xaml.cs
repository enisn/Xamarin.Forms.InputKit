using InputKit.Shared;

namespace SandboxMAUI.Pages;

public partial class SelectionViewPage : ContentPage
{
	public SelectionViewPage()
	{
		InitializeComponent();
        picker.ItemsSource = Enum.GetValues(typeof(SelectionType));
        picker.SelectedItem = SelectionType.Button;

        labelPositionPicker.SelectedItem = LabelPosition.Before;
        labelPositionPicker.ItemsSource = Enum.GetValues(typeof(LabelPosition));
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
}