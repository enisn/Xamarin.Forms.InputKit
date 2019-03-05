using Plugin.InputKit.Shared.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sample.InputKit.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RadioButtonsPage : ContentPage
	{
        static readonly Random rnd = new Random();
		public RadioButtonsPage ()
		{
			InitializeComponent ();
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
    }
}