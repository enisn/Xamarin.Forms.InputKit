using Plugin.InputKit.Shared.Abstraction;
using Plugin.InputKit.Shared.Configuration;
using Plugin.InputKit.Shared.Layouts;
using Plugin.InputKit.Shared;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Plugin.InputKit.Shared.Controls
{
    /// <summary>
    /// Radio Button with Text
    /// </summary>
    public class RadioButton : StatefulStackLayout
    {
        #region Statics
        /// <summary>
        /// Default values of RadioButton
        /// </summary>
        public static GlobalSetting GlobalSetting { get; private set; } = new GlobalSetting
        {
            Color = InputKitOptions.GetAccentColor(),
            BorderColor = Color.Black,
            TextColor = (Color)Label.TextColorProperty.DefaultValue,
            Size = Device.GetNamedSize(Device.RuntimePlatform == Device.iOS ? NamedSize.Large : NamedSize.Medium, typeof(Label)) * 1.2,
            CornerRadius = -1,
            FontSize = Device.GetNamedSize(Device.RuntimePlatform == Device.iOS ? NamedSize.Medium : NamedSize.Small, typeof(Label)),
            LabelPosition = LabelPosition.After
        };
        #endregion

        #region Constants
        public const string RESOURCE_CIRCLE = "InputKit.Shared.Resources.circle.png";
        public const string RESOURCE_DOT = "InputKit.Shared.Resources.dot.png";
        #endregion

        #region Fields
        internal Grid IconLayout;
        internal IconView iconCircle = new IconView { Source = ImageSource.FromResource(RESOURCE_CIRCLE), FillColor = GlobalSetting.BorderColor, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.Center, HeightRequest = GlobalSetting.Size, WidthRequest = GlobalSetting.Size };
        internal IconView iconChecked = new IconView { Source = ImageSource.FromResource(RESOURCE_DOT), FillColor = GlobalSetting.Color, IsVisible = false, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.Center, HeightRequest = GlobalSetting.Size, WidthRequest = GlobalSetting.Size };
        internal Label lblText = new Label { VerticalTextAlignment = TextAlignment.Center, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.Fill, TextColor = GlobalSetting.TextColor, FontSize = GlobalSetting.FontSize, FontFamily = GlobalSetting.FontFamily, MaxLines = 3, LineBreakMode = LineBreakMode.WordWrap };
        private bool _isDisabled;
        #endregion

        #region Ctor
        /// <summary>
        /// Default Constructor
        /// </summary>
        public RadioButton()
        {
            InitVisualStates();

            Orientation = StackOrientation.Horizontal;
            if (Device.RuntimePlatform != Device.iOS)
                lblText.FontSize = lblText.FontSize *= 1.5;


            ApplyIsCheckedAction = ApplyIsChecked;
            ApplyIsPressedAction = ApplyIsPressed;

            IconLayout = new Grid
            {
                VerticalOptions = LayoutOptions.Center,
                Children =
                {
                    iconCircle,
                    iconChecked
                },
                MinimumWidthRequest = GlobalSetting.Size * 1.66,
            };

            ApplyLabelPosition(LabelPosition);

            GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(Tapped) });
        }
        #endregion


        /// <summary>
        /// Click event, triggered when clicked
        /// </summary>
        public event EventHandler Clicked;

        /// <summary>
        /// Event triggered when the Checked property changed, which may occur due to a click or setting the 
        /// IsChecked property in code.
        /// </summary>
        public event EventHandler Checked;

        #region Properties
        /// <summary>
        /// Method to run when check changed. Default value is <see cref="ApplyIsChecked(bool)"/> It's not recommended to change this field. But you can set your custom <see cref="void"/> if you really need.
        /// </summary>
        public Action<bool> ApplyIsCheckedAction { get; set; }

        /// <summary>
        /// Applies pressed effect. Default value is <see cref="ApplyIsPressed(bool)"/>. You can set another <see cref="void"/> to make custom pressed effects.
        /// </summary>
        public Action<bool> ApplyIsPressedAction { get; set; }

        /// <summary>
        /// Click command, executed when clicked.  Parameter will be Value property if CommandParameter is not set
        /// </summary>
        public ICommand ClickCommand { get; set; }

        /// <summary>
        /// A command parameter will be sent to commands.
        /// </summary>
        public object CommandParameter { get; set; }

        /// <summary>
        /// Value to keep inside of Radio Button
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Gets or Sets, is that Radio Button selected/choosed/Checked
        /// </summary>
        public bool IsChecked
        {
            get => (bool)GetValue(IsCheckedProperty);
            set => SetValue(IsCheckedProperty, value);
        }

        /// <summary>
        /// this control if is Disabled
        /// </summary>
        public bool IsDisabled { get => _isDisabled; set { _isDisabled = value; Opacity = value ? 0.6 : 1; } }

        /// <summary>
        /// Text Description of Radio Button. It will be displayed right of Radio Button
        /// </summary>
        public string Text { get => lblText.Text; set { lblText.Text = value; lblText.IsVisible = !string.IsNullOrEmpty(value); } }
        /// <summary>
        /// Fontsize of Description Text
        /// </summary>

        [System.ComponentModel.TypeConverter(typeof(FontSizeConverter))]
        public double TextFontSize { get => lblText.FontSize; set => lblText.FontSize = value; }

        /// <summary>
        /// Set your own background image instead of default circle.
        /// </summary>
        public ImageSource CircleImage { get => (ImageSource)GetValue(CircleImageProperty); set => SetValue(CircleImageProperty, value); }

        public ImageSource CheckedImage { get => (ImageSource)GetValue(CheckedImageProperty); set => SetValue(CheckedImageProperty, value); }

        /// <summary>
        /// To be added.
        /// </summary>
        public string FontFamily { get => lblText.FontFamily; set => lblText.FontFamily = value; }

        /// <summary>
        /// Color of Radio Button's checked.
        /// </summary>
        public Color Color { get => (Color)GetValue(ColorProperty); set => SetValue(ColorProperty, value); }

        /// <summary>
        /// Color of radio button's outline border 
        /// </summary>
        public Color CircleColor { get => (Color)GetValue(CircleColorProperty); set => SetValue(CircleColorProperty, value); }

        /// <summary>
        /// Color of description text of Radio Button
        /// </summary>
        public Color TextColor { get => (Color)GetValue(TextColorProperty); set => SetValue(TextColorProperty, value); }
        /// <summary>
        /// Internal use only. Applies effect when pressed.
        /// </summary>

        [Browsable(false)]
        public bool IsPressed { get => (bool)GetValue(IsPressedProperty); set => SetValue(IsPressedProperty, value); }
        /// <summary>
        /// Gets or sets the label position.
        /// </summary>
        public LabelPosition LabelPosition
        {
            get => (LabelPosition)GetValue(LabelPositionProperty);
            set => SetValue(LabelPositionProperty, value);
        }
        #endregion

        #region BindableProperties
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(nameof(IsChecked), typeof(bool), typeof(RadioButton), false, propertyChanged: (bo, ov, nv) => (bo as RadioButton).ApplyIsCheckedAction((bool)nv));
        public static readonly BindableProperty IsDisabledProperty = BindableProperty.Create(nameof(IsDisabled), typeof(bool), typeof(RadioButton), false, propertyChanged: (bo, ov, nv) => (bo as RadioButton).IsDisabled = (bool)nv);
        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(RadioButton), "", propertyChanged: (bo, ov, nv) => (bo as RadioButton).Text = (string)nv);
        public static readonly BindableProperty TextFontSizeProperty = BindableProperty.Create(nameof(TextFontSize), typeof(double), typeof(RadioButton), 20.0, propertyChanged: (bo, ov, nv) => (bo as RadioButton).TextFontSize = (double)nv);
        public static readonly BindableProperty ColorProperty = BindableProperty.Create(nameof(Color), typeof(Color), typeof(RadioButton), GlobalSetting.Color, propertyChanged: (bo, ov, nv) => (bo as RadioButton).UpdateColors());
        public static readonly BindableProperty CircleImageProperty = BindableProperty.Create(nameof(CircleImage), typeof(ImageSource), typeof(RadioButton), default(ImageSource), propertyChanged: (bo, ov, nv) => (bo as RadioButton).iconCircle.Source = nv as ImageSource ?? nv?.ToString());
        public static readonly BindableProperty CheckedImageProperty = BindableProperty.Create(nameof(CheckedImage), typeof(ImageSource), typeof(RadioButton), default(ImageSource), propertyChanged: (bo, ov, nv) => (bo as RadioButton).iconChecked.Source = nv as ImageSource ?? nv?.ToString());
        public static readonly BindableProperty CircleColorProperty = BindableProperty.Create(nameof(CircleColor), typeof(Color), typeof(RadioButton), GlobalSetting.BorderColor, propertyChanged: (bo, ov, nv) => (bo as RadioButton).UpdateColors());
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(RadioButton), GlobalSetting.TextColor, propertyChanged: (bo, ov, nv) => (bo as RadioButton).UpdateColors());
        public static readonly BindableProperty ClickCommandProperty = BindableProperty.Create(nameof(ClickCommand), typeof(ICommand), typeof(RadioButton), null, propertyChanged: (bo, ov, nv) => (bo as RadioButton).ClickCommand = (ICommand)nv);
        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(RadioButton), propertyChanged: (bo, ov, nv) => (bo as RadioButton).CommandParameter = nv);
        public static readonly BindableProperty IsPressedProperty = BindableProperty.Create(nameof(IsPressed), typeof(bool), typeof(RadioButton), propertyChanged: (bo, ov, nv) => (bo as RadioButton).ApplyIsPressedAction((bool)nv));
        public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(RadioButton), propertyChanged: (bo, ov, nv) => (bo as RadioButton).FontFamily = (string)nv);
        public static readonly BindableProperty LabelPositionProperty = BindableProperty.Create(
            propertyName: nameof(LabelPosition), declaringType: typeof(RadioButton),
            returnType: typeof(LabelPosition), defaultBindingMode: BindingMode.TwoWay,
            defaultValue: GlobalSetting.LabelPosition,
            propertyChanged: (bo, ov, nv) => (bo as RadioButton).ApplyLabelPosition((LabelPosition)nv));
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        #endregion

        #region Methods
        private void ApplyLabelPosition(LabelPosition position)
        {
            Children.Clear();
            if (position == LabelPosition.After)
            {
                IconLayout.HorizontalOptions = LayoutOptions.Center;
                Children.Add(IconLayout);
                Children.Add(lblText);
            }
            else
            {
                IconLayout.HorizontalOptions = LayoutOptions.Center;
                Children.Add(lblText);
                Children.Add(IconLayout);
            }
        }

        /// <summary>
        /// That handles tapps and triggers event, commands etc.
        /// </summary>
        void Tapped()
        {
            if (IsDisabled)
            {
                return;
            }

            IsChecked = true;
            Clicked?.Invoke(this, new EventArgs());
            ClickCommand?.Execute(CommandParameter ?? Value);
        }

        void UpdateColors()
        {
            iconChecked.FillColor = Color;
            iconCircle.FillColor = IsChecked ? Color : CircleColor;
            lblText.TextColor = TextColor;
        }

        public virtual void ApplyIsChecked(bool isChecked)
        {
            var changed = iconChecked.IsVisible != isChecked;
            iconChecked.IsVisible = isChecked;
            UpdateColors();
            if (changed)
            {
                Checked?.Invoke(this, null);
            }
        }

        public virtual async void ApplyIsPressed(bool isPressed)
        {
            await IconLayout.ScaleTo(isPressed ? .8 : 1, 100);
        }

        void InitVisualStates()
        {
            VisualStateManager.SetVisualStateGroups(this, new VisualStateGroupList
            {
                new VisualStateGroup
                {
                    Name = "InputKitStates",
                    TargetType = typeof(RadioButton),
                    States =
                    {
                        new VisualState
                        {
                            Name = "Pressed",
                            TargetType = typeof(RadioButton),
                            Setters =
                            {
                                new Setter { Property = IsPressedProperty, Value = true }
                            }
                        },
                        new VisualState
                        {
                            Name = "Normal",
                            TargetType = typeof(RadioButton),
                            Setters =
                            {
                                new Setter { Property = IsPressedProperty, Value = false }
                            }
                        }
                    }
                }
            });
        }
        #endregion
    }
}
