﻿using Plugin.InputKit.Shared.Abstraction;
using Plugin.InputKit.Shared.Configuration;
using Plugin.InputKit.Shared.Helpers;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Plugin.InputKit.Shared.Controls
{
    /// <summary>
    /// A checkbox for boolean inputs. It Includes a text inside
    /// </summary>
    public class CheckBox : StackLayout, IValidatable
    {
        public static GlobalSetting GlobalSetting { get; private set; } = new GlobalSetting
        {
            BackgroundColor = Color.Transparent,
            Color = Color.Accent,
            BorderColor = Color.Black,
            TextColor = (Color)Label.TextColorProperty.DefaultValue,
            Size = 25,
            CornerRadius = -1,
            FontSize = 14,
        };
        #region Constants
        public const string RESOURCE_CHECK = "Plugin.InputKit.Shared.Resources.check.png";
        public const string RESOURCE_CROSS = "Plugin.InputKit.Shared.Resources.cross.png";
        public const string RESOURCE_STAR = "Plugin.InputKit.Shared.Resources.star.png";
        #endregion
        Frame boxBackground = new Frame { Padding = 0, CornerRadius = GlobalSetting.CornerRadius, InputTransparent = true, HeightRequest = GlobalSetting.Size, WidthRequest = GlobalSetting.Size, BackgroundColor = GlobalSetting.BackgroundColor, MinimumWidthRequest = 35, BorderColor = GlobalSetting.BorderColor, VerticalOptions = LayoutOptions.CenterAndExpand, HasShadow = false };
        BoxView boxSelected = new BoxView { IsVisible = false, HeightRequest = GlobalSetting.Size * .60, WidthRequest = GlobalSetting.Size * .60, Color = GlobalSetting.Color, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.Center };
        //Label lblSelected = new Label { Text = "✓", Margin = new Thickness(0, -1, 0, 0), FontSize = GlobalSetting.Size * .72, FontAttributes = FontAttributes.Bold, IsVisible = false, TextColor = GlobalSetting.Color, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.CenterAndExpand };
        IconView imgSelected = new IconView { Source = ImageSource.FromResource(RESOURCE_CHECK), FillColor = GlobalSetting.Color, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.CenterAndExpand };
        Label lblOption = new Label { VerticalOptions = LayoutOptions.CenterAndExpand, FontSize = GlobalSetting.FontSize, TextColor = GlobalSetting.TextColor, FontFamily = GlobalSetting.FontFamily, IsVisible = false };
        private CheckType _type = CheckType.Box;
        private bool _isEnabled;
        /// <summary>
        /// Default Constructor
        /// </summary>
        public CheckBox()
        {
            this.Orientation = StackOrientation.Horizontal;
            this.Padding = new Thickness(0, 10);
            this.Spacing = 10;
            boxBackground.Content = boxSelected;
            this.Children.Add(boxBackground);
            //this.Children.Add(new Grid { Children = { boxBackground, boxSelected }, MinimumWidthRequest = 35 });
            this.Children.Add(lblOption);
            this.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => { if (IsDisabled) return; IsChecked = !IsChecked; ExecuteCommand(); CheckChanged?.Invoke(this, new EventArgs()); ValidationChanged?.Invoke(this, new EventArgs()); }),
            });
        }

        void ExecuteCommand()
        {
            if (CheckChangedCommand?.CanExecute(CommandParameter ?? this) ?? false)
                CheckChangedCommand?.Execute(CommandParameter ?? this);
        }
        async void Animate()
        {
            try
            {
                await boxBackground.ScaleTo(0.9, 100, Easing.BounceIn);
                if (Type == CheckType.Material)
                    boxBackground.BackgroundColor = IsChecked ? this.Color : Color.Transparent;
                else
                    boxBackground.BorderColor = IsChecked ? this.Color : this.BorderColor;
                await boxBackground.ScaleTo(1, 100, Easing.BounceIn);
            }
            catch (Exception)
            {
            }
        }
        /// <summary>
        /// Invoked when check changed
        /// </summary>
        public event EventHandler CheckChanged;
        public event EventHandler ValidationChanged;
        /// <summary>
        /// Executed when check changed
        /// </summary>
        public ICommand CheckChangedCommand { get; set; }
        /// <summary>
        /// Command Parameter for Commands. If this is null, CommandParameter will be sent as itself of CheckBox
        /// </summary>
        public object CommandParameter { get => GetValue(CommandParameterProperty); set => SetValue(CommandParameterProperty, value); }
        /// <summary>
        /// Quick generator constructor
        /// </summary>
        /// <param name="optionName">Text to Display</param>
        /// <param name="key">Value to keep it like an ID</param>
        public CheckBox(string optionName, int key) : this()
        {
            Key = key;
            Text = optionName;
        }
        /// <summary>
        /// You can set a Unique key for each control
        /// </summary>
        public int Key { get; set; }
        /// <summary>
        /// Text to display right of CheckBox
        /// </summary>
        public string Text { get => lblOption.Text; set { lblOption.Text = value; lblOption.IsVisible = !String.IsNullOrEmpty(value); } }
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
                Animate();
            }
        }
        /// <summary>
        /// Checkbox box background color. Default is LightGray
        /// </summary>
        public Color BoxBackgroundColor { get => (Color)GetValue(BoxBackgroundColorProperty); set => SetValue(BoxBackgroundColorProperty, value); }
        /// <summary>
        /// Gets or sets the checkbutton enabled or not. If checkbox is disabled, checkbox can not be interacted.
        /// </summary>
        public bool IsDisabled { get => _isEnabled; set { _isEnabled = value; this.Opacity = value ? 0.6 : 1; } }
        /// <summary>
        /// Color of Checkbox checked
        /// </summary>
        public Color Color
        {
            get => (Color)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }
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
        public Color BorderColor { get => (Color)GetValue(BorderColorProperty); set => SetValue(BorderColorProperty, value); }
        /// <summary>
        /// WARNING! : If you set this as required, user must set checked this control to be validated!
        /// </summary>
        public bool IsRequired { get; set; }
        /// <summary>
        /// Checks if entry required and checked
        /// </summary>
        public bool IsValidated => !this.IsRequired || this.IsChecked;
        /// <summary>
        /// Not available for this control
        /// </summary>
        public string ValidationMessage { get; set; }
        /// <summary>
        /// Fontfamily of CheckBox Text
        /// </summary>
        public string FontFamily { get => lblOption.FontFamily; set => lblOption.FontFamily = value; }

        public ImageSource CustomIcon { get => (ImageSource)GetValue(CustomIconProperty); set => SetValue(CustomIconProperty, value); }
        #region BindableProperties
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static readonly BindableProperty ColorProperty = BindableProperty.Create(nameof(Color), typeof(Color), typeof(CheckBox), Color.Accent, propertyChanged: (bo, ov, nv) => (bo as CheckBox).UpdateColor());
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(CheckBox), GlobalSetting.TextColor, propertyChanged: (bo, ov, nv) => (bo as CheckBox).TextColor = (Color)nv);
        public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(nameof(IsChecked), typeof(bool), typeof(CheckBox), false, BindingMode.TwoWay, propertyChanged: (bo, ov, nv) => (bo as CheckBox).IsChecked = (bool)nv);
        public static readonly BindableProperty IsDisabledProperty = BindableProperty.Create(nameof(IsDisabled), typeof(bool), typeof(CheckBox), false, propertyChanged: (bo, ov, nv) => (bo as CheckBox).IsDisabled = (bool)nv);
        public static readonly BindableProperty KeyProperty = BindableProperty.Create(nameof(Key), typeof(int), typeof(CheckBox), 0, propertyChanged: (bo, ov, nv) => (bo as CheckBox).Key = (int)nv);
        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(CheckBox), "", propertyChanged: (bo, ov, nv) => (bo as CheckBox).Text = (string)nv);
        public static readonly BindableProperty CheckChangedCommandProperty = BindableProperty.Create(nameof(CheckChangedCommand), typeof(ICommand), typeof(CheckBox), null, propertyChanged: (bo, ov, nv) => (bo as CheckBox).CheckChangedCommand = (ICommand)nv);
        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(CheckBox), null);
        public static readonly BindableProperty BoxBackgroundColorProperty = BindableProperty.Create(nameof(BoxBackgroundColor), typeof(Color), typeof(CheckBox), GlobalSetting.BackgroundColor, propertyChanged: (bo, ov, nv) => (bo as CheckBox).UpdateBoxBackground());
        public static readonly BindableProperty TextFontSizeProperty = BindableProperty.Create(nameof(TextFontSize), typeof(double), typeof(CheckBox), 14.0, propertyChanged: (bo, ov, nv) => (bo as CheckBox).TextFontSize = (double)nv);
        public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(CheckBox), GlobalSetting.BorderColor, propertyChanged: (bo, ov, nv) => (bo as CheckBox).UpdateBorderColor());
        public static readonly BindableProperty CustomIconProperty = BindableProperty.Create(nameof(CustomIcon), typeof(ImageSource), typeof(CheckBox), default(ImageSource), propertyChanged: (bo, ov, nv) => (bo as CheckBox).UpdateType((bo as CheckBox).Type));
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        #endregion
        #region Methods
        void UpdateBoxBackground()
        {
            if (this.Type == CheckType.Material)
                return;
            boxBackground.BackgroundColor = BoxBackgroundColor;
        }
        void UpdateColor()
        {
            boxSelected.Color = Color;
            if (Type == CheckType.Material)
            {
                boxBackground.BorderColor = Color;
                boxBackground.BackgroundColor = IsChecked ? Color : Color.Transparent;
                imgSelected.FillColor = Color.ToSurfaceColor();
            }
            else
            {
                boxBackground.BorderColor = BorderColor;
                boxBackground.BackgroundColor = BackgroundColor;
                imgSelected.FillColor = Color;
            }
        }
        void UpdateBorderColor()
        {
            if (this.Type == CheckType.Material) return;
            boxBackground.BorderColor = this.BorderColor;
        }
        void UpdateAllColors()
        {
            UpdateColor();
            UpdateBoxBackground();
            UpdateBorderColor();
        }
        void SetBoxSize(double value)
        {
            boxBackground.WidthRequest = value;
            boxBackground.HeightRequest = value;
            boxSelected.WidthRequest = value * .6;  //old value 0.72
            boxSelected.HeightRequest = value * 0.6;
            //lblSelected.FontSize = value * 0.72;       //old value 0.76 //TODO: Do something to resizing
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
                    imgSelected.Source = ImageSource.FromResource(RESOURCE_CHECK);
                    boxBackground.Content = imgSelected;
                    break;
                case CheckType.Cross:
                    imgSelected.Source = ImageSource.FromResource(RESOURCE_CROSS);
                    boxBackground.Content = imgSelected;
                    break;
                case CheckType.Star:
                    imgSelected.Source = ImageSource.FromResource(RESOURCE_STAR);
                    boxBackground.Content = imgSelected;
                    break;
                case CheckType.Material:
                    imgSelected.Source = ImageSource.FromResource(RESOURCE_CHECK);
                    boxBackground.CornerRadius = 5;
                    boxBackground.Content = imgSelected;
                    break;
                case CheckType.Custom:
                    imgSelected.Source = CustomIcon;
                    boxBackground.Content = imgSelected;
                    break;
            }
            UpdateAllColors();
        }
        /// <summary>
        /// Not available for this control
        /// </summary>
        public void DisplayValidation()
        {

        }
        #endregion

        public enum CheckType
        {
            Box,
            Check,
            Cross,
            Star,
            Material,
            Custom = 90
        }
    }
}
