using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Plugin.InputKit.Shared.Controls
{
    public class Dropdown : Frame
    {
        CenteredPicker picker = new CenteredPicker();
        IconView imgIcon = new IconView { FillColor = Color.Accent,Margin=10, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.End, Source= "https://cdn3.iconfinder.com/data/icons/faticons/32/arrow-down-01-512.png" };
        public Dropdown()
        {
            this.Content = new Grid
            {
                Children =
                {
                    picker,
                    imgIcon
                }
            };
        }



        public event EventHandler Picked;
        public ICommand PickedCommand { get; set; }
        public string Title { get => picker.Title; set => picker.Title = value; }
        public object SelectedItem { get => picker.SelectedItem; set => picker.SelectedItem = value; }
        public System.Collections.IList ItemSource { get => picker.ItemsSource; set => picker.ItemsSource = value; }
        public BindingBase ItemDisplayBinding { get => picker.ItemDisplayBinding; set => picker.ItemDisplayBinding = value; }
        public Color IconColor { get => imgIcon.FillColor; set => imgIcon.FillColor = value; }
        public string IconSource { get => imgIcon.Source; set => imgIcon.Source = value; }


        public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(Dropdown), null, propertyChanged: (bo, ov, nv) => (bo as Dropdown).Title = (string)nv);
        public static readonly BindableProperty SelectedItemProperty =BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(Dropdown), null, BindingMode.TwoWay, propertyChanged: (bo, ov, nv) => (bo as Dropdown).SelectedItem = nv);
        public static readonly BindableProperty ItemSourceProperty =BindableProperty.Create(nameof(ItemSource), typeof(System.Collections.IList), typeof(Dropdown), null, propertyChanged: (bo, ov, nv) => (bo as Dropdown).ItemSource = (System.Collections.IList)nv);
        public static readonly BindableProperty ItemDisplayBindingProperty =BindableProperty.Create(nameof(ItemDisplayBinding), typeof(BindingBase), typeof(Dropdown), null, propertyChanged: (bo, ov, nv) => (bo as Dropdown).ItemDisplayBinding = (BindingBase)nv);
        public static readonly BindableProperty PickedCommandProperty =BindableProperty.Create(nameof(PickedCommand), typeof(Command), typeof(Dropdown), null, propertyChanged: (bo, ov, nv) => (bo as Dropdown).PickedCommand = (Command)nv);
        public static readonly BindableProperty IconColorProperty = BindableProperty.Create(nameof(IconColor), typeof(Color), typeof(Dropdown), Color.Accent, propertyChanged: (bo, ov, nv) => (bo as Dropdown).IconColor = (Color)nv);
        

    }






    internal class CenteredPicker : Picker
    {
        public TextAlignment TextAlingn { get => (TextAlignment)GetValue(TextAlignProperty); set { SetValue(TextAlignProperty, value); OnPropertyChanged(nameof(TextAlingn)); } }

        public static readonly BindableProperty TextAlignProperty =
            BindableProperty.Create(nameof(TextAlingn), typeof(TextAlignment), typeof(CenteredPicker), TextAlignment.Center, BindingMode.OneWay);

    }
}
