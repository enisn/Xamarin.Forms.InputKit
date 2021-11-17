using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Essentials;
using Microsoft.Maui.Graphics;
using RadioButton = InputKit.Shared.Controls.RadioButton;

namespace SandboxMAUI
{
	public partial class MainPage : TabbedPage
	{
        readonly Random rnd = new Random();

		public MainPage()
		{
			InitializeComponent();
            iconView.Source = ImageSource.FromResource(RadioButton.RESOURCE_DOT);
            iconView.WidthRequest = 50;
            iconView.HeightRequest = 50;
            iconView.FillColor = Colors.Red;
		}

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
        }

        private void ChangePosition(object sender, EventArgs e)
        {
            if (sender is RadioButton rb)
            {
                if (rb.LabelPosition == InputKit.Shared.LabelPosition.After)
                {
                    rb.LabelPosition = InputKit.Shared.LabelPosition.Before;
                }
                else
                {
                    rb.LabelPosition = InputKit.Shared.LabelPosition.After;
                }
            }
        }
    }
}
