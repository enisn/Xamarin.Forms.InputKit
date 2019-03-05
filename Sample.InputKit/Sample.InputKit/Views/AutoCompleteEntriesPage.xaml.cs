using Plugin.InputKit.Shared.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sample.InputKit.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AutoCompleteEntriesPage : ContentPage
    {
        ObservableCollection<string> source = new ObservableCollection<string>();
        public AutoCompleteEntriesPage()
        {
            try
            {
                InitializeComponent();
                autoCompleteEntry.ItemsSource = source;
                source.Add("Option 1");
                source.Add("Option 2");
                source.Add("Test 1");
                source.Add("Test 2");
                source.Add("Test 3");
                source.Add("Test 4");
                source.Add("Sample");
                source.Add("Xamarin");
                source.Add("Xamarin Forms");
                source.Add("Xamarin Droid");
                source.Add("Xamarin Android");
                source.Add("Xamarin iOS");
                source.Add("Xamarin Apple");
                source.Add("Xamarin Mac");
                source.Add("Xamarin UWP");
            }
            catch (Exception e)
            {
                DisplayAlert("Error:", e.ToString(), "ok");
            }
        }       
    }
}