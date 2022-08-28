using InputKit.Shared.Helpers;
using CheckBox = InputKit.Shared.Controls.CheckBox;

namespace SandboxMAUI.Pages;

public partial class CheckBoxPage : ContentPage
{
	public CheckBoxPage()
	{
		InitializeComponent();
	}
    Random rnd = new Random();
    private void Button_Clicked(object sender, EventArgs e)
    {
        var colors = typeof(Colors).GetFields();
        var color = (Color)colors[rnd.Next(colors.Length)].GetValue(null);
        foreach (var view in mainLayout.Children)
        {
            if (view is CheckBox chk)
            {
                chk.Color = color;
            }
        }

        if(sender is Button button)
        {
            button.BackgroundColor = color;
            button.TextColor = color.ToSurfaceColor();
        }
    }
}