using Plugin.InputKit.Shared.Abstraction;
using Plugin.InputKit.Shared.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Plugin.InputKit.Shared.Controls
{
    /// <summary>
    /// This Entry contains validation and some stuffs inside
    /// </summary>
    public class AdvancedEntry : StackLayout, IValidatable
    {
        /// <summary>
        /// This settings will be replaced default values of all AdvancedEntries
        /// </summary>
        public static GlobalSetting GlobalSetting { get; private set; } = new GlobalSetting
        {
            BackgroundColor = Color.White,
            CornerRadius = 20,
            BorderColor = (Color)Frame.BorderColorProperty.DefaultValue,
            Color = Color.Accent,
            FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
            Size = -1,
            TextColor = (Color)Entry.TextColorProperty.DefaultValue,
        };

        Label lblTitle = new Label { Margin = new Thickness(6, 0, 0, 0), IsVisible = false, TextColor = GlobalSetting.TextColor, LineBreakMode = LineBreakMode.TailTruncation, FontFamily = GlobalSetting.FontFamily };
        Label lblAnnotation = new Label { Margin = new Thickness(6, 0, 0, 0), IsVisible = false, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)), Opacity = 0.8, TextColor = GlobalSetting.TextColor, FontFamily = GlobalSetting.FontFamily };
        Frame frmBackground = new Frame { BackgroundColor = GlobalSetting.BackgroundColor, CornerRadius = (float)GlobalSetting.CornerRadius, BorderColor = GlobalSetting.BorderColor, Padding = 0 };
        Image imgWarning = new Image { Margin = 10, HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center, InputTransparent = true, Source = "alert.png" };
        IconView imgIcon = new IconView { InputTransparent = true, Margin = 10, VerticalOptions = LayoutOptions.CenterAndExpand, HeightRequest = 30, FillColor = GlobalSetting.Color };
        Entry txtInput = new EmptyEntry { TextColor = GlobalSetting.TextColor, PlaceholderColor = Color.LightGray, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Center, FontFamily = GlobalSetting.FontFamily };
        /// <summary>
        /// Default Constructor
        /// </summary>
        public AdvancedEntry()
        {
            this.Children.Add(lblTitle);
            this.Children.Add(lblAnnotation);
            this.Children.Add(frmBackground);
            frmBackground.Content = new Grid
            {
                Children =
                {
                    new StackLayout { Orientation = StackOrientation.Horizontal,
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
            imgWarning.IsVisible = this.IsRequired;
        }
        void ExecuteCommand()
        {
            if (CompletedCommand?.CanExecute(CommandParameter ?? this) ?? false)
                CompletedCommand?.Execute(CommandParameter ?? this);
        }
        #region Not Implemented
        public bool IsSelected { get => false; set { } }
        public object Value { get; set; }
        public bool IsValidated => IsAnnotated;
        #endregion
        #region Fields
        private Color _defaultAnnotationColor = Color.Gray;
        private AnnotationType _annotation;
        private bool _isDisabled;
        private int _minLength;
        private string _validationMessage;
        #endregion
        public event EventHandler Completed;
        public event EventHandler<TextChangedEventArgs> TextChanged;
        public event EventHandler Clicked;
        public event EventHandler ValidationChanged;
        /// <summary>
        /// Focus on this entry
        /// </summary>
        public new void Focus()
        {
            txtInput.Focus();
        }
        /// <summary>
        /// Onfocus from this entry and hides keyboard.
        /// </summary>
        public new void Unfocus()
        {
            txtInput.Unfocus();
        }
        /// <summary>
        /// Automaticly finds next Advanced entry and focus it.
        /// </summary>
        public void FocusNext()
        {
            if (this.Parent is Layout<View> == false) return;

            Layout<View> parent = this.Parent as Layout<View>;

            int index = parent.Children.IndexOf(this);
            for (int i = index + 1; i < (index + 4).Clamp(0, parent.Children.Count); i++)
            {
                if (parent.Children[i] is AdvancedEntry)
                {
                    (parent.Children[i] as AdvancedEntry).Focus();
                    break;
                }
            }
        }
        /// <summary>
        /// Resets of current annotation check and hides annotation message 
        /// </summary>
        public void Reset()
        {
            txtInput.Text = null;
            this.AnnotationMessage = null;
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
        ///------------------------------------------------------------------------
        /// <summary>
        /// Text of this input
        /// </summary>
        public string Text { get => txtInput.Text; set => txtInput.Text = value; }
        ///------------------------------------------------------------------------
        /// <summary>
        /// Title will be shown top of this control
        /// </summary>
        public string Title { get => lblTitle.Text; set { lblTitle.Text = value; lblTitle.IsVisible = !String.IsNullOrEmpty(value); } }
        ///------------------------------------------------------------------------
        /// <summary>
        /// Icons of this Entry
        /// </summary>
        public string IconImage { get => imgIcon.Source.ToString(); set => imgIcon.Source = value; }
        ///------------------------------------------------------------------------
        /// <summary>
        /// Color of Icon
        /// </summary>
        public Color IconColor { get => imgIcon.FillColor; set => imgIcon.FillColor = value; }
        ///------------------------------------------------------------------------
        /// <summary>
        /// BackgroundColor of this Control
        /// </summary>
        public new Color BackgroundColor { get => frmBackground.BackgroundColor; set => frmBackground.BackgroundColor = value; }
        ///------------------------------------------------------------------------
        /// <summary>
        /// Bordercolor of this control
        /// </summary>
        public Color BorderColor { get => frmBackground.BorderColor; set => frmBackground.BorderColor = value; }
        ///------------------------------------------------------------------------
        /// <summary>
        /// Text Color of this Control
        /// </summary>
        public Color TextColor { get => txtInput.TextColor; set => txtInput.TextColor = value; }
        ///------------------------------------------------------------------------
        /// <summary>
        /// BackgroundColor of this Control
        /// </summary>
        public Color PlaceholderColor { get => txtInput.PlaceholderColor; set => txtInput.PlaceholderColor = value; }

        ///------------------------------------------------------------------------
        /// <summary>
        /// Placeholder of entry
        /// </summary>
        public string Placeholder { get => txtInput.Placeholder; set => txtInput.Placeholder = value; }
        /// <summary>
        /// Maximum length of this Entry
        /// </summary>
        public int MaxLength
        {
            get => txtInput.MaxLength;
            set => txtInput.MaxLength = value;
        }
        ///------------------------------------------------------------------------
        /// <summary>
        /// Minimum length of this Entry
        /// </summary>
        public int MinLength { get => _minLength; set { _minLength = value; UpdateWarning(); /*DisplayValidation(); */} }
        ///------------------------------------------------------------------------
        /// <summary>
        /// Corner radius of Entry.
        /// </summary>
        public float CornerRadius { get => frmBackground.CornerRadius; set => frmBackground.CornerRadius = value; }
        ///------------------------------------------------------------------------
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
        ///------------------------------------------------------------------------
        /// <summary>
        /// This will be shown below title. This automaticly updating. If you set this manually you must set true IgnoreValidationMessage !!! 
        /// </summary>
        public string AnnotationMessage
        {
            get => lblAnnotation.Text;
            set
            {
                lblAnnotation.Text = value;
                lblAnnotation.IsVisible = !String.IsNullOrEmpty(value);
            }
        }
        ///------------------------------------------------------------------------
        /// <summary>
        /// AnnotationMessage's color.
        /// </summary>
        public Color AnnotationColor
        {
            get => lblAnnotation.TextColor;
            set { lblAnnotation.TextColor = value; _defaultAnnotationColor = value; }
        }
        ///------------------------------------------------------------------------
        /// <summary>
        /// will be added
        /// </summary>
        public AnnotationType Annotation { get => _annotation; set { _annotation = value; UpdateKeyboard(value); } }
        ///------------------------------------------------------------------------
        /// <summary>
        /// Disabled this control
        /// </summary>
        public bool IsDisabled
        {
            get => _isDisabled; set
            {
                _isDisabled = value;
                this.Opacity = value ? 0.6 : 1;
                txtInput.IsEnabled = !value;
            }
        }
        ///------------------------------------------------------------------------
        /// <summary>
        /// Finds this entry if Annotated
        /// </summary>
        public bool IsAnnotated
        {
            get
            {
                if (!this.IsRequired)
                    return true;
                if (String.IsNullOrEmpty(Text))
                    return false;
                if (Text.Length < MinLength)
                    return false;

                switch (Annotation)
                {
                    case AnnotationType.NameSurname:
                        return Text.Trim().Contains(" ") && Text.All((c) => !Char.IsNumber(c));
                    case AnnotationType.Letter:
                        return Text.All(Char.IsLetter);
                    case AnnotationType.Number:
                    case AnnotationType.Money:
                        return Text.All(c => Char.IsDigit(c) || c == ',');
                    case AnnotationType.Integer:
                        return Text.All(Char.IsDigit);
                    case AnnotationType.Phone:
                        return Text.All(Char.IsDigit) && this.Text.Length == this.MaxLength;
                    case AnnotationType.Text:
                        return Text.All(c => c == ' ' || !(Char.IsSymbol(c) || Char.IsSurrogate(c) || Char.IsControl(c) || Char.IsPunctuation(c)));
                    case AnnotationType.Email:
                        var splitted = Text.Split('@');
                        return Text.Contains("@") && splitted.Length == 2 && splitted.LastOrDefault().Length > 3 && splitted.LastOrDefault().Contains(".") && splitted.LastOrDefault().Split('.').LastOrDefault()?.Length >= 2;
                    case AnnotationType.Password:
                        return Text.Any(Char.IsDigit) && Text.Any(Char.IsLetter);
                    case AnnotationType.Regex:
                        return Regex.IsMatch(Text, RegexPattern);
                }
                return true;
            }
            set { }
        }
        ///------------------------------------------------------------------------
        /// <summary>
        /// Comes from IValidatable implementation. Shows this if Validated.
        /// </summary>
        public bool IsRequired { get => (bool)GetValue(IsRequiredProperty); set => SetValue(IsRequiredProperty, value); }
        ///------------------------------------------------------------------------
        /// <summary>
        /// Validation message to update automaticly. This will be shown when entry is not validated
        /// </summary>
        public string ValidationMessage { get => _validationMessage; set { _validationMessage = value; DisplayValidation(); } }
        ///------------------------------------------------------------------------
        /// <summary>
        /// Ignores automaticly update annotationmessage
        /// </summary>
        public bool IgnoreValidationMessage { get => (bool)GetValue(IgnoreValidationMessageProperty); set => SetValue(IgnoreValidationMessageProperty, value); }
        /// <summary>
        /// Executed when entry completed.
        /// </summary>
        public ICommand CompletedCommand { get; set; }
        /// <summary>
        /// Parameter to send with CompletedCommand
        /// </summary>
        public object CommandParameter { get => GetValue(CommandParameterProperty); set => SetValue(CommandParameterProperty, value); }
        /// <summary>
        /// You need to set Annotation="Regex" to use this.
        /// </summary>
        public string RegexPattern { get => (string)GetValue(RegexPatternProperty); set => SetValue(RegexPatternProperty,value); }
        /// <summary>
        /// Gets and sets keyboard type of this entry
        /// </summary>
        public Keyboard Keyboard { get => txtInput.Keyboard; set => txtInput.Keyboard = value; }

        //--------------------------------------------------------------------------------------------------------------------------------------------------
        #region BindableProperties
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(AdvancedEntry), null, BindingMode.TwoWay, propertyChanged: (bo, ov, nv) => (bo as AdvancedEntry).Text = (string)nv);
        public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(AdvancedEntry), null, propertyChanged: (bo, ov, nv) => (bo as AdvancedEntry).Title = (string)nv);
        public static readonly BindableProperty PlaceHolderProperty = BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(AdvancedEntry), null, propertyChanged: (bo, ov, nv) => (bo as AdvancedEntry).Placeholder = (string)nv);
        public static readonly BindableProperty IconImageProperty = BindableProperty.Create(nameof(IconImage), typeof(string), typeof(AdvancedEntry), null, propertyChanged: (bo, ov, nv) => (bo as AdvancedEntry).IconImage = (string)nv);
        public static readonly BindableProperty MaxLengthProperty = BindableProperty.Create(nameof(MaxLength), typeof(int), typeof(AdvancedEntry), 0, propertyChanged: (bo, ov, nv) => (bo as AdvancedEntry).MaxLength = (int)nv);
        public static readonly BindableProperty IsAnnotatedProperty = BindableProperty.Create(nameof(IsAnnotated), typeof(bool), typeof(AdvancedEntry), false, BindingMode.OneWayToSource);
        public static readonly BindableProperty AnnotationColorProperty = BindableProperty.Create(nameof(AnnotationColor), typeof(Color), typeof(AdvancedEntry), Color.Default, propertyChanged: (bo, ov, nv) => (bo as AdvancedEntry).AnnotationColor = (Color)nv);
        public static readonly BindableProperty AnnotationMessageProperty = BindableProperty.Create(nameof(AnnotationMessage), typeof(string), typeof(AdvancedEntry), "", propertyChanged: (bo, ov, nv) => (bo as AdvancedEntry).AnnotationMessage = (string)nv);
        public static readonly BindableProperty CompletedCommandProperty = BindableProperty.Create(nameof(CompletedCommand), typeof(ICommand), typeof(AdvancedEntry), null, propertyChanged: (bo, ov, nv) => (bo as AdvancedEntry).CompletedCommand = (ICommand)nv);
        public static readonly BindableProperty AnnotationProperty = BindableProperty.Create(nameof(Annotation), typeof(AnnotationType), typeof(AdvancedEntry), AdvancedEntry.AnnotationType.None, propertyChanged: (bo, ov, nv) => (bo as AdvancedEntry).Annotation = (AnnotationType)nv);
        public static readonly BindableProperty ValidationMessageProperty = BindableProperty.Create(nameof(ValidationMessage), typeof(string), typeof(AdvancedEntry), propertyChanged: (bo, ov, nv) => (bo as AdvancedEntry).ValidationMessage = (string)nv);
        public static readonly BindableProperty IgnoreValidationMessageProperty = BindableProperty.Create(nameof(IgnoreValidationMessage), typeof(bool), typeof(AdvancedEntry), false, propertyChanged: (bo, ov, nv) => (bo as AdvancedEntry).DisplayValidation());
        public static readonly BindableProperty IsRequiredProperty = BindableProperty.Create(nameof(IsRequired), typeof(bool), typeof(AdvancedEntry), false, propertyChanged: (bo, ov, nv) => (bo as AdvancedEntry).UpdateWarning());
        public static readonly BindableProperty PlaceholderColorProperty = BindableProperty.Create(nameof(PlaceholderColor), typeof(Color), typeof(AdvancedEntry), Color.LightGray, propertyChanged: (bo, ov, nv) => (bo as AdvancedEntry).PlaceholderColor = (Color)nv);
        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(AdvancedEntry), propertyChanged: (bo, ov, nv) => (bo as AdvancedEntry).CommandParameter = nv);
        public static readonly BindableProperty RegexPatternProperty = BindableProperty.Create(nameof(RegexPattern), typeof(string), typeof(AdvancedEntry), "", propertyChanged: (bo, ov, nv) => { (bo as AdvancedEntry).DisplayValidation(); (bo as AdvancedEntry).UpdateWarning(); } );
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        #endregion
        //--------------------------------------------------------------------------------------------------------------------------------------------------
        [Obsolete("Keyboard won't be changed automaticly on newer versions. Try set Keyboard property",false)]
        public void UpdateKeyboard(AnnotationType annotation)
        {
            switch (annotation)
            {
                case AnnotationType.Letter:
                case AnnotationType.Text:
                    txtInput.Keyboard = Keyboard.Chat;
                    txtInput.IsPassword = false;
                    break;
                case AnnotationType.Integer:
                case AnnotationType.Number:
                case AnnotationType.Money:
                    txtInput.Keyboard = Keyboard.Numeric;
                    txtInput.IsPassword = false;
                    break;
                case AnnotationType.Email:
                    txtInput.Keyboard = Keyboard.Email;
                    txtInput.IsPassword = false;
                    break;
                case AnnotationType.Phone:
                    txtInput.Keyboard = Keyboard.Telephone;
                    txtInput.IsPassword = false;
                    break;
                case AnnotationType.Password:
                    txtInput.Keyboard = Keyboard.Plain;
                    txtInput.IsPassword = true;
                    break;
                default:
                    txtInput.Keyboard = Keyboard.Default;
                    txtInput.IsPassword = false;
                    break;
            }
        }
        ///------------------------------------------------------------------------
        /// <summary>
        /// Triggers to display annotation message
        /// </summary>
        public void DisplayValidation()
        {
            if (!this.IsValidated)
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

        void UpdateWarning()
        {
            ValidationChanged?.Invoke(this, new EventArgs());
            imgWarning.IsVisible = this.IsRequired && !this.IsAnnotated;
        }
        /// <summary>
        /// Anum of Annotations. Detail will be added later.
        /// </summary>
        public enum AnnotationType
        {
            None,
            Letter,
            Integer,
            Number,
            Text,
            Money,
            Email,
            NameSurname,
            Password,
            Phone,
            Regex
        }
    }
}
