using Plugin.InputKit.Shared.Configuration;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Sample.InputKit
{
    /// <summary>
    /// A checkbox for boolean inputs. It Includes a text inside
    /// </summary>
    public class CheckBox : StackLayout
    {
        public static GlobalSetting GlobalSetting { get; private set; } = new GlobalSetting
        {
            BackgroundColor = Color.Transparent,
            Color = Color.Accent,
            //BorderColor = Color.Black,
            Size = 25,
            CornerRadius = -1,
            FontSize = 14,
        };


        Frame boxBackground = new Frame { Padding = 0, InputTransparent = true, BackgroundColor = GlobalSetting.BackgroundColor, BorderColor = Color.Black, VerticalOptions = LayoutOptions.CenterAndExpand };
        BoxView boxSelected = new BoxView { IsVisible = false, Color = GlobalSetting.Color, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.Center };
        Label lblSelected = new Label { Text = "✓", FontAttributes = FontAttributes.Bold, IsVisible = false, TextColor = Color.Accent, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.CenterAndExpand };
        Label lblOption = new Label { VerticalOptions = LayoutOptions.CenterAndExpand, FontSize = GlobalSetting.FontSize };
        private CheckType _type = CheckType.Box;
        private bool _isEnabled;
        /// <summary>
        /// Default Constructor
        /// </summary>
        public CheckBox()
        {
            this.Orientation = StackOrientation.Horizontal;
            this.Margin = new Thickness(10, 0);
            this.Padding = new Thickness(10);
            this.Spacing = 10;
            boxBackground.Content = boxSelected;
            this.Children.Add(boxBackground);
            //this.Children.Add(new Grid { Children = { boxBackground, boxSelected }, MinimumWidthRequest = 35 });
            this.Children.Add(lblOption);
            this.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => { if (IsDisabled) return; IsChecked = !IsChecked; CheckChanged?.Invoke(this, new EventArgs()); CheckChangedCommand?.Execute(this.IsChecked); }),
            });
        }
        /// <summary>
        /// Invoked when check changed
        /// </summary>
        public event EventHandler CheckChanged;
        /// <summary>
        /// Executed when check changed
        /// </summary>
        public ICommand CheckChangedCommand { get; set; }
        /// <summary>
        /// Quick generator constructor
        /// </summary>
        /// <param name="optionName">Text to Display</param>
        /// <param name="key">Value to keep it like an ID</param>
        public CheckBox(string optionName, int key) : this()
        {
            Key = key;
            lblOption.Text = optionName;
        }
        /// <summary>
        /// You can set a Unique key for each control
        /// </summary>
        public int Key { get; set; }
        /// <summary>
        /// Text to display right of CheckBox
        /// </summary>
        public string Text { get => lblOption.Text; set => lblOption.Text = value; }
        /// <summary>
        /// IsChecked Property
        /// </summary>
        public bool IsChecked
        {
            get => boxBackground.Content.IsVisible;
            set
            {
                boxBackground.Content.IsVisible = value;
                SetValue(IsCheckedProperty, value);
            }
        }
        /// <summary>
        /// Checkbox box background color. Default is LightGray
        /// </summary>
        public Color BoxBackgroundColor { get => boxBackground.BackgroundColor; set => boxBackground.BackgroundColor = value; }
        /// <summary>
        /// Gets or sets the checkbutton enabled or not. If checkbox is disabled, checkbox can not be interacted.
        /// </summary>
        public bool IsDisabled { get => _isEnabled; set { _isEnabled = value; this.Opacity = value ? 0.6 : 1; } }
        /// <summary>
        /// Color of Checkbox checked
        /// </summary>
        public Color Color { get => boxSelected.BackgroundColor; set { boxSelected.BackgroundColor = value; lblSelected.TextColor = value; } }
        /// <summary>
        /// Color of text
        /// </summary>
        public Color TextColor { get => lblOption.TextColor; set => lblOption.TextColor = value; }
        /// <summary>
        /// Which icon will be shown when checkbox is checked
        /// </summary>
        public CheckType Type { get => _type; set { _type = value; UpdateType(value); } }
        /// <summary>
        /// Size of Checkbox
        /// </summary>
        public double BoxSize { get => boxBackground.Width; }
        /// <summary>
        /// SizeRequest of CheckBox
        /// </summary>
        public double BoxSizeRequest { get => boxBackground.WidthRequest; set => SetBoxSize(value); }
        /// <summary>
        /// Fontsize of Checkbox text
        /// </summary>
        public double TextFontSize { get => lblOption.FontSize; set => lblOption.FontSize = value; }
        /// <summary>
        /// Border color of around CheckBox
        /// </summary>
        public Color BorderColor { get => boxBackground.BorderColor; set => boxBackground.BorderColor = value; }

        #region BindableProperties
        public static readonly BindableProperty ColorProperty = BindableProperty.Create(nameof(Color), typeof(Color), typeof(CheckBox), Color.Accent, propertyChanged: (bo, ov, nv) => (bo as CheckBox).Color = (Color)nv);
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(CheckBox), Color.Gray, propertyChanged: (bo, ov, nv) => (bo as CheckBox).TextColor = (Color)nv);
        public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(nameof(IsChecked), typeof(bool), typeof(CheckBox), false, BindingMode.TwoWay, propertyChanged: (bo, ov, nv) => (bo as CheckBox).IsChecked = (bool)nv);
        public static readonly BindableProperty IsDisabledProperty = BindableProperty.Create(nameof(IsDisabled), typeof(bool), typeof(CheckBox), false, propertyChanged: (bo, ov, nv) => (bo as CheckBox).IsDisabled = (bool)nv);
        public static readonly BindableProperty KeyProperty = BindableProperty.Create(nameof(Key), typeof(int), typeof(CheckBox), 0, propertyChanged: (bo, ov, nv) => (bo as CheckBox).Key = (int)nv);
        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(CheckBox), "", propertyChanged: (bo, ov, nv) => (bo as CheckBox).Text = (string)nv);
        public static readonly BindableProperty CheckChangedCommandProperty = BindableProperty.Create(nameof(CheckChangedCommand), typeof(ICommand), typeof(CheckBox), null, propertyChanged: (bo, ov, nv) => (bo as CheckBox).CheckChangedCommand = (ICommand)nv);
        public static readonly BindableProperty BoxBackgroundColorProperty = BindableProperty.Create(nameof(BoxBackgroundColor), typeof(Color), typeof(CheckBox), Color.Gray, propertyChanged: (bo, ov, nv) => (bo as CheckBox).BoxBackgroundColor = (Color)nv);
        public static readonly BindableProperty TextFontSizeProperty = BindableProperty.Create(nameof(TextFontSize), typeof(double), typeof(CheckBox), 14.0, propertyChanged: (bo, ov, nv) => (bo as CheckBox).TextFontSize = (double)nv);
        #endregion
        void SetBoxSize(double value)
        {
            boxBackground.WidthRequest = value;
            boxBackground.HeightRequest = value;
            boxSelected.WidthRequest = value * 0.65;  //old value 0.72
            boxSelected.HeightRequest = value * 0.65;
            lblSelected.FontSize = value * 0.72;       //old value 0.76
            this.Children[0].MinimumWidthRequest = value * 1.4;
        }

        void UpdateType(CheckType _Type)
        {
            switch (_Type)
            {
                case CheckType.Box:
                    boxBackground.Content = boxSelected;
                    break;
                case CheckType.Check:
                    lblSelected.Text = "✓";
                    boxBackground.Content = lblSelected;
                    break;
                case CheckType.Cross:
                    lblSelected.Text = "✕";
                    boxBackground.Content = lblSelected;

                    break;
                case CheckType.Star:
                    lblSelected.Text = "★";
                    boxBackground.Content = lblSelected;
                    break;
            }
        }
        public enum CheckType
        {
            Box,
            Check,
            Cross,
            Star
        }
    }
}
