using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using RadioButton = InputKit.Shared.Controls.RadioButton;

namespace SandboxMAUI.Pages;

public partial class RadioButtonPage : ContentPage
{
	public RadioButtonPage()
	{
		InitializeComponent();
	}

    private static Random rnd = new Random();

    private void RandomizeColors(object sender, EventArgs e)
    {
        var colors = typeof(Colors).GetFields();
        var color = (Color)colors[rnd.Next(colors.Length)].GetValue(null);
        foreach (var view in groupView.Children)
        {
            if (view is RadioButton rb)
            {
                rb.Color = color;
            }
        }

        if (sender is Button button)
        {
            button.BackgroundColor = color;
        }
    }
}