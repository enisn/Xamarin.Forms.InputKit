using Plugin.InputKit.Shared.Abstraction;
using Plugin.InputKit.Shared.Configuration;
using Plugin.InputKit.Shared.Helpers;
using Plugin.InputKit.Shared.Layouts;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Plugin.InputKit.Shared.Controls
{
    /// <summary>
    /// A checkbox for boolean inputs. It Includes a text inside
    /// </summary>
    public partial class CheckBox : StatefulStackLayout, IValidatable
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

        #region Fields
        internal Frame frmBackground = new Frame { Padding = 0, CornerRadius = GlobalSetting.CornerRadius, InputTransparent = true, HeightRequest = GlobalSetting.Size, WidthRequest = GlobalSetting.Size, BackgroundColor = GlobalSetting.BackgroundColor, MinimumWidthRequest = 35, BorderColor = GlobalSetting.BorderColor, VerticalOptions = LayoutOptions.CenterAndExpand, HasShadow = false };
        internal BoxView boxSelected = new BoxView { IsVisible = false, HeightRequest = GlobalSetting.Size * .60, WidthRequest = GlobalSetting.Size * .60, Color = GlobalSetting.Color, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.Center };
        internal IconView imgSelected = new IconView { Source = ImageSource.FromResource(RESOURCE_CHECK), FillColor = GlobalSetting.Color, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.Center, IsVisible = false };
        internal Label lblOption = new Label { VerticalOptions = LayoutOptions.CenterAndExpand, FontSize = GlobalSetting.FontSize, TextColor = GlobalSetting.TextColor, FontFamily = GlobalSetting.FontFamily, IsVisible = false };
        private CheckType _type = CheckType.Box;
        private bool _isEnabled;
        #endregion

        #region Ctor
        /// <summary>
        /// Default Constructor
        /// </summary>
        public CheckBox()
        {
            InitVisualStates();
            this.Orientation = StackOrientation.Horizontal;
            this.Padding = new Thickness(0, 10);
            this.Spacing = 10;
            this.frmBackground.Content = boxSelected;
            this.Children.Add(frmBackground);
            this.Children.Add(lblOption);
            this.ApplyIsCheckedAction = ApplyIsChecked;
            this.ApplyIsPressedAction = ApplyIsPressed;
            this.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() => { if (IsDisabled) return; IsChecked = !IsChecked; ExecuteCommand(); CheckChanged?.Invoke(this, new EventArgs()); ValidationChanged?.Invoke(this, new EventArgs()); }),
            });
        }
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
        #endregion

        #region Events
        /// <summary>
        /// Invoked when check changed
        /// </summary>
        public event EventHandler CheckChanged;
        public event EventHandler ValidationChanged;
        #endregion

        #region Properties
        //-----------------------------------------------------------------------------
        /// <summary>
        /// Method to run when check changed. Default value is <see cref="ApplyIsChecked(bool)"/> It's not recommended to change this field. But you can set your custom <see cref="void"/> if you really need.
        /// </summary>
        public Action<CheckBox, bool> ApplyIsCheckedAction { get; set; }
        //-----------------------------------------------------------------------------
        /// <summary>
        /// Applies pressed effect. Default value is <see cref="ApplyIsPressed(bool)"/>. You can set another <see cref="void"/> to make custom pressed effects.
        /// </summary>
        public Action<CheckBox, bool> ApplyIsPressedAction { get; set; }
        /// <summary>
        /// Executed when check changed
        /// </summary>
        public ICommand CheckChangedCommand { get; set; }
        /// <summary>
        /// Command Parameter for Commands. If this is null, CommandParameter will be sent as itself of CheckBox
        /// </summary>
        public object CommandParameter { get => GetValue(CommandParameterProperty); set => SetValue(CommandParameterProperty, value); }
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
            get => (bool)GetValue(IsCheckedProperty);
            set
            {
                SetValue(IsCheckedProperty, value);
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
        public double BoxSize { get => frmBackground.Width; }
        /// <summary>
        /// SizeRequest of CheckBox
        /// </summary>
        public double BoxSizeRequest { get => frmBackground.WidthRequest; set => SetBoxSize(value); }
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

        public bool IsPressed { get; set; }
        /// <summary>
        /// Corner radius of Box of CheckBox.
        /// </summary>
        public float CornerRadius { get => (float)GetValue(CornerRadiusProperty); set => SetValue(CornerRadiusProperty, value); }
        #endregion

        #region BindableProperties
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static readonly BindableProperty ColorProperty = BindableProperty.Create(nameof(Color), typeof(Color), typeof(CheckBox), Color.Accent, propertyChanged: (bo, ov, nv) => (bo as CheckBox).UpdateColors());
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(CheckBox), GlobalSetting.TextColor, propertyChanged: (bo, ov, nv) => (bo as CheckBox).TextColor = (Color)nv);
        public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(nameof(IsChecked), typeof(bool), typeof(CheckBox), false, BindingMode.TwoWay, propertyChanged: (bo, ov, nv) => (bo as CheckBox).ApplyIsCheckedAction((bo as CheckBox), (bool)nv));
        public static readonly BindableProperty IsDisabledProperty = BindableProperty.Create(nameof(IsDisabled), typeof(bool), typeof(CheckBox), false, propertyChanged: (bo, ov, nv) => (bo as CheckBox).IsDisabled = (bool)nv);
        public static readonly BindableProperty KeyProperty = BindableProperty.Create(nameof(Key), typeof(int), typeof(CheckBox), 0, propertyChanged: (bo, ov, nv) => (bo as CheckBox).Key = (int)nv);
        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(CheckBox), "", propertyChanged: (bo, ov, nv) => (bo as CheckBox).Text = (string)nv);
        public static readonly BindableProperty CheckChangedCommandProperty = BindableProperty.Create(nameof(CheckChangedCommand), typeof(ICommand), typeof(CheckBox), null, propertyChanged: (bo, ov, nv) => (bo as CheckBox).CheckChangedCommand = (ICommand)nv);
        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(CheckBox), null);
        public static readonly BindableProperty BoxBackgroundColorProperty = BindableProperty.Create(nameof(BoxBackgroundColor), typeof(Color), typeof(CheckBox), GlobalSetting.BackgroundColor, propertyChanged: (bo, ov, nv) => (bo as CheckBox).UpdateBoxBackground());
        public static readonly BindableProperty TextFontSizeProperty = BindableProperty.Create(nameof(TextFontSize), typeof(double), typeof(CheckBox), GlobalSetting.FontSize, propertyChanged: (bo, ov, nv) => (bo as CheckBox).TextFontSize = (double)nv);
        public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(CheckBox), GlobalSetting.BorderColor, propertyChanged: (bo, ov, nv) => (bo as CheckBox).UpdateBorderColor());
        public static readonly BindableProperty CustomIconProperty = BindableProperty.Create(nameof(CustomIcon), typeof(ImageSource), typeof(CheckBox), default(ImageSource), propertyChanged: (bo, ov, nv) => (bo as CheckBox).UpdateType((bo as CheckBox).Type));
        public static readonly BindableProperty IsPressedProperty = BindableProperty.Create(nameof(IsPressed), typeof(bool), typeof(CheckBox), propertyChanged: (bo, ov, nv) => (bo as CheckBox).ApplyIsPressedAction((bo as CheckBox), (bool)nv));
        public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(nameof(CornerRadius), typeof(float), typeof(CheckBox), GlobalSetting.CornerRadius, propertyChanged: (bo, ov, nv) => (bo as CheckBox).frmBackground.CornerRadius = (float)nv);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        #endregion

        #region Methods
        void ExecuteCommand()
        {
            if (CheckChangedCommand?.CanExecute(CommandParameter ?? this) ?? false)
                CheckChangedCommand?.Execute(CommandParameter ?? this);
        }
        void UpdateBoxBackground()
        {
            if (this.Type == CheckType.Material)
                return;

            frmBackground.BackgroundColor = BoxBackgroundColor;
        }
        void UpdateColors()
        {
            boxSelected.Color = Color;
            if (Type == CheckType.Material)
            {
                frmBackground.BorderColor = Color;
                frmBackground.BackgroundColor = IsChecked ? Color : Color.Transparent;
                imgSelected.FillColor = Color.ToSurfaceColor();
            }
            else
            {
                frmBackground.BorderColor = IsChecked ? Color : BorderColor;
                frmBackground.BackgroundColor = BackgroundColor;
                imgSelected.FillColor = Color;
            }
        }
        void UpdateBorderColor()
        {
            if (this.Type == CheckType.Material)
                return;

            frmBackground.BorderColor = this.BorderColor;
        }
        void UpdateAllColors()
        {
            UpdateColors();
            //UpdateBoxBackground();
            //UpdateBorderColor();
        }
        void SetBoxSize(double value)
        {
            frmBackground.WidthRequest = value;
            frmBackground.HeightRequest = value;
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
                    frmBackground.Content = boxSelected;
                    break;
                case CheckType.Check:
                    imgSelected.Source = ImageSource.FromResource(RESOURCE_CHECK);
                    frmBackground.Content = imgSelected;
                    break;
                case CheckType.Cross:
                    imgSelected.Source = ImageSource.FromResource(RESOURCE_CROSS);
                    frmBackground.Content = imgSelected;
                    break;
                case CheckType.Star:
                    imgSelected.Source = ImageSource.FromResource(RESOURCE_STAR);
                    frmBackground.Content = imgSelected;
                    break;
                case CheckType.Material:
                    imgSelected.Source = ImageSource.FromResource(RESOURCE_CHECK);
                    frmBackground.CornerRadius = 5;
                    frmBackground.Content = imgSelected;
                    break;
                case CheckType.Custom:
                    imgSelected.Source = CustomIcon;
                    frmBackground.Content = imgSelected;
                    break;
            }
            UpdateAllColors();
        }
        protected virtual void InitVisualStates()
        {
            VisualStateManager.SetVisualStateGroups(this, new VisualStateGroupList
            {
                new VisualStateGroup
                {
                    Name = "InputKitStates",
                    TargetType = typeof(CheckBox),
                    States =
                    {
                        new VisualState
                        {
                            Name = "Pressed",
                            TargetType = typeof(CheckBox),
                            Setters =
                            {
                                new Setter { Property = CheckBox.IsPressedProperty, Value = true }
                            }
                        },
                        new VisualState
                        {
                            Name = "Normal",
                            TargetType = typeof(RadioButton),
                            Setters =
                            {
                                new Setter { Property = CheckBox.IsPressedProperty, Value = false }
                            }
                        }
                    }
                }
            });
        }
        /// <summary>
        /// Not available for this control
        /// </summary>
        public void DisplayValidation()
        {

        }
        public static void ApplyIsChecked(CheckBox checkBox, bool isChecked)
        {
            checkBox.frmBackground.Content.IsVisible = isChecked;
            checkBox.UpdateColors();
        }
        public static async void ApplyIsPressed(CheckBox checkBox, bool isPressed)
        {
            await checkBox.frmBackground.ScaleTo(isPressed ? .8 : 1, 50, Easing.BounceIn);
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
