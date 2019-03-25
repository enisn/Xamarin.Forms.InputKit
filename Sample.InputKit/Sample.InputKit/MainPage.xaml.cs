using Plugin.InputKit.Shared.Controls;
using Sample.InputKit.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Sample.InputKit
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void AdvancedEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(e.NewTextValue);
        }

        private void CheckBoxes_Clicked(object sender, EventArgs e) => Navigation.PushAsync(new CheckBoxesPage());

        private void RadioButons_Clicked(object sender, EventArgs e) => Navigation.PushAsync(new RadioButtonsPage());

        private void AutoCompleteEntries_Clicked(object sender, EventArgs e) => Navigation.PushAsync(new AutoCompleteEntriesPage());

        private void Dropdowns_Clicked(object sender, EventArgs e) => Navigation.PushAsync(new DropdownsPage());

        private void AdvancedEntries_Clicked(object sender, EventArgs e) => Navigation.PushAsync(new AdvancedEntriesPage());

        private void AdvancedSliders_Clicked(object sender, EventArgs e) => Navigation.PushAsync(new AdvancedSlidersPage());

        private void SelectionView_Clicked(object sender, EventArgs e) => Navigation.PushAsync(new SelectionViewPage());
        
    }
}
