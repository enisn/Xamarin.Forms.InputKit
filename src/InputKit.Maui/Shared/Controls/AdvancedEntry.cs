using InputKit.Shared.Configuration;
using System.Windows.Input;

namespace InputKit.Shared.Controls
{
    /// <summary>
    /// This Entry contains validation and some stuffs inside
    /// </summary>
    public partial class AdvancedEntry : StackLayout
    {
        #region Statics
        /// <summary>
        /// Keeps default setting of <see cref="AdvancedEntry"/>. AdvancedEntry uses this default settings to initalize.
        /// </summary>
        public static GlobalSetting GlobalSetting { get; private set; } = new GlobalSetting
        {
            BackgroundColor = Application.Current.PlatformAppTheme == AppTheme.Dark ? Colors.Black : Colors.White,
            CornerRadius = 20,
            BorderColor = Application.Current.PlatformAppTheme == AppTheme.Dark ? Colors.WhiteSmoke : Colors.Gray,
            Color = InputKitOptions.GetAccentColor(),
            FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
            Size = -1, /* This is not supported for this control*/
            TextColor = (Color)Entry.TextColorProperty.DefaultValue,
            LabelPosition = LabelPosition.After
        };
        #endregion

        #region Constants
        public const string REGEX_LETTERONLY = "^[A-Za-z]*$";
        public const string REGEX_NONDIGITS = "^[^0-9]*$";
        public const string REGEX_DIGITSONLY = "^[0-9]*$";
        public const string REGEX_DECIMAL = "^\\d+((\\.|,)\\d+)?$";
        public const string REGEX_EMAIL = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
        public const string REGEX_PASSWORD = "^(((?=.*[a-z])(?=.*[A-Z]))|((?=.*[a-z])(?=.*[0-9]))|((?=.*[A-Z])(?=.*[0-9])))(?=.{6,})";
        public const string REGEX_PHONE = "^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\\s\\./0-9]*$";
        #endregion

        #region Fields
        readonly Label lblTitle = new Label { Margin = new Thickness(6, 0, 0, 0), IsVisible = false, TextColor = GlobalSetting.TextColor, LineBreakMode = LineBreakMode.TailTruncation, FontFamily = GlobalSetting.FontFamily };
        readonly Frame frmBackground = new Frame { BackgroundColor = GlobalSetting.BackgroundColor, CornerRadius = (float)GlobalSetting.CornerRadius, BorderColor = GlobalSetting.BorderColor, Padding = new Thickness(5, 0, 0, 0), HasShadow = false };
        readonly Image imgIcon = new Image { InputTransparent = true, Margin = 5, IsVisible = false, VerticalOptions = LayoutOptions.Center, HeightRequest = 30 };
        readonly Entry txtInput;
        readonly Grid inputGrid;
        #endregion

        #region Ctor
        /// <summary>
        /// Default Constructor
        /// </summary>
        public AdvancedEntry()
        {
            txtInput = GetInputEntry();
            Children.Add(lblTitle);
            Children.Add(frmBackground);

            inputGrid = new Grid();
            inputGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = 30 });
            inputGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
            inputGrid.Add(imgIcon, column: 0);
            inputGrid.Add(txtInput, column: 1);

            frmBackground.Content = new Grid
            {
                BackgroundColor = Colors.Transparent,
                Children =
                {
                    inputGrid,
                }
            };

            InitializeValidation();
            txtInput.Completed += (s, args) => { ExecuteCommand(); Completed?.Invoke(this, new EventArgs()); FocusNext(); };
            txtInput.Focused += (s, args) => { var arg = new FocusEventArgs(this, true); FocusedCommand?.Execute(arg); Focused?.Invoke(this, arg); };
            txtInput.Unfocused += (s, args) => { var arg = new FocusEventArgs(this, false); UnfocusedCommand?.Execute(arg); Unfocused?.Invoke(this, arg); };
        }
        #endregion

        #region Not Implemented
        public bool IsSelected { get => false; set { } }

        #endregion
        #region Fields
        private bool _isDisabled;
        #endregion
        #region Events
        public event EventHandler Completed;
        public event EventHandler<TextChangedEventArgs> TextChanged;
        public new event EventHandler<FocusEventArgs> Unfocused;
        public new event EventHandler<FocusEventArgs> Focused;
        #endregion
        #region Properties

        /// <summary>
        /// Text of this input
        /// </summary>
        public string Text { get => txtInput.Text; set { txtInput.Text = value; OnPropertyChanged(); } }

        /// <summary>
        /// Title will be shown top of this control
        /// </summary>
        public string Title { get => lblTitle.Text; set { lblTitle.Text = value; lblTitle.IsVisible = !string.IsNullOrEmpty(value); } }

        public Color TitleColor { get => (Color)GetValue(TitleColorProperty); set => SetValue(TitleColorProperty, value); }

        /// <summary>
        /// Icons of this Entry
        /// </summary>
        public ImageSource IconImage
        {
            get => imgIcon.Source;
            set
            {
                imgIcon.IsVisible = value != null;
                imgIcon.Source = value;
            }
        }

        /// <summary>
        /// BackgroundColor of this Control
        /// </summary>
        public new Color BackgroundColor { get => (Color)GetValue(BackgroundColorProperty); set => SetValue(BackgroundColorProperty, value); }

        /// <summary>
        /// Bordercolor of this control
        /// </summary>
        public Color BorderColor { get => (Color)GetValue(BorderColorProperty); set => SetValue(BorderColorProperty, value); }

        /// <summary>
        /// Text Color of this Control
        /// </summary>
        public Color TextColor { get => (Color)GetValue(TextColorProperty); set => SetValue(TextColorProperty, value); }

        /// <summary>
        /// HorizontalTextAlignment of this Control
        /// </summary>
        public TextAlignment HorizontalTextAlignment { get => (TextAlignment)GetValue(HorizontalTextAlignmentProperty); set => SetValue(HorizontalTextAlignmentProperty, value); }

        /// <summary>
        /// BackgroundColor of this Control
        /// </summary>
        public Color PlaceholderColor { get => txtInput.PlaceholderColor; set => txtInput.PlaceholderColor = value; }

        /// <summary>
        /// Placeholder of entry
        /// </summary>
        public string Placeholder { get => (string)GetValue(PlaceholderProperty); set => SetValue(PlaceholderProperty, value); }

        /// <summary>
        /// Maximum length of this Entry
        /// </summary>
        public int MaxLength
        {
            get => (int)GetValue(MaxLengthProperty);
            set => SetValue(MaxLengthProperty, value);
        }

        /// <summary>
        /// Corner radius of Entry.
        /// </summary>
        public float CornerRadius { get => (float)GetValue(CornerRadiusProperty); set => SetValue(CornerRadiusProperty, value); }

        /// <summary>
        /// To be added.
        /// </summary>
        public string FontFamily
        {
            get => txtInput.FontFamily;
            set
            {
                lblTitle.FontFamily = value;
                labelValidation.Value.FontFamily = value;
                txtInput.FontFamily = value;
            }
        }

        /// <summary>
        /// Disabled this control
        /// </summary>
        public bool IsDisabled
        {
            get => _isDisabled; set
            {
                _isDisabled = value;
                Opacity = value ? 0.6 : 1;
                txtInput.IsEnabled = !value;
            }
        }

        /// <summary>
        /// IsPassword situation of entry.
        /// </summary>
        public bool IsPassword { get => txtInput.IsPassword; set => txtInput.IsPassword = value; }

        /// <summary>
        /// Executed when entry completed.
        /// </summary>
        public ICommand CompletedCommand { get; set; }

        /// <summary>
        /// Executed when entry focused.
        /// </summary>
        public Command<FocusEventArgs> FocusedCommand { get; set; }

        /// <summary>
        /// Executed when entry unfocused.
        /// </summary>
        public Command<FocusEventArgs> UnfocusedCommand { get; set; }

        /// <summary>
        /// Parameter to send with CompletedCommand
        /// </summary>
        public object CommandParameter { get => GetValue(CommandParameterProperty); set => SetValue(CommandParameterProperty, value); }

        /// <summary>
        /// Changes Font Size of Entry's Text
        /// </summary>
        /// 
        [System.ComponentModel.TypeConverter(typeof(FontSizeConverter))]
        public double TextFontSize { get => (double)GetValue(TextFontSizeProperty); set => SetValue(TextFontSizeProperty, value); }
        ///----------------------------------------- -------------------------------
        /// <summary>
        /// Gets and sets keyboard type of this entry
        /// </summary>
        public Keyboard Keyboard { get => txtInput.Keyboard; set => txtInput.Keyboard = value; }

        /// <summary>
        /// Sets if an Empty Entry is Valid
        /// </summary>
        public bool Nullable { get => (bool)GetValue(NullableProperty); set => SetValue(NullableProperty, value); }

        public int CursorPosition { get => (int)GetValue(CursorPositionProperty); set => SetValue(CursorPositionProperty, value); }
        #endregion

        //--------------------------------------------------------------------------------------------------------------------------------------------------
        #region BindableProperties
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(AdvancedEntry), null, BindingMode.TwoWay, propertyChanged: (bo, ov, nv) => (bo as AdvancedEntry).Text = (string)nv);
        public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(AdvancedEntry), null, propertyChanged: (bo, ov, nv) => (bo as AdvancedEntry).Title = (string)nv);
        public static readonly BindableProperty TitleColorProperty = BindableProperty.Create(nameof(TitleColor), typeof(Color), typeof(AdvancedEntry), (Color)Label.TextColorProperty.DefaultValue, propertyChanged: (bo, ov, nv) => (bo as AdvancedEntry).lblTitle.TextColor = (Color)nv);
        public static readonly BindableProperty IconImageProperty = BindableProperty.Create(nameof(IconImage), typeof(string), typeof(AdvancedEntry), null, propertyChanged: (bo, ov, nv) => (bo as AdvancedEntry).IconImage = (string)nv);
        public static readonly new BindableProperty BackgroundColorProperty = BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(AdvancedEntry), (Color)VisualElement.BackgroundColorProperty.DefaultValue, propertyChanged: (bo, ov, nv) => (bo as AdvancedEntry).frmBackground.BackgroundColor = (Color)nv);
        public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(AdvancedEntry), (Color)Microsoft.Maui.Controls.Frame.BorderColorProperty.DefaultValue, propertyChanged: (bo, ov, nv) => (bo as AdvancedEntry).frmBackground.BorderColor = (Color)nv);
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(AdvancedEntry), Entry.TextColorProperty.DefaultValue, propertyChanged: (bo, ov, nv) => (bo as AdvancedEntry).txtInput.TextColor = (Color)nv);
        public static readonly BindableProperty PlaceholderColorProperty = BindableProperty.Create(nameof(PlaceholderColor), typeof(Color), typeof(AdvancedEntry), Colors.LightGray, propertyChanged: (bo, ov, nv) => (bo as AdvancedEntry).PlaceholderColor = (Color)nv);
        public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(AdvancedEntry), default(string), propertyChanged: (bo, ov, nv) => (bo as AdvancedEntry).txtInput.Placeholder = (string)nv);
        public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(nameof(CornerRadius), typeof(float), typeof(AdvancedEntry), (float)Microsoft.Maui.Controls.Frame.CornerRadiusProperty.DefaultValue, propertyChanged: (bo, ov, nv) => (bo as AdvancedEntry).frmBackground.CornerRadius = (float)nv);
        public static readonly BindableProperty AnnotationColorProperty = BindableProperty.Create(nameof(ValidationColor), typeof(Color), typeof(AdvancedEntry), InputKitOptions.GetAccentColor(), propertyChanged: (bo, ov, nv) => (bo as AdvancedEntry).ValidationColor = (Color)nv);
        public static readonly BindableProperty CompletedCommandProperty = BindableProperty.Create(nameof(CompletedCommand), typeof(ICommand), typeof(AdvancedEntry), null, propertyChanged: (bo, ov, nv) => (bo as AdvancedEntry).CompletedCommand = (ICommand)nv);
        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(AdvancedEntry), propertyChanged: (bo, ov, nv) => (bo as AdvancedEntry).CommandParameter = nv);
        public static readonly BindableProperty TextFontSizeProperty = BindableProperty.Create(nameof(TextFontSize), typeof(double), typeof(AdvancedEntry), Device.GetNamedSize(NamedSize.Default, typeof(Label)), propertyChanged: (bo, ov, nv) => (bo as AdvancedEntry).txtInput.FontSize = (double)nv);
        public static readonly BindableProperty HorizontalTextAlignmentProperty = BindableProperty.Create(nameof(HorizontalTextAlignment), typeof(TextAlignment), typeof(AdvancedEntry), TextAlignment.Start, propertyChanged: (bo, ov, nv) => (bo as AdvancedEntry).txtInput.HorizontalTextAlignment = (TextAlignment)nv);
        public static readonly BindableProperty NullableProperty = BindableProperty.Create(nameof(Nullable), typeof(bool), typeof(AdvancedEntry), false, propertyChanged: (bo, ov, nv) => (bo as AdvancedEntry).Nullable = (bool)nv);
        public static readonly BindableProperty CursorPositionProperty = BindableProperty.Create(nameof(CursorPosition), typeof(int), typeof(AdvancedEntry), 0, propertyChanged: (bo, ov, nv) => (bo as AdvancedEntry).txtInput.CursorPosition = (int)nv);

        public static readonly BindableProperty MaxLengthProperty = BindableProperty.Create(
            nameof(MaxLength),
            typeof(int),
            typeof(AdvancedEntry),
            int.MaxValue,
            propertyChanged: (bindable, oldValue, newValue) => (bindable as AdvancedEntry).txtInput.MaxLength = (int)newValue);



#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        #endregion

        #region Methods
        void ExecuteCommand()
        {
            if (CompletedCommand?.CanExecute(CommandParameter ?? this) ?? false)
                CompletedCommand?.Execute(CommandParameter ?? this);
        }

        /// <summary>
        /// Focus on this entry
        /// </summary>
        public virtual new void Focus()
        {
            Focused?.Invoke(this, new FocusEventArgs(this, true));
            txtInput.Focus();
        }
        /// <summary>
        /// Onfocus from this entry and hides keyboard.
        /// </summary>
        public virtual new void Unfocus()
        {
            Focused?.Invoke(this, new FocusEventArgs(this, true));
            txtInput.Unfocus();
        }

        /// <summary>
        /// Automaticly finds next Advanced entry and focus it.
        /// </summary>
        public virtual void FocusNext()
        {
            if (Parent is Layout parent)
            {
                int index = parent.Children.IndexOf(this);
                for (int i = index + 1; i < Math.Clamp(index + 4, 0, parent.Children.Count); i++)
                {
                    if (parent.Children[i] is AdvancedEntry)
                    {
                        (parent.Children[i] as AdvancedEntry).Focus();
                        break;
                    }
                }
            }
        }

        private protected virtual Entry GetInputEntry()
        {
            return new Entry
            {
                TextColor = GlobalSetting.TextColor,
                PlaceholderColor = Colors.LightGray,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Center,
                FontFamily = GlobalSetting.FontFamily,
                BackgroundColor = Colors.Transparent,
            };
        }
        #endregion
    }
}
