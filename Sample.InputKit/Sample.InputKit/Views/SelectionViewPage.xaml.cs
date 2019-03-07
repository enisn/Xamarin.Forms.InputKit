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
    public partial class SelectionViewPage : ContentPage
    {
        public SelectionViewPage()
        {
            InitializeComponent();
            picker.ItemsSource = Enum.GetValues(typeof(SelectionType));
            picker.SelectedItem = SelectionType.Button;
        }

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectionView.SelectionType = (SelectionType)picker.SelectedItem;
        }
    }
}