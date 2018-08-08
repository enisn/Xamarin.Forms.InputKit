using Plugin.InputKit.Shared.Abstraction;
using Plugin.InputKit.Shared.Configuration;
using Plugin.InputKit.Shared.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using Xamarin.Forms;

namespace Plugin.InputKit.Shared.Controls
{
    /// <summary>
    /// A dropdown picker
    /// </summary>
    public class Dropdown : StackLayout, IValidatable
    {
        public static GlobalSetting GlobalSetting { get; private set; } = new GlobalSetting
        {
            BackgroundColor = Color.White,
            CornerRadius = 20,
            BorderColor = (Color)Button.BorderColorProperty.DefaultValue,
            Color = Color.Accent,
            FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Button)),
            Size = -1,
            TextColor = Color.Black,
        };

        IconView imgIcon = new IconView { InputTransparent = true, FillColor = GlobalSetting.Color, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.CenterAndExpand, Margin = 5 };
        IconView imgArrow = new IconView { InputTransparent = true, FillColor = GlobalSetting.Color, Source = "arrow_down.png", HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.CenterAndExpand, Margin = 5 };
        Label lblTitle = new Label { Margin = new Thickness(6, 0, 0, 0), IsVisible = false, TextColor = GlobalSetting.TextColor, LineBreakMode = LineBreakMode.TailTruncation, FontFamily = GlobalSetting.FontFamily };
        Label lblAnnotation = new Label { Margin = new Thickness(6, 0, 0, 0), IsVisible = false, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)), Opacity = 0.8, TextColor = GlobalSetting.TextColor, FontFamily = GlobalSetting.FontFamily };
        Button btnBackground = new Button { BackgroundColor = GlobalSetting.BackgroundColor, CornerRadius = (int)GlobalSetting.CornerRadius, BorderColor = GlobalSetting.BorderColor, TextColor = GlobalSetting.TextColor, FontFamily = GlobalSetting.FontFamily };

        PopupMenu pMenu = new PopupMenu();
        private string _placeholder;
        private string _validationMessage;
        private bool _isRequired;

        public Dropdown()
        {
            this.Children.Add(lblTitle);
            this.Children.Add(lblAnnotation);

            this.Children.Add(new Grid
            {
                Children =
                {
                    btnBackground,
                    imgIcon,
                    imgArrow
                }
            });
            btnBackground.Clicked += Menu_Requested;
            pMenu.OnItemSelected += Menu_Item_Selected;
            UpdateMainText();
        }
        public event EventHandler ValidationChanged;
        #region SelectionRegion
        private void Menu_Requested(object sender, EventArgs e)
        {
            pMenu.ShowPopup(imgArrow);
        }
        private void Menu_Item_Selected(string item, int index)
        {
            try
            {
                SelectedItem = ItemsSource[index];
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }
        public IList ItemsSource
        {
            get => pMenu.ItemsSource;
            set
            {
                pMenu.ItemsSource = value;
                if (value is INotifyCollectionChanged)
                    (value as INotifyCollectionChanged).CollectionChanged += Dropdown_CollectionChanged;
            }
        }
        public object SelectedItem { get => GetValue(SelectedItemProperty); set => SetValue(SelectedItemProperty, value); }
        private void Dropdown_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            pMenu.ItemsSource = (IList)GetValue(ItemsSourceProperty);
        }
        void UpdateSelected()
        {
            UpdateMainText();
            DisplayValidation();
            ValidationChanged?.Invoke(this, new EventArgs());
        }
        #endregion

        public string Title { get => lblTitle.Text; set { lblTitle.Text = value; lblTitle.IsVisible = !String.IsNullOrEmpty(value); } }
        public string IconImage { get => imgIcon.Source; set => imgIcon.Source = value; }
        public string FontFamily { get => btnBackground.FontFamily; set { btnBackground.FontFamily = value; lblTitle.FontFamily = value; lblAnnotation.FontFamily = value; } }
        public new Color BackgroundColor { get => btnBackground.BackgroundColor; set => btnBackground.BackgroundColor = value; }
        public Color Color { get => imgIcon.FillColor; set => UpdateColors(); }
        public Color TextColor { get => (Color)GetValue(TextColorProperty); set => SetValue(TextColorProperty, value); }
        public Color AnnotationColor { get => lblAnnotation.TextColor; set => lblAnnotation.TextColor = value; }
        public Color TitleColor { get => lblTitle.TextColor; set => lblTitle.TextColor = value; }
        public Color BorderColor { get => btnBackground.BorderColor; set { btnBackground.BorderColor = value; btnBackground.BorderWidth = value == (Color)Button.BorderColorProperty.DefaultValue ? (double)Button.BorderWidthProperty.DefaultValue : 2; } }
        public int CornerRadius { get => btnBackground.CornerRadius; set => btnBackground.CornerRadius = value; }
        public string Placeholder { get => _placeholder; set { _placeholder = value; UpdateMainText(); } }

        public bool IsRequired { get => _isRequired; set { _isRequired = value; DisplayValidation(); } }

        public bool IsValidated => !IsRequired || SelectedItem != null;

        public string ValidationMessage { get => _validationMessage; set { _validationMessage = value; DisplayValidation(); } }

        void UpdateColors()
        {
            imgIcon.FillColor = Color;
            imgArrow.FillColor = Color;
        }
        void UpdateMainText()
        {
            btnBackground.Text = SelectedItem == null ? Placeholder : SelectedItem.ToString();
            btnBackground.TextColor = SelectedItem == null ? TextColor.MultiplyAlpha(0.5) : TextColor.MultiplyAlpha(1);
        }

        public void DisplayValidation()
        {
            lblAnnotation.Text = IsValidated ? null : ValidationMessage;
            lblAnnotation.IsVisible = !String.IsNullOrEmpty(lblAnnotation.Text);
        }
        #region BindableProperties
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IList), typeof(Dropdown), propertyChanged: (bo, ov, nv) => (bo as Dropdown).ItemsSource = (IList)nv);
        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(Dropdown), null, BindingMode.TwoWay, propertyChanged: (bo, ov, nv) => (bo as Dropdown).UpdateSelected());
        public new static readonly BindableProperty BackgroundColorProperty = BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(Dropdown), Color.White, propertyChanged: (bo, ov, nv) => (bo as Dropdown).BackgroundColor = (Color)nv);
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(Dropdown), GlobalSetting.TextColor, propertyChanged: (bo, ov, nv) => (bo as Dropdown).UpdateMainText());
        public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(Dropdown), propertyChanged: (bo, ov, nv) => (bo as Dropdown).Title = (string)nv);
        public static readonly BindableProperty IconImageProperty = BindableProperty.Create(nameof(IconImage), typeof(string), typeof(Dropdown), propertyChanged: (bo, ov, nv) => (bo as Dropdown).IconImage = (string)nv);
        public static readonly BindableProperty AnnotationColorProperty = BindableProperty.Create(nameof(AnnotationColor), typeof(Color), typeof(Dropdown), GlobalSetting.Color, propertyChanged: (bo, ov, nv) => (bo as Dropdown).AnnotationColor = (Color)nv);
        public static readonly BindableProperty TitleColorProperty = BindableProperty.Create(nameof(TitleColor), typeof(Color), typeof(Dropdown), GlobalSetting.TextColor, propertyChanged: (bo, ov, nv) => (bo as Dropdown).TitleColor = (Color)nv);
        public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(Dropdown), GlobalSetting.BorderColor, propertyChanged: (bo, ov, nv) => (bo as Dropdown).BorderColor = (Color)nv);
        public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(Dropdown), null, propertyChanged: (bo, ov, nv) => (bo as Dropdown).Placeholder = (string)nv);
        public static readonly BindableProperty IsRequiredProperty = BindableProperty.Create(nameof(IsRequired), typeof(bool), typeof(Dropdown), false, propertyChanged: (bo, ov, nv) => (bo as Dropdown).IsRequired = (bool)nv);
        public static readonly BindableProperty ValidationMessageProperty = BindableProperty.Create(nameof(ValidationMessage), typeof(string), typeof(Dropdown), null, propertyChanged: (bo, ov, nv) => (bo as Dropdown).ValidationMessage = (string)nv);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        #endregion
    }
}
