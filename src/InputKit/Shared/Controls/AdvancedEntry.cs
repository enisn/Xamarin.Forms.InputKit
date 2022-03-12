using InputKit.Shared.Abstraction;
using InputKit.Shared.Configuration;
using Plainer.Maui.Controls;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace InputKit.Shared.Controls
{
    /// <summary>
    /// This Entry contains validation and some stuffs inside
    /// </summary>
    public partial class AdvancedEntry : StackLayout, IValidatable
    {
        #region Statics
        /// <summary>
        /// Keeps default setting of <see cref="AdvancedEntry"/>. AdvancedEntry uses this default settings to initalize.
        /// </summary>
        public static GlobalSetting GlobalSetting { get; private set; } = new GlobalSetting
        {
            BackgroundColor = Colors.White,
            CornerRadius = 20,
            BorderColor = Colors.Gray,
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
        readonly Label lblAnnotation = new Label { Margin = new Thickness(6, 0, 0, 0), IsVisible = false, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)), Opacity = 0.8, TextColor = GlobalSetting.TextColor, FontFamily = GlobalSetting.FontFamily };
        readonly Frame frmBackground = new Frame { BackgroundColor = GlobalSetting.BackgroundColor, CornerRadius = (float)GlobalSetting.CornerRadius, BorderColor = GlobalSetting.BorderColor, Padding = new Thickness(5, 0, 0, 0), HasShadow = false };
        readonly Image imgWarning = new Image { Margin = 10, HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center, InputTransparent = true, Source = "alert.png" };
        readonly IconView imgIcon = new IconView { InputTransparent = true, IsVisible = false, Margin = new Thickness(5, 10, 10, 10), VerticalOptions = LayoutOptions.CenterAndExpand, HeightRequest = 30, FillColor = GlobalSetting.Color };
        readonly Entry txtInput;
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

            ApplyValidationPosition(GlobalSetting.LabelPosition);

            frmBackground.Content = new Grid
            {
                BackgroundColor = Colors.Transparent,
                Children =
                {
                    new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        BackgroundColor = Colors.Transparent,
                        Children =
                        {
                            imgIcon,
                            txtInput
                        }
                    },
                    imgWarning
                }
            };

            txtInput.TextChanged += TxtInput_TextChanged;
            txtInput.Completed += (s, args) => { ExecuteCommand(); Completed?.Invoke(this, new EventArgs()); FocusNext(); };
            txtInput.Focused += (s, args) => { var arg = new FocusEventArgs(this, true); FocusedCommand?.Execute(arg); Focused?.Invoke(this, arg); };
            txtInput.Unfocused += (s, args) => { var arg = new FocusEventArgs(this, false); UnfocusedCommand?.Execute(arg); Unfocused?.Invoke(this, arg); };
            imgWarning.IsVisible = IsRequired;
            Reset();
        }
        #endregion

        #region Not Implemented
        public bool IsSelected { get => false; set { } }
        public object Value { get; set; }
        public bool IsValidated => IsAnnotated;
        #endregion
        #region Fields
        private Color _defaultAnnotationColor = Colors.Gray;
        private AnnotationType _annotation;
        private bool _isDisabled;
        private int _minLength;
        private string _validationMessage;
        #endregion
        #region Events
        public event EventHandler Completed;
        public event EventHandler<TextChangedEventArgs> TextChanged;
        public event EventHandler Clicked;
        public event EventHandler ValidationChanged;
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
        public string IconImage
        {
            get => imgIcon.Source.ToString();
            set
            {
                imgIcon.IsVisible = !string.IsNullOrEmpty(value);
                imgIcon.Source = value;
            }
        }

        /// <summary>
        /// Color of Icon
        /// </summary>
        public Color IconColor { get => (Color)GetValue(IconColorProperty); set => SetValue(IconColorProperty, value); }

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
            get => txtInput.MaxLength;
            set => txtInput.MaxLength = value;
        }

        /// <summary>
        /// Minimum length of this Entry
        /// </summary>
        public int MinLength { get => _minLength; set { _minLength = value; UpdateWarning(); /*DisplayValidation(); */} }

        /// <summary>
        /// Corner radius of Entry.
        /// </summary>
        public float CornerRadius { get => (float)GetValue(CornerRadiusProperty); set => SetValue(CornerRadiusProperty, Value); }

        /// <summary>
        /// To be added.
        /// </summary>
        public string FontFamily
        {
            get => txtInput.FontFamily;
            set
            {
                lblTitle.FontFamily = value;
                lblAnnotation.FontFamily = value;
                txtInput.FontFamily = value;
            }
        }

        /// <summary>
        /// This will be shown below title. This automaticly updating. If you set this manually you must set true IgnoreValidationMessage !!! 
        /// </summary>
        public string AnnotationMessage
        {
            get => lblAnnotation.Text;
            set
            {
                lblAnnotation.Text = value;
                lblAnnotation.IsVisible = !string.IsNullOrEmpty(value);
            }
        }

        /// <summary>
        /// AnnotationMessage's color.
        /// </summary>
        public Color AnnotationColor
        {
            get => lblAnnotation.TextColor;
            set { lblAnnotation.TextColor = value; _defaultAnnotationColor = value; }
        }

        /// <summary>
        /// will be added
        /// </summary>
        public AnnotationType Annotation { get => _annotation; set { _annotation = value; UpdateKeyboard(value); } }

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
        /// Finds this entry if Annotated
        /// </summary>
        public bool IsAnnotated
        {
            get
            {
                if (!IsRequired)
                    return true;

                if (string.IsNullOrEmpty(Text))
                {
                    if (Nullable)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                if (Text.Length < MinLength)
                    return false;

                switch (Annotation)
                {
                    case AnnotationType.None:
                        /* something can be placed here*/
                        break;
                    case AnnotationType.LettersOnly:
                        return Regex.Match(Text, REGEX_LETTERONLY).Success;

                    case AnnotationType.DigitsOnly:
                        return Regex.Match(Text, REGEX_DIGITSONLY).Success;

                    case AnnotationType.NonDigitsOnly:
                        return Regex.Match(Text, REGEX_NONDIGITS).Success;

                    case AnnotationType.Decimal:
                        return Regex.Match(Text, REGEX_DECIMAL).Success;

                    case AnnotationType.Email:
                        return Regex.Match(Text, REGEX_EMAIL).Success;

                    case AnnotationType.Password:
                        return Regex.Match(Text, REGEX_PASSWORD).Success;

                    case AnnotationType.Phone:
                        return Regex.Match(Text, REGEX_PHONE).Success;

                    case AnnotationType.ShortType:
                        return short.TryParse(Text, out _);

                    case AnnotationType.IntType:
                        return int.TryParse(Text, out _);

                    case AnnotationType.LongType:
                        return long.TryParse(Text, out _);

                    case AnnotationType.FloatType:
                        return float.TryParse(Text, out _);

                    case AnnotationType.DoubleType:
                        return double.TryParse(Text, out _);

                    case AnnotationType.DecimalType:
                        return decimal.TryParse(Text, out _);

                    case AnnotationType.ByteType:
                        return byte.TryParse(Text, out _);

                    case AnnotationType.SByteType:
                        return sbyte.TryParse(Text, out _);

                    case AnnotationType.CharType:
                        return char.TryParse(Text, out _);

                    case AnnotationType.UIntType:
                        return uint.TryParse(Text, out _);

                    case AnnotationType.ULongType:
                        return ulong.TryParse(Text, out _);

                    case AnnotationType.UShortType:
                        return ushort.TryParse(Text, out _);

                    case AnnotationType.RegexPattern:
                        return Regex.Match(Text, RegexPattern).Success;
                }
                return true;
            }
            set { /*to make visible in XAML pages*/ }
        }

        /// <summary>
        /// IsPassword situation of entry.
        /// </summary>
        public bool IsPassword { get => txtInput.IsPassword; set => txtInput.IsPassword = value; }

        /// <summary>
        /// Comes from IValidatable implementation. Shows this if Validated.
        /// </summary>
        public bool IsRequired { get => (bool)GetValue(IsRequiredProperty); set => SetValue(IsRequiredProperty, value); }

        /// <summary>
        /// Validation message to update automaticly. This will be shown when entry is not validated
        /// </summary>
        public string ValidationMessage
        {
            get => _validationMessage;
            set
            {
                var oldValueIsNull = _validationMessage == null;
                _validationMessage = value;
                if (!oldValueIsNull)
                    DisplayValidation();
            }
        }

        /// <summary>
        /// Ignores automaticly update annotationmessage
        /// </summary>
        public bool IgnoreValidationMessage { get => (bool)GetValue(IgnoreValidationMessageProperty); set => SetValue(IgnoreValidationMessageProperty, value); }

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
        /// You need to set Annotation="RegexPattern" to use this.
        /// </summary>
        public string RegexPattern { get => (string)GetValue(RegexPatternProperty); set => SetValue(RegexPatternProperty, value); }

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
        //----------------------------------------- -------------------------------
        /// <summary>
        /// Position of Annotation Label for AdvancedEntry
        /// </summary>
        public LabelPosition ValidationPosition
        {
            get => (LabelPosition)GetValue(ValidationPositionProperty);
            set => SetValue(ValidationPositionProperty, value);
        }
        //----------------------------------------- -------------------------------
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
        public static readonly BindableProperty IconColorProperty = BindableProperty.Create(nameof(IconColor), typeof(Color), typeof(AdvancedEntry), Colors.Black, propertyChanged: (bo, ov, nv) => (bo as AdvancedEntry).imgIcon.FillColor = (Color)nv);
        public static readonly new BindableProperty BackgroundColorProperty = BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(AdvancedEntry), (Color)VisualElement.BackgroundColorProperty.DefaultValue, propertyChanged: (bo, ov, nv) => (bo as AdvancedEntry).frmBackground.BackgroundColor = (Color)nv);
        public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(AdvancedEntry), (Color)Microsoft.Maui.Controls.Frame.BorderColorProperty.DefaultValue, propertyChanged: (bo, ov, nv) => (bo as AdvancedEntry).frmBackground.BorderColor = (Color)nv);
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(AdvancedEntry), Entry.TextColorProperty.DefaultValue, propertyChanged: (bo, ov, nv) => (bo as AdvancedEntry).txtInput.TextColor = (Color)nv);
        public static readonly BindableProperty PlaceholderColorProperty = BindableProperty.Create(nameof(PlaceholderColor), typeof(Color), typeof(AdvancedEntry), Colors.LightGray, propertyChanged: (bo, ov, nv) => (bo as AdvancedEntry).PlaceholderColor = (Color)nv);
        public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(AdvancedEntry), default(string), propertyChanged: (bo, ov, nv) => (bo as AdvancedEntry).txtInput.Placeholder = (string)nv);
        public static readonly BindableProperty MaxLengthProperty = BindableProperty.Create(nameof(MaxLength), typeof(int), typeof(AdvancedEntry), int.MaxValue, propertyChanged: (bo, ov, nv) => (bo as AdvancedEntry).MaxLength = (int)nv);
        public static readonly BindableProperty MinLengthProperty = BindableProperty.Create(nameof(MinLength), typeof(int), typeof(AdvancedEntry), 0, propertyChanged: (bo, ov, nv) => (bo as AdvancedEntry).MinLength = (int)nv);
        public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(nameof(CornerRadius), typeof(float), typeof(AdvancedEntry), (float)Microsoft.Maui.Controls.Frame.CornerRadiusProperty.DefaultValue, propertyChanged: (bo, ov, nv) => (bo as AdvancedEntry).frmBackground.CornerRadius = (float)nv);
        public static readonly BindableProperty IsAnnotatedProperty = BindableProperty.Create(nameof(IsAnnotated), typeof(bool), typeof(AdvancedEntry), false, BindingMode.OneWayToSource);
        public static readonly BindableProperty AnnotationColorProperty = BindableProperty.Create(nameof(AnnotationColor), typeof(Color), typeof(AdvancedEntry), InputKitOptions.GetAccentColor(), propertyChanged: (bo, ov, nv) => (bo as AdvancedEntry).AnnotationColor = (Color)nv);
        public static readonly BindableProperty AnnotationMessageProperty = BindableProperty.Create(nameof(AnnotationMessage), typeof(string), typeof(AdvancedEntry), "", propertyChanged: (bo, ov, nv) => (bo as AdvancedEntry).AnnotationMessage = (string)nv);
        public static readonly BindableProperty CompletedCommandProperty = BindableProperty.Create(nameof(CompletedCommand), typeof(ICommand), typeof(AdvancedEntry), null, propertyChanged: (bo, ov, nv) => (bo as AdvancedEntry).CompletedCommand = (ICommand)nv);
        public static readonly BindableProperty AnnotationProperty = BindableProperty.Create(nameof(Annotation), typeof(AnnotationType), typeof(AdvancedEntry), AnnotationType.None, propertyChanged: (bo, ov, nv) => (bo as AdvancedEntry).Annotation = (AnnotationType)nv);
        public static readonly BindableProperty ValidationMessageProperty = BindableProperty.Create(nameof(ValidationMessage), typeof(string), typeof(AdvancedEntry), propertyChanged: (bo, ov, nv) => (bo as AdvancedEntry).ValidationMessage = (string)nv);
        public static readonly BindableProperty IgnoreValidationMessageProperty = BindableProperty.Create(nameof(IgnoreValidationMessage), typeof(bool), typeof(AdvancedEntry), false, propertyChanged: (bo, ov, nv) => (bo as AdvancedEntry).DisplayValidation());
        public static readonly BindableProperty IsRequiredProperty = BindableProperty.Create(nameof(IsRequired), typeof(bool), typeof(AdvancedEntry), false, propertyChanged: (bo, ov, nv) => (bo as AdvancedEntry).UpdateWarning());
        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(AdvancedEntry), propertyChanged: (bo, ov, nv) => (bo as AdvancedEntry).CommandParameter = nv);
        public static readonly BindableProperty RegexPatternProperty = BindableProperty.Create(nameof(RegexPattern), typeof(string), typeof(AdvancedEntry), "", propertyChanged: (bo, ov, nv) => { (bo as AdvancedEntry).DisplayValidation(); (bo as AdvancedEntry).UpdateWarning(); });
        public static readonly BindableProperty TextFontSizeProperty = BindableProperty.Create(nameof(TextFontSize), typeof(double), typeof(AdvancedEntry), Device.GetNamedSize(NamedSize.Default, typeof(Label)), propertyChanged: (bo, ov, nv) => (bo as AdvancedEntry).txtInput.FontSize = (double)nv);
        public static readonly BindableProperty HorizontalTextAlignmentProperty = BindableProperty.Create(nameof(HorizontalTextAlignment), typeof(TextAlignment), typeof(AdvancedEntry), TextAlignment.Start, propertyChanged: (bo, ov, nv) => (bo as AdvancedEntry).txtInput.HorizontalTextAlignment = (TextAlignment)nv);
        public static readonly BindableProperty NullableProperty = BindableProperty.Create(nameof(Nullable), typeof(bool), typeof(AdvancedEntry), false, propertyChanged: (bo, ov, nv) => (bo as AdvancedEntry).Nullable = (bool)nv);
        public static readonly BindableProperty ValidationPositionProperty = BindableProperty.Create(
                                propertyName: nameof(ValidationPosition), declaringType: typeof(AdvancedEntry),
                                returnType: typeof(LabelPosition), defaultBindingMode: BindingMode.TwoWay,
                                defaultValue: GlobalSetting.LabelPosition,
                                propertyChanged: (bo, ov, nv) => (bo as AdvancedEntry).ApplyValidationPosition((LabelPosition)nv));
        public static readonly BindableProperty CursorPositionProperty = BindableProperty.Create(nameof(CursorPosition), typeof(int), typeof(AdvancedEntry), 0, propertyChanged: (bo, ov, nv) => (bo as AdvancedEntry).txtInput.CursorPosition = (int)nv);
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
        /// <summary>
        /// Resets of current annotation check and hides annotation message 
        /// </summary>
        public void Reset()
        {
            txtInput.Text = null;
            AnnotationMessage = null;
            imgWarning.IsVisible = false;
        }
        private void TxtInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            SetValue(TextProperty, txtInput.Text);
            SetValue(IsAnnotatedProperty, IsAnnotated);

            UpdateWarning();
            if (!IgnoreValidationMessage)
                DisplayValidation();
            TextChanged?.Invoke(this, e);
        }
        [Obsolete("Keyboard won't be changed automaticly on newer versions. Try set Keyboard property", false)]
        public void UpdateKeyboard(AnnotationType annotation)
        {
            switch (annotation)
            {
                case AnnotationType.None:
                    txtInput.Keyboard = Keyboard.Default;
                    break;
                case AnnotationType.LettersOnly:
                    txtInput.Keyboard = Keyboard.Plain;
                    break;
                case AnnotationType.DigitsOnly:
                case AnnotationType.ShortType:
                case AnnotationType.IntType:
                case AnnotationType.LongType:
                case AnnotationType.FloatType:
                case AnnotationType.DoubleType:
                case AnnotationType.DecimalType:
                case AnnotationType.ByteType:
                case AnnotationType.SByteType:
                case AnnotationType.CharType:
                case AnnotationType.UIntType:
                case AnnotationType.ULongType:
                case AnnotationType.UShortType:
                case AnnotationType.Decimal:
                    txtInput.Keyboard = Keyboard.Numeric;
                    break;
                case AnnotationType.NonDigitsOnly:
                    txtInput.Keyboard = Keyboard.Text;
                    break;
                case AnnotationType.Email:
                    txtInput.Keyboard = Keyboard.Email;
                    break;
                case AnnotationType.Password:
                    txtInput.Keyboard = Keyboard.Chat;
                    break;
                case AnnotationType.Phone:
                    txtInput.Keyboard = Keyboard.Telephone;
                    break;
            }
        }

        /// <summary>
        /// Triggers to display annotation message
        /// </summary>
        public void DisplayValidation()
        {
            if (!IsValidated)
            {
                AnnotationMessage = ValidationMessage;
                //AnnotationColor = Color.Red;
            }
            else
            {
                AnnotationMessage = null;
                //AnnotationColor = _defaultAnnotationColor;
            }
        }

        private void UpdateWarning()
        {
            ValidationChanged?.Invoke(this, new EventArgs());
            imgWarning.IsVisible = IsRequired && !IsAnnotated;
        }

        private void ApplyValidationPosition(LabelPosition position)
        {
            Children.Remove(lblAnnotation);
            switch (position)
            {
                case LabelPosition.Before:
                    Children.Insert(1, lblAnnotation);
                    break;
                case LabelPosition.After:
                    Children.Add(lblAnnotation);
                    break;
            }
        }

        private protected virtual Entry GetInputEntry()
        {
            return new EntryView
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
