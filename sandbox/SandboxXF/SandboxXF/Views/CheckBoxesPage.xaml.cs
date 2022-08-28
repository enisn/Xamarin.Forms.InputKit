using Plugin.InputKit.Shared.Helpers;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CheckBox = Plugin.InputKit.Shared.Controls.CheckBox;

namespace SandboxXF.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CheckBoxesPage : ContentPage
    {
        static readonly Random rnd = new Random();
        public CheckBoxesPage()
        {
            InitializeComponent();
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

            if (sender is Button button)
            {
                button.BackgroundColor = color;
                button.TextColor = color.ToSurfaceColor();
            }
        }
    }
}