using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Essentials;
using SandboxMAUI.Pages;

namespace SandboxMAUI
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        async void GoToCheckBoxPage(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new CheckBoxPage());
        }

        async void GoToRadioButtonPage(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new RadioButtonPage());
        }

        async void GoToAdvancedEntryPage(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new AdvancedEntryPage());
        }

        async void GoToAdvancedSliderPage(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new AdvancedSliderPage());
        }
    }
}
