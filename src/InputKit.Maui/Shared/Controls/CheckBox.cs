using InputKit.Shared.Abstraction;
using InputKit.Shared.Configuration;
using InputKit.Shared.Helpers;
using InputKit.Shared.Layouts;
using Microsoft.Maui.Controls.Shapes;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Input;
using Path = Microsoft.Maui.Controls.Shapes.Path;

namespace InputKit.Shared.Controls;

/// <summary>
/// A checkbox for boolean inputs. It Includes a text inside
/// </summary>
public partial class CheckBox : StatefulStackLayout, IValidatable
{
    public static GlobalSetting GlobalSetting { get; } = new GlobalSetting
    {
        BackgroundColor = Colors.Transparent,
        Color = InputKitOptions.GetAccentColor(),
        BorderColor = Application.Current.PlatformAppTheme == AppTheme.Dark ? Colors.WhiteSmoke : Colors.Black,
        TextColor = (Color)Label.TextColorProperty.DefaultValue,
        Size = 25,
        CornerRadius = 2,
        FontSize = 14,
        LabelPosition = LabelPosition.After
    };

    #region Constants
    internal const double CHECK_SIZE_RATIO = .65;
    #endregion

    #region Fields
    protected internal Grid IconLayout;
    protected Rectangle outlineBox = new Rectangle
    {
        Fill = GlobalSetting.BackgroundColor,
        Stroke = GlobalSetting.BorderColor,
        StrokeThickness = 2,
        WidthRequest = GlobalSetting.Size,
        HeightRequest = GlobalSetting.Size,
        RadiusX = GlobalSetting.CornerRadius,
    };
    protected Path selectedIcon = new Path
    {
        Fill = GlobalSetting.Color,
        Aspect = Stretch.Uniform,
        HeightRequest = GlobalSetting.Size,
        WidthRequest = GlobalSetting.Size,
        MaximumHeightRequest = GlobalSetting.Size,
        MaximumWidthRequest = GlobalSetting.Size,
        Scale = 0,
    };
    protected internal Label lblOption = new Label
    {
        VerticalOptions = LayoutOptions.Center,
        HorizontalOptions = LayoutOptions.Start,
        FontSize = GlobalSetting.FontSize,
        TextColor = GlobalSetting.TextColor,
        FontFamily = GlobalSetting.FontFamily,
        IsVisible = false
    };
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
        Orientation = StackOrientation.Horizontal;
        Spacing = 10;
        Padding = new Thickness(0, 10);
        ApplyIsCheckedAction = ApplyIsChecked;
        ApplyIsPressedAction = ApplyIsPressed;

        IconLayout = new Grid
        {
            Children =
            {
                outlineBox,
                selectedIcon
            }
        };

        ApplyLabelPosition(LabelPosition);
        UpdateType();
        UpdateShape();
        GestureRecognizers.Add(new TapGestureRecognizer
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
    /// <summary>
    /// Method to run when check changed. Default value is <see cref="ApplyIsChecked(CheckBox, bool)"/> It's not recommended to change this field. But you can set your custom <see cref="void"/> if you really need.
    /// </summary>
    public Action<CheckBox, bool> ApplyIsCheckedAction { get; set; }

    /// <summary>
    /// Applies pressed effect. Default value is <see cref="ApplyIsChecked(CheckBox, bool)"/>. You can set another <see cref="void"/> to make custom pressed effects.
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
    public string Text { get => lblOption.Text; set { lblOption.Text = value; lblOption.IsVisible = !string.IsNullOrEmpty(value); } }

    /// <summary>
    /// IsChecked Property
    /// </summary>
    public bool IsChecked
    {
        get => (bool)GetValue(IsCheckedProperty);
        set => SetValue(IsCheckedProperty, value);
    }

    /// <summary>
    /// Checkbox box background color. Default is LightGray
    /// </summary>
    public Color BoxBackgroundColor { get => (Color)GetValue(BoxBackgroundColorProperty); set => SetValue(BoxBackgroundColorProperty, value); }

    /// <summary>
    /// Gets or sets the checkbutton enabled or not. If checkbox is disabled, checkbox can not be interacted.
    /// </summary>
    public bool IsDisabled { get => _isEnabled; set { _isEnabled = value; Opacity = value ? 0.6 : 1; } }

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
    /// Gets or sets icon color of checked state. If you leave null, checkbox will make a decision between Black and White depending on Color.
    /// </summary>
    public Color IconColor { get => (Color)GetValue(IconColorProperty); set => SetValue(IconColorProperty, value); }

    /// <summary>
    /// Which icon will be shown when checkbox is checked
    /// </summary>
    public CheckType Type { get => _type; set { _type = value; UpdateType(); } }

    /// <summary>
    /// Size of Checkbox
    /// </summary>
    public double BoxSize { get => outlineBox.Width; }

    /// <summary>
    /// SizeRequest of CheckBox
    /// </summary>
    public double BoxSizeRequest { get => outlineBox.WidthRequest; set => SetBoxSize(value); }

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
    public bool IsValidated => !IsRequired || IsChecked;
    /// <summary>
    /// Not available for this control
    /// </summary>
    public string ValidationMessage { get; set; }

    /// <summary>
    /// Fontfamily of CheckBox Text
    /// </summary>
    public string FontFamily { get => (string)GetValue(FontFamilyProperty); set => SetValue(FontFamilyProperty, value); }

    [Obsolete("This option is removed. Use CustomIconGeometry")]
    public ImageSource CustomIcon { get => default; set { } }

    [TypeConverter(typeof(PathGeometryConverter))]
    public Geometry IconGeometry { get => (Geometry)GetValue(IconGeometryProperty); set => SetValue(IconGeometryProperty, value); }

    public bool IsPressed { get; set; }
    /// <summary>
    /// Corner radius of Box of CheckBox.
    /// </summary>
    public float CornerRadius { get => (float)GetValue(CornerRadiusProperty); set => SetValue(CornerRadiusProperty, value); }
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
    public static readonly BindableProperty ColorProperty = BindableProperty.Create(nameof(Color), typeof(Color), typeof(CheckBox), GlobalSetting.Color, propertyChanged: (bo, ov, nv) => (bo as CheckBox).UpdateColors());
    public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(CheckBox), GlobalSetting.TextColor, propertyChanged: (bo, ov, nv) => (bo as CheckBox).TextColor = (Color)nv);
    public static readonly BindableProperty IconColorProperty = BindableProperty.Create(nameof(IconColor), typeof(Color), typeof(CheckBox), Colors.Transparent, propertyChanged: (bo, ov, nv) => (bo as CheckBox).UpdateColors());
    public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(nameof(IsChecked), typeof(bool), typeof(CheckBox), false, BindingMode.TwoWay, propertyChanged: (bo, ov, nv) => (bo as CheckBox).ApplyIsCheckedAction(bo as CheckBox, (bool)nv));
    public static readonly BindableProperty IsDisabledProperty = BindableProperty.Create(nameof(IsDisabled), typeof(bool), typeof(CheckBox), false, propertyChanged: (bo, ov, nv) => (bo as CheckBox).IsDisabled = (bool)nv);
    public static readonly BindableProperty KeyProperty = BindableProperty.Create(nameof(Key), typeof(int), typeof(CheckBox), 0, propertyChanged: (bo, ov, nv) => (bo as CheckBox).Key = (int)nv);
    public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(CheckBox), "", propertyChanged: (bo, ov, nv) => (bo as CheckBox).Text = (string)nv);
    public static readonly BindableProperty CheckChangedCommandProperty = BindableProperty.Create(nameof(CheckChangedCommand), typeof(ICommand), typeof(CheckBox), null, propertyChanged: (bo, ov, nv) => (bo as CheckBox).CheckChangedCommand = (ICommand)nv);
    public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(CheckBox), null);
    public static readonly BindableProperty BoxBackgroundColorProperty = BindableProperty.Create(nameof(BoxBackgroundColor), typeof(Color), typeof(CheckBox), GlobalSetting.BackgroundColor, propertyChanged: (bo, ov, nv) => (bo as CheckBox).UpdateBoxBackground());
    public static readonly BindableProperty TextFontSizeProperty = BindableProperty.Create(nameof(TextFontSize), typeof(double), typeof(CheckBox), GlobalSetting.FontSize, propertyChanged: (bo, ov, nv) => (bo as CheckBox).TextFontSize = (double)nv);
    public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(CheckBox), GlobalSetting.BorderColor, propertyChanged: (bo, ov, nv) => (bo as CheckBox).UpdateBorderColor());
    public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(CheckBox), Label.FontFamilyProperty.DefaultValue, propertyChanged: (bo, ov, nv) => (bo as CheckBox).UpdateFontFamily(nv?.ToString()));
    public static readonly BindableProperty IconGeometryProperty = BindableProperty.Create(nameof(IconGeometry), typeof(Geometry), typeof(CheckBox), defaultValue: PredefinedShapes.Check, propertyChanged: (bo, ov, nv) => (bo as CheckBox).UpdateShape());
    public static readonly BindableProperty IsPressedProperty = BindableProperty.Create(nameof(IsPressed), typeof(bool), typeof(CheckBox), propertyChanged: (bo, ov, nv) => (bo as CheckBox).ApplyIsPressedAction(bo as CheckBox, (bool)nv));
    public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(nameof(CornerRadius), typeof(float), typeof(CheckBox), GlobalSetting.CornerRadius, propertyChanged: (bo, ov, nv) => (bo as CheckBox).outlineBox.RadiusX = (float)nv);
    public static readonly BindableProperty LabelPositionProperty = BindableProperty.Create(
        propertyName: nameof(LabelPosition), declaringType: typeof(CheckBox),
        returnType: typeof(LabelPosition), defaultBindingMode: BindingMode.TwoWay,
        defaultValue: GlobalSetting.LabelPosition,
        propertyChanged: (bo, ov, nv) => (bo as CheckBox).ApplyLabelPosition((LabelPosition)nv));
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    #endregion

    #region Methods
    void ApplyLabelPosition(LabelPosition position)
    {
        Children.Clear();
        if (position == LabelPosition.After)
        {
            lblOption.HorizontalOptions = LayoutOptions.Start;
            Children.Add(IconLayout);
            Children.Add(lblOption);
        }
        else
        {
            lblOption.HorizontalOptions = LayoutOptions.FillAndExpand;
            Children.Add(lblOption);
            Children.Add(IconLayout);
        }
    }

    void ExecuteCommand()
    {
        if (CheckChangedCommand?.CanExecute(CommandParameter ?? this) ?? true)
            CheckChangedCommand?.Execute(CommandParameter ?? this);
    }

    void UpdateBoxBackground()
    {
        if (Type == CheckType.Material)
            return;

        outlineBox.Fill = BoxBackgroundColor;
    }

    void UpdateColors()
    {
        //selectedIcon.Fill = Color;

        switch (Type)
        {
            case CheckType.Regular:
                outlineBox.Stroke = IsChecked ? Color : BorderColor;
                outlineBox.Fill = BoxBackgroundColor;
                selectedIcon.Fill = IconColor == Colors.Transparent ? Color : IconColor;
                break;
            case CheckType.Filled:
                outlineBox.Fill = IsChecked ? BorderColor : Colors.Transparent;
                selectedIcon.Fill = IsChecked ? Color : Colors.Transparent;
                break;
            case CheckType.Material:
                outlineBox.Stroke = Color;
                outlineBox.Fill = IsChecked ? Color : Colors.Transparent;
                selectedIcon.Fill = Color.ToSurfaceColor();
                break;
            default:
                outlineBox.Stroke = IsChecked ? Color : BorderColor;
                outlineBox.Fill = BoxBackgroundColor;
                selectedIcon.Fill = IconColor == Colors.Transparent ? Color : IconColor;
                break;
        }
    }

    void UpdateBorderColor()
    {
        if (Type == CheckType.Material)
            return;

        outlineBox.Stroke = BorderColor;
    }

    void SetBoxSize(double size)
    {
        outlineBox.HeightRequest = size;
        outlineBox.WidthRequest = size;
        //selectedIcon.MaximumHeightRequest = size * CHECK_SIZE_RATIO;
        //selectedIcon.MaximumWidthRequest = size * CHECK_SIZE_RATIO;
    }

    void UpdateShape()
    {
        selectedIcon.Data = IconGeometry;
    }

    void UpdateType()
    {
        UpdateColors();
    }

    void UpdateFontFamily(string value)
    {
        lblOption.FontFamily = value;
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
    /// <summary>
    /// Not available for this control
    /// </summary>
    public void DisplayValidation()
    {

    }

    public static void ApplyIsChecked(CheckBox checkBox, bool isChecked)
    {
        checkBox.selectedIcon.ScaleTo(isChecked ? CHECK_SIZE_RATIO : 0, 160);

        checkBox.UpdateColors();
    }

    public static async void ApplyIsPressed(CheckBox checkBox, bool isPressed)
    {
        await checkBox.outlineBox.ScaleTo(isPressed ? .8 : 1, 50, Easing.BounceIn);
        var radiusVal = isPressed ? checkBox.outlineBox.RadiusX * 2f : checkBox.CornerRadius;
        checkBox.outlineBox.RadiusX = radiusVal;
    }
    #endregion

    public enum CheckType
    {
        [Obsolete("This option is removed. Use IconGeometry instead.")]
        Box,
        [Obsolete("This option is removed. Use IconGeometry instead")]
        Check,
        [Obsolete("This option is removed. Use IconGeometry instead")]
        Cross,
        [Obsolete("This option is removed. Use IconGeometry instead")]
        Star,
        [Obsolete("This option is removed. Use IconGeometry instead")]
        Custom = 90,
        Regular,
        Filled,
        Material,
    }
}