using Plugin.InputKit.Shared;
using Plugin.InputKit.Shared.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SandboxXF.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectionViewPage : ContentPage
    {
        public SelectionViewPage()
        {
            InitializeComponent();
            picker.ItemsSource = Enum.GetValues(typeof(SelectionType));
            picker.SelectedItem = SelectionType.Button;

            labelPositionPicker.SelectedItem = LabelPosition.Before;
            labelPositionPicker.ItemsSource = Enum.GetValues(typeof(LabelPosition));
        }

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectionView.SelectionType = (SelectionType)picker.SelectedItem;
        }

        private void LabelPositionChanged(object sender, EventArgs e)
        {
            if (sender is Picker pkr)
            {
                selectionView.LabelPosition = (LabelPosition)pkr.SelectedItem;
            }
        }
    }
}