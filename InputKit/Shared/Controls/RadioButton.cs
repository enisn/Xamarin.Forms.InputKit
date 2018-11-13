using Plugin.InputKit.Shared.Abstraction;
using Plugin.InputKit.Shared.Configuration;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Plugin.InputKit.Shared.Controls
{
    /// <summary>
    /// Groups radiobuttons, Inherited StackLayout.
    /// </summary>
    public class RadioButtonGroupView : StackLayout, IValidatable
    {
        ///-----------------------------------------------------------------------------
        /// <summary>
        /// Default constructor of RadioButtonGroupView
        /// </summary>
        public RadioButtonGroupView()
        {
            this.ChildAdded += RadioButtonGroupView_ChildAdded;
            this.ChildrenReordered += RadioButtonGroupView_ChildrenReordered;
        }
        ///-----------------------------------------------------------------------------
        /// <summary>
        /// Invokes when tapped on RadioButon
        /// </summary>
        public event EventHandler SelectedItemChanged;
        /// <summary>
        /// Implementation of IValidatable, Triggered when value changed.
        /// </summary>
        public event EventHandler ValidationChanged;

        ///-----------------------------------------------------------------------------
        /// <summary>
        /// Executes when tapped on RadioButton
        /// </summary>
        public ICommand SelectedItemChangedCommand { get; set; }
        /// <summary>
        /// Command Parameter will be sent in SelectedItemChangedCommand
        /// </summary>
        public object CommandParameter { get; set; }
        private void RadioButtonGroupView_ChildrenReordered(object sender, EventArgs e)
        {
            UpdateAllEvent();
        }
        private void UpdateAllEvent()
        {
            foreach (var item in this.Children)
            {
                if (item is RadioButton)
                {
                    (item as RadioButton).Clicked -= UpdateSelected;
                    (item as RadioButton).Clicked += UpdateSelected;
                }
            }
        }
        private void RadioButtonGroupView_ChildAdded(object sender, ElementEventArgs e)
        {
            if (e.Element is RadioButton)
            {
                (e.Element as RadioButton).Clicked -= UpdateSelected;
                (e.Element as RadioButton).Clicked += UpdateSelected;
            }
        }
        void UpdateSelected(object selected, EventArgs e)
        {
            foreach (var item in this.Children)
            {
                if (item is RadioButton)
                    (item as RadioButton).IsChecked = item == selected;
            }

            SetValue(SelectedItemProperty, this.SelectedItem);
            OnPropertyChanged(nameof(SelectedItem));
            SetValue(SelectedIndexProperty, this.SelectedIndex);
            OnPropertyChanged(nameof(SelectedIndex));
            SelectedItemChanged?.Invoke(this, new EventArgs());
            if (SelectedItemChangedCommand?.CanExecute(CommandParameter ?? this) ?? false)
                SelectedItemChangedCommand?.Execute(CommandParameter ?? this);
            ValidationChanged?.Invoke(this, new EventArgs());
        }
        /// <summary>
        /// this will be added later
        /// </summary>
        public async void DisplayValidation()
        {
            this.BackgroundColor = Color.Red;
            await Task.Delay(500);
            this.BackgroundColor = Color.Transparent;
        }

        /// <summary>
        /// Returns selected radio button's index from inside of this.
        /// </summary>
        public int SelectedIndex
        {
            get
            {
                int index = 0;
                foreach (var item in this.Children)
                {
                    if (item is RadioButton)
                    {
                        if ((item as RadioButton).IsChecked)
                            return index;
                        index++;
                    }
                }
                return -1;
            }
            set
            {
                int index = 0;
                foreach (var item in this.Children)
                {
                    if (item is RadioButton)
                    {
                        (item as RadioButton).IsChecked = index == value;
                        index++;
                    }
                }
            }
        }
        ///-----------------------------------------------------------------------------
        /// <summary>
        /// Returns selected radio button's Value from inside of this.
        /// You can change the selectedItem too by sending a Value which matches ones of radio button's value
        /// </summary>
        public object SelectedItem
        {
            get
            {
                foreach (var item in this.Children)
                {
                    if (item is RadioButton && (item as RadioButton).IsChecked)
                        return (item as RadioButton).Value;
                }
                return null;
            }
            set
            {
                foreach (var item in this.Children)
                {
                    if (item is RadioButton)
                        (item as RadioButton).IsChecked = (item as RadioButton).Value == value;
                }
            }
        }
        ///-----------------------------------------------------------------------------
        /// <summary>
        /// It will be added later
        /// </summary>
        public bool IsRequired { get; set; }
        ///-----------------------------------------------------------------------------
        /// <summary>
        /// It will be added later
        /// </summary>
        public bool IsValidated { get => !this.IsRequired || this.SelectedIndex >= 0; }
        ///-----------------------------------------------------------------------------
        /// <summary>
        /// It will be added later
        /// </summary>
        public string ValidationMessage { get; set; }
        #region BindableProperties
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(RadioButtonGroupView), null, propertyChanged: (bo, ov, nv) => (bo as RadioButtonGroupView).SelectedItem = nv);
        public static readonly BindableProperty SelectedIndexProperty = BindableProperty.Create(nameof(SelectedIndex), typeof(int), typeof(RadioButtonGroupView), -1, BindingMode.TwoWay, propertyChanged: (bo, ov, nv) => (bo as RadioButtonGroupView).SelectedIndex = (int)nv);
        public static readonly BindableProperty SelectedItemChangedCommandProperty = BindableProperty.Create(nameof(SelectedItemChangedCommand), typeof(ICommand), typeof(RadioButtonGroupView), null, propertyChanged: (bo, ov, nv) => (bo as RadioButtonGroupView).SelectedItemChangedCommand = (ICommand)nv);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        #endregion
    }
    ///-----------------------------------------------------------------------------
    /// <summary>
    /// Radio Button with Text
    /// </summary>
    public class RadioButton : StackLayout
    {
        /// <summary>
        /// Default values of RadioButton
        /// </summary>
        public static GlobalSetting GlobalSetting { get; private set; } = new GlobalSetting
        {
            Color = Color.Accent,
            BorderColor = Color.Black,
            TextColor = (Color)Label.TextColorProperty.DefaultValue,
            Size = Device.GetNamedSize(Device.RuntimePlatform == Device.iOS ? NamedSize.Medium : NamedSize.Small, typeof(Label)),
            CornerRadius = -1,
            FontSize = Device.GetNamedSize(Device.RuntimePlatform == Device.iOS ? NamedSize.Medium : NamedSize.Small, typeof(Label)),
        };
        //.92
        //1.66 minReq

        Label lblEmpty = new Label { TextColor = GlobalSetting.BorderColor, Text = "◯", VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = GlobalSetting.Size, };
        Label lblFilled = new Label { TextColor = GlobalSetting.Color, Text = "●", IsVisible = false, Scale = 0.9, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center, FontSize = GlobalSetting.Size * .92 };
        Label lblText = new Label { Margin = new Thickness(0, 5, 0, 0), Text = "", VerticalTextAlignment = TextAlignment.Center, VerticalOptions = LayoutOptions.CenterAndExpand, TextColor = GlobalSetting.TextColor, FontSize = GlobalSetting.FontSize, FontFamily = GlobalSetting.FontFamily };
        private bool _isDisabled;

        ///-----------------------------------------------------------------------------
        /// <summary>
        /// Default Constructor
        /// </summary>
        public RadioButton()
        {
            if (Device.RuntimePlatform != Device.iOS)
                lblText.FontSize = lblText.FontSize *= 1.5;
            lblEmpty.FontSize = lblText.FontSize * 1.3;
            lblFilled.FontSize = lblText.FontSize * 1.3;
            Orientation = StackOrientation.Horizontal;
            this.Children.Add(new Grid
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Children =
                {
                    lblEmpty,
                    lblFilled
                },
                MinimumWidthRequest = GlobalSetting.Size * 1.66,
            });
            this.Children.Add(lblText);
            this.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(Tapped) });
        }
        ///-----------------------------------------------------------------------------
        /// <summary>
        /// Quick generating constructor.
        /// </summary>
        /// <param name="value">Value to keep in radio button</param>
        /// <param name="displayMember">If you send an ojbect as value. Which property will be displayed. Or override .ToString() inside of your object.</param>
        /// <param name="isChecked"> Checked or not situation</param>
        public RadioButton(object value, string displayMember, bool isChecked = false) : this()
        {
            this.Value = value;
            this.IsChecked = isChecked;
            string text;
            if (!String.IsNullOrEmpty(displayMember))
                text = value.GetType().GetProperty(displayMember)?.GetValue(value).ToString();
            else
                text = value.ToString();
            lblText.Text = text ?? " ";
        }
        ///-----------------------------------------------------------------------------
        /// <summary>
        /// Quick generating constructor.
        /// </summary>
        /// <param name="text">Text to display right of Radio button </param>
        /// <param name="isChecked">IsSelected situation</param>
        public RadioButton(string text, bool isChecked = false) : this()
        {
            Value = text;
            lblText.Text = text;
            this.IsChecked = isChecked;
        }
        ///-----------------------------------------------------------------------------
        /// <summary>
        /// Click event, triggered when clicked
        /// </summary>
        public event EventHandler Clicked;
        ///-----------------------------------------------------------------------------
        /// <summary>
        /// Click command, executed when clicked.  Parameter will be Value property if CommandParameter is not set
        /// </summary>
        public ICommand ClickCommand { get; set; }
        ///-----------------------------------------------------------------------------
        /// <summary>
        /// A command parameter will be sent to commands.
        /// </summary>
        public object CommandParameter { get; set; }
        ///-----------------------------------------------------------------------------
        /// <summary>
        /// Value to keep inside of Radio Button
        /// </summary>
        public object Value { get; set; }
        ///-----------------------------------------------------------------------------
        /// <summary>
        /// Gets or Sets, is that Radio Button selected/choosed/Checked
        /// </summary>
        public bool IsChecked { get => lblFilled.IsVisible; set { lblFilled.IsVisible = value; SetValue(IsCheckedProperty, value); } }
        ///-----------------------------------------------------------------------------
        /// <summary>
        /// this control if is Disabled
        /// </summary>
        public bool IsDisabled { get => _isDisabled; set { _isDisabled = value; this.Opacity = value ? 0.6 : 1; } }
        ///-----------------------------------------------------------------------------
        /// <summary>
        /// Text Description of Radio Button. It will be displayed right of Radio Button
        /// </summary>
        public string Text { get => lblText.Text; set => lblText.Text = value; }
        /// <summary>
        /// Fontsize of Description Text
        /// </summary>
        public double TextFontSize { get => lblText.FontSize; set { lblText.FontSize = value; } }
        ///-----------------------------------------------------------------------------
        /// <summary>
        /// Size of Radio Button
        /// </summary>
        public double CircleSize { get => lblEmpty.FontSize; set => SetCircleSize(value); }
        ///-----------------------------------------------------------------------------
        /// <summary>
        /// To be added.
        /// </summary>
        public string FontFamily { get => lblText.FontFamily; set => lblText.FontFamily = value; }
        ///-----------------------------------------------------------------------------
        /// <summary>
        /// Color of Radio Button's checked.
        /// </summary>
        public Color Color { get => lblFilled.TextColor; set => lblFilled.TextColor = value; }
        ///-----------------------------------------------------------------------------
        /// <summary>
        /// Color of radio button's outline border 
        /// </summary>
        public Color CircleColor { get => lblEmpty.TextColor; set => lblEmpty.TextColor = value; }
        ///-----------------------------------------------------------------------------
        /// <summary>
        /// Color of description text of Radio Button
        /// </summary>
        public Color TextColor { get => lblText.TextColor; set => lblText.TextColor = value; }
        #region BindableProperties
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(nameof(IsChecked), typeof(bool), typeof(RadioButton), false, propertyChanged: (bo, ov, nv) => (bo as RadioButton).IsChecked = (bool)nv);
        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(RadioButton), "", propertyChanged: (bo, ov, nv) => (bo as RadioButton).Text = (string)nv);
        public static readonly BindableProperty TextFontSizeProperty = BindableProperty.Create(nameof(TextFontSize), typeof(double), typeof(RadioButton), 20.0, propertyChanged: (bo, ov, nv) => (bo as RadioButton).TextFontSize = (double)nv);
        public static readonly BindableProperty ColorProperty = BindableProperty.Create(nameof(Color), typeof(Color), typeof(RadioButton), Color.Default, propertyChanged: (bo, ov, nv) => (bo as RadioButton).Color = (Color)nv);
        public static readonly BindableProperty CircleColorProperty = BindableProperty.Create(nameof(CircleColor), typeof(Color), typeof(RadioButton), Color.Default, propertyChanged: (bo, ov, nv) => (bo as RadioButton).CircleColor = (Color)nv);
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(RadioButton), Color.Default, propertyChanged: (bo, ov, nv) => (bo as RadioButton).TextColor = (Color)nv);
        public static readonly BindableProperty ClickCommandProperty = BindableProperty.Create(nameof(ClickCommand), typeof(ICommand), typeof(RadioButton), null, propertyChanged: (bo, ov, nv) => (bo as RadioButton).ClickCommand = (ICommand)nv);
        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(RadioButton), propertyChanged: (bo, ov, nv) => (bo as RadioButton).CommandParameter = nv);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        #endregion

        ///-----------------------------------------------------------------------------
        /// <summary>
        /// That handles tapps and triggers event, commands etc.
        /// </summary>
        void Tapped()
        {
            if (IsDisabled) return;
            IsChecked = !IsChecked;
            Clicked?.Invoke(this, new EventArgs());
            ClickCommand?.Execute(CommandParameter ?? Value);
        }
        ///-----------------------------------------------------------------------------
        /// <summary>
        /// Sets size of Circle
        /// </summary>
        void SetCircleSize(double value)
        {
            lblEmpty.FontSize = value;
            lblFilled.FontSize = value * .92;
            if (this.Children.Count > 0)
                this.Children[0].MinimumWidthRequest = value * 1.66;
        }
    }
}
