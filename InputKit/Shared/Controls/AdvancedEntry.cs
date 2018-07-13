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
    [Obsolete("This will be remove d after newer versions.")]
    public class AdvancedEntry : StackLayout, IValidatable
    {
        /// <summary>
        /// This settings will be replaced default values of all AdvancedEntries
        /// </summary>
        public static GlobalSetting GlobalSetting { get; private set; } = new GlobalSetting
        {
            BackgroundColor = Color.White,
            CornerRadius = 20,
            BorderColor = Color.Transparent,
            Color = Color.Accent,
            FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
            Size = -1,
            TextColor = Color.Black,
        };

        Label lblTitle = new Label { Margin = new Thickness(6, 0, 0, 0), IsVisible = false, TextColor = GlobalSetting.TextColor, LineBreakMode = LineBreakMode.TailTruncation, };
        Label lblAnnotation = new Label { Margin = new Thickness(6, 0, 0, 0), IsVisible = false, FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)), Opacity = 0.8, TextColor = GlobalSetting.TextColor };
        Frame frmBackground = new Frame { BackgroundColor = GlobalSetting.BackgroundColor, CornerRadius = (float)GlobalSetting.CornerRadius, Padding = 0 };
        Image imgWarning = new Image { Margin = 10, HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center, InputTransparent = true, Source = "https://github.com/google/material-design-icons/blob/master/alert/drawable-mdpi/ic_warning_black_24dp.png" };
        IconView imgIcon = new IconView { InputTransparent = true, Margin = 10, VerticalOptions = LayoutOptions.CenterAndExpand, HeightRequest = 30, FillColor = GlobalSetting.Color };
        Entry txtInput = new EmptyEntry { TextColor = GlobalSetting.TextColor, PlaceholderColor = Color.LightGray, HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Center };
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
            txtInput.Completed += (s, args) => { CompletedCommand?.Execute(s); Completed?.Invoke(this, new EventArgs()); FocusNext(); };
            imgWarning.IsVisible = this.IsRequired;
        }


        #region Not Implemented
        public event EventHandler Clicked;
        public bool IsSelected { get => false; set { } }
        public object Value { get; set; }
        public bool IsValidated => IsAnnotated;
        private Color _defaultAnnotationColor = Color.Gray;
        #endregion



        private AnnotationType _annotation;
        private bool _isDisabled;
        private bool _isRequired;
        private int _minLength;

        public event EventHandler Completed;
        public event EventHandler ValidationChanged;
        public new void Focus()
        {
            txtInput.Focus();
        }
        public new void Unfocus()
        {
            txtInput.Unfocus();
        }
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

        private void TxtInput_TextChanged(object sender, TextChangedEventArgs e)
        {

            SetValue(TextProperty, txtInput.Text);
            SetValue(IsAnnotatedProperty, IsAnnotated);
            ValidationChanged?.Invoke(this, new EventArgs());

            UpdateWarning();
            if (!IgnoreValidationMessage)
                DisplayValidation();
        }

        public string Text { get => txtInput.Text; set => txtInput.Text = value; }
        public string Title { get => lblTitle.Text; set { lblTitle.Text = value; lblTitle.IsVisible = !String.IsNullOrEmpty(value); } }
        public string IconImage { get => imgIcon.Source.ToString(); set => imgIcon.Source = value; }
        public string Placeholder { get => txtInput.Placeholder; set => txtInput.Placeholder = value; }
        public int MaxLength
        {
            get => txtInput.MaxLength;
            set => txtInput.MaxLength = value;
        }
        public int MinLength { get => _minLength; set { _minLength = value; UpdateWarning(); DisplayValidation(); } }
        public string AnnotationMessage
        {
            get => lblAnnotation.Text;
            set { lblAnnotation.Text = value; lblAnnotation.IsVisible = !String.IsNullOrEmpty(value); }
        }
        public Color AnnotationColor
        {
            get => lblAnnotation.TextColor;
            set { lblAnnotation.TextColor = value; _defaultAnnotationColor = value; }
        }
        public AnnotationType Annotation { get => _annotation; set { _annotation = value; UpdateKeyboard(value); } }
        public bool IsDisabled
        {
            get => _isDisabled; set
            {
                _isDisabled = value;
                this.Opacity = value ? 0.6 : 1;
                txtInput.IsEnabled = !value;
            }
        }
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
                        return Text.Trim().Contains(" ");
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
                }
                return true;
            }

            set { }
        }

        public bool IsRequired { get => _isRequired; set { _isRequired = value; UpdateWarning(); } }
        public string ValidationMessage { get; set; }
        public bool IgnoreValidationMessage { get; set; }
        public ICommand CompletedCommand { get; set; }


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
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        #endregion

        void UpdateKeyboard(AnnotationType annotation)
        {
            switch (annotation)
            {
                case AnnotationType.Letter:
                case AnnotationType.Text:
                    txtInput.Keyboard = Keyboard.Chat;
                    break;
                case AnnotationType.Integer:
                case AnnotationType.Number:
                case AnnotationType.Money:
                    txtInput.Keyboard = Keyboard.Numeric;
                    break;
                case AnnotationType.Email:
                    txtInput.Keyboard = Keyboard.Email;
                    break;
                case AnnotationType.Phone:
                    txtInput.Keyboard = Keyboard.Telephone;
                    break;
                case AnnotationType.Password:
                    txtInput.Keyboard = Keyboard.Plain;
                    txtInput.IsPassword = true;
                    break;
                default:
                    txtInput.Keyboard = Keyboard.Default;
                    break;
            }
        }

        public void DisplayValidation()
        {
            if (!this.IsValidated)
            {
                //txtInput.TextColor = Color.Red;
                AnnotationMessage = ValidationMessage;
                AnnotationColor = Color.Red;
            }
            else
            {
                //if (!String.IsNullOrEmpty(ValidationMessage)) return;
                AnnotationMessage = null;
                AnnotationColor = _defaultAnnotationColor;
            }
        }

        void UpdateWarning()
        {
            imgWarning.IsVisible = this.IsRequired && !this.IsAnnotated;
        }

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
            Phone
        }
    }


    internal class EmptyEntry : Entry
    {

    }


}
