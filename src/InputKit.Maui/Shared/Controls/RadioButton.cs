using InputKit.Shared.Configuration;
using InputKit.Shared.Helpers;
using InputKit.Shared.Layouts;
using Microsoft.Maui.Controls.Shapes;
using System.ComponentModel;
using System.Windows.Input;
using Path = Microsoft.Maui.Controls.Shapes.Path;

namespace InputKit.Shared.Controls;

/// <summary>
/// Radio Button with Text
/// </summary>
[ContentProperty(nameof(ContentProxy))]
public class RadioButton : StatefulStackLayout
{
    #region Statics
    /// <summary>
    /// Default values of RadioButton
    /// </summary>
    public static GlobalSetting GlobalSetting { get; private set; } = new GlobalSetting
    {
        Color = InputKitOptions.GetAccentColor(),
        BorderColor = Application.Current.PlatformAppTheme == AppTheme.Dark ? Colors.WhiteSmoke : Colors.Black,
        TextColor = (Color)Label.TextColorProperty.DefaultValue,
        Size = 25,
        CornerRadius = -1,
        FontSize = 14,
        LabelPosition = LabelPosition.After,
        LineBreakMode = LineBreakMode.WordWrap
    };
    #endregion

    #region Fields
    protected internal Grid IconLayout;
    protected internal Ellipse iconCircle = new Ellipse
    {
        StrokeThickness = 2,
        Stroke = GlobalSetting.BorderColor,
        VerticalOptions = LayoutOptions.Center,
        HorizontalOptions = LayoutOptions.Center,
        HeightRequest = GlobalSetting.Size,
        WidthRequest = GlobalSetting.Size,
    };
    protected internal Path iconChecked = new Path
    {
        Fill = GlobalSetting.Color,
        Scale = 0,
        Aspect = Stretch.Uniform,
        VerticalOptions = LayoutOptions.Center,
        HorizontalOptions = LayoutOptions.Center,
        HeightRequest = GlobalSetting.Size,
        WidthRequest = GlobalSetting.Size,
    };
    protected internal ContentView contentHolder = new ContentView();
    protected internal Label lblText = new Label
    {
        LineBreakMode = GlobalSetting.LineBreakMode,
        VerticalOptions = LayoutOptions.Center,
        HorizontalOptions = LayoutOptions.Start,
        FontSize = GlobalSetting.FontSize,
        TextColor = GlobalSetting.TextColor,
        FontFamily = GlobalSetting.FontFamily,
        IsVisible = false
    };

    public View ContentProxy { get => contentHolder.Content; set => contentHolder.Content = value; }

    private bool _isDisabled;
    protected const double DOT_FULL_SCALE = .65;
    #endregion

    #region Ctor
    /// <summary>
    /// Default Constructor
    /// </summary>
    public RadioButton()
    {
        Orientation = StackOrientation.Horizontal;
        Spacing = 10;

        ApplyIsCheckedAction = ApplyIsChecked;
        ApplyIsPressedAction = ApplyIsPressed;
        contentHolder.Content = lblText;
        IconLayout = new Grid
        {
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Center,
            Children =
            {
                iconCircle,
                iconChecked
            },
            MinimumWidthRequest = GlobalSetting.Size,
        };

        ApplyLabelPosition(LabelPosition);
        UpdateShape();

        GestureRecognizers.Add(new TapGestureRecognizer
        {
            Command = new Command(() =>
            {
                if (IsDisabled)
                {
                    return;
                }

                IsChecked = true;
            })
        });
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
    public object Value { get => GetValue(ValueProperty); set => SetValue(ValueProperty, value); }

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
    public string Text
    {
        get => lblText.Text; set
        {
            lblText.Text = value; lblText.IsVisible = !string.IsNullOrEmpty(value);
            contentHolder.Content = lblText;
        }
    }
    /// <summary>
    /// Fontsize of Description Text
    /// </summary>

    [System.ComponentModel.TypeConverter(typeof(FontSizeConverter))]
    public double TextFontSize { get => lblText.FontSize; set => lblText.FontSize = value; }


    [TypeConverter(typeof(PathGeometryConverter))]
    public Geometry SelectedIconGeomerty { get => (Geometry)GetValue(SelectedIconGeomertyProperty); set => SetValue(SelectedIconGeomertyProperty, value); }

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
    /// Gets or sets the label position.
    /// </summary>
    public LabelPosition LabelPosition
    {
        get => (LabelPosition)GetValue(LabelPositionProperty);
        set => SetValue(LabelPositionProperty, value);
    }

    /// <summary>
    /// Gets or sets the line break mode for the label.
    /// </summary>
    public LineBreakMode LineBreakMode { get => (LineBreakMode)GetValue(LineBreakModeProperty); set => SetValue(LineBreakModeProperty, value); }

    /// <summary>
    /// Gets or sets the vertical options for the icon.
    /// </summary>
    public LayoutOptions IconVerticalOptions { get => (LayoutOptions)GetValue(IconVerticalOptionsProperty); set => SetValue(IconVerticalOptionsProperty, value); }

    /// <summary>
    /// Gets or sets the horizontal options for the icon.
    /// </summary>
    public LayoutOptions IconHorizontalOptions { get => (LayoutOptions)GetValue(IconHorizontalOptionsProperty); set => SetValue(IconHorizontalOptionsProperty, value); }

    #endregion

    #region BindableProperties
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(nameof(IsChecked), typeof(bool), typeof(RadioButton), false, BindingMode.TwoWay, propertyChanged: (bo, ov, nv) => (bo as RadioButton).ApplyIsCheckedAction((bool)nv));
    public static readonly BindableProperty IsDisabledProperty = BindableProperty.Create(nameof(IsDisabled), typeof(bool), typeof(RadioButton), false, propertyChanged: (bo, ov, nv) => (bo as RadioButton).IsDisabled = (bool)nv);
    public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(RadioButton), "", propertyChanged: (bo, ov, nv) => (bo as RadioButton).Text = (string)nv);
    public static readonly BindableProperty TextFontSizeProperty = BindableProperty.Create(nameof(TextFontSize), typeof(double), typeof(RadioButton), 20.0, propertyChanged: (bo, ov, nv) => (bo as RadioButton).TextFontSize = (double)nv);
    public static readonly BindableProperty ColorProperty = BindableProperty.Create(nameof(Color), typeof(Color), typeof(RadioButton), GlobalSetting.Color, propertyChanged: (bo, ov, nv) => (bo as RadioButton).UpdateColors());
    public static readonly BindableProperty CircleColorProperty = BindableProperty.Create(nameof(CircleColor), typeof(Color), typeof(RadioButton), GlobalSetting.BorderColor, propertyChanged: (bo, ov, nv) => (bo as RadioButton).UpdateColors());
    public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(RadioButton), GlobalSetting.TextColor, propertyChanged: (bo, ov, nv) => (bo as RadioButton).UpdateColors());
    public static readonly BindableProperty ClickCommandProperty = BindableProperty.Create(nameof(ClickCommand), typeof(ICommand), typeof(RadioButton), null, propertyChanged: (bo, ov, nv) => (bo as RadioButton).ClickCommand = (ICommand)nv);
    public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(RadioButton), propertyChanged: (bo, ov, nv) => (bo as RadioButton).CommandParameter = nv);
    public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(RadioButton), propertyChanged: (bo, ov, nv) => (bo as RadioButton).FontFamily = (string)nv);
    public static readonly BindableProperty SelectedIconGeomertyProperty = BindableProperty.Create(nameof(SelectedIconGeomerty), typeof(Geometry), typeof(RadioButton), PredefinedShapes.Dot, propertyChanged: (bo, ov, nv) => (bo as RadioButton).UpdateShape());
    public static readonly BindableProperty LabelPositionProperty = BindableProperty.Create(
        propertyName: nameof(LabelPosition), declaringType: typeof(RadioButton),
        returnType: typeof(LabelPosition), defaultBindingMode: BindingMode.TwoWay,
        defaultValue: GlobalSetting.LabelPosition,
        propertyChanged: (bo, ov, nv) => (bo as RadioButton).ApplyLabelPosition((LabelPosition)nv));


    public static readonly BindableProperty LineBreakModeProperty = BindableProperty.Create(nameof(LineBreakMode), typeof(LineBreakMode), typeof(RadioButton), defaultValue: GlobalSetting.LineBreakMode,
        propertyChanged: (bindable, oldValue, newValue) => (bindable as RadioButton).lblText.LineBreakMode = (LineBreakMode)newValue);

    public static readonly BindableProperty IconVerticalOptionsProperty = BindableProperty.Create(nameof(IconVerticalOptions), typeof(LayoutOptions), typeof(RadioButton), defaultValue: LayoutOptions.Center,
        propertyChanged: (bindable, oldValue, newValue) => (bindable as RadioButton).IconLayout.VerticalOptions = (LayoutOptions)newValue);

    public static readonly BindableProperty IconHorizontalOptionsProperty = BindableProperty.Create(nameof(IconHorizontalOptions), typeof(LayoutOptions), typeof(RadioButton), defaultValue: LayoutOptions.Center,
        propertyChanged: (bindable, oldValue, newValue) => (bindable as RadioButton).IconLayout.HorizontalOptions = (LayoutOptions)newValue);

    public static readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(object), typeof(RadioButton), defaultBindingMode: BindingMode.OneTime);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    #endregion

    #region Methods
    private void ApplyLabelPosition(LabelPosition position)
    {
        Children.Clear();
        if (position == LabelPosition.After)
        {
            lblText.HorizontalOptions = LayoutOptions.Start;
            Children.Add(IconLayout);
            Children.Add(contentHolder);
        }
        else
        {
            lblText.HorizontalOptions = LayoutOptions.FillAndExpand;
            Children.Add(contentHolder);
            Children.Add(IconLayout);
        }
    }

    protected override async void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

        // TODO: Remove this logic after resolution of https://github.com/dotnet/maui/issues/8873
        // This is a workaround.

#if ANDROID
        await Task.Delay(1);
#endif
        if (IconLayout.Width != -1 && lblText.Width > this.Width)
        {
            lblText.MaximumWidthRequest = this.Width - this.Spacing - IconLayout.Width;
        }
    }

    private protected virtual void UpdateShape()
    {
        iconChecked.Data = SelectedIconGeomerty;
    }

    protected void UpdateColors()
    {
        iconChecked.Fill = Color;
        iconCircle.Stroke = IsChecked ? Color : CircleColor;
        lblText.TextColor = TextColor;
    }

    public virtual void ApplyIsChecked(bool isChecked)
    {
        Clicked?.Invoke(this, new EventArgs());
        ClickCommand?.Execute(CommandParameter ?? Value);

        var isCheckedInLastState = iconChecked.Scale == DOT_FULL_SCALE;

        var changed = isCheckedInLastState != isChecked;

        if (changed)
        {
            iconChecked.ScaleTo(isChecked ? DOT_FULL_SCALE : 0, 180);
            UpdateColors();
            Checked?.Invoke(this, null);
        }

        var state = isChecked ? VisualStateManager.CommonStates.Selected : VisualStateManager.CommonStates.Normal;
        VisualStateManager.GoToState(this, state);
        DefaultVisualState = state;
    }

    public virtual async void ApplyIsPressed(StatefulStackLayout statefulLayout, bool isPressed)
    {
        if (statefulLayout is RadioButton radioButton)
        {
            await radioButton.IconLayout.ScaleTo(isPressed ? .8 : 1, 100);
        }
    }
    #endregion
}
