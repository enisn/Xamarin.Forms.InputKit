using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CheckBox = Plugin.InputKit.Shared.Controls.CheckBox;

namespace Sample.InputKit.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CheckBoxesPage : ContentPage
	{
        static readonly Random rnd = new Random();
		public CheckBoxesPage()
		{
			InitializeComponent ();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            var colors = typeof(Color).GetFields();
            var color = (Color)colors[rnd.Next(colors.Length)].GetValue(null);
            foreach (var view in mainLayout.Children)
            {
                if (view is CheckBox chk)
                {
                    chk.Color = color;
                }
            }
        }

        private void ChangePosition(object sender, EventArgs e)
        {
            if (sender is CheckBox cb)
            {
                cb.LabelPosition = cb.IsChecked ? Plugin.InputKit.Shared.LabelPosition.After :
                                                  Plugin.InputKit.Shared.LabelPosition.Before;
            }
        }
    }
}