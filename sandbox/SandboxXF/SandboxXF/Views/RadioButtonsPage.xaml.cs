using SandboxXF.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using RadioButton = Plugin.InputKit.Shared.Controls.RadioButton;

namespace SandboxXF.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RadioButtonsPage : ContentPage
	{
        static readonly Random rnd = new Random();
		public RadioButtonsPage ()
		{
			InitializeComponent ();
            BindingContext = new RadioButtonsViewModel();
        }

        private void RandomizeColors(object sender, EventArgs e)
        {
            var colors = typeof(Color).GetFields();
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
                if (rb.LabelPosition == Plugin.InputKit.Shared.LabelPosition.After)
                {
                    rb.LabelPosition = Plugin.InputKit.Shared.LabelPosition.Before;
                }
                else
                {
                    rb.LabelPosition = Plugin.InputKit.Shared.LabelPosition.After;
                }
            }
        }
    }
}