using InputKit.Shared.Abstraction;
using InputKit.Shared.Configuration;
using InputKit.Shared.Helpers;
using InputKit.Shared.Layouts;
using Microsoft.Maui.Controls.Shapes;
using System.ComponentModel;
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
        BorderColor = Colors.Black,
        TextColor = (Color)Label.TextColorProperty.DefaultValue,
        Size = 25,
        CornerRadius = 4,
        FontSize = 14,
        LabelPosition = LabelPosition.After
    };
    protected static PathGeometryConverter PathGeometryConverter { get; } = new PathGeometryConverter();

    #region Constants
    public const string PATH_CHECK = "M 6.5212 16.4777 l -6.24 -6.24 c -0.3749 -0.3749 -0.3749 -0.9827 0 -1.3577 l 1.3576 -1.3577 c 0.3749 -0.3749 0.9828 -0.3749 1.3577 0 L 7.2 11.7259 L 16.2036 2.7224 c 0.3749 -0.3749 0.9828 -0.3749 1.3577 0 l 1.3576 1.3577 c 0.3749 0.3749 0.3749 0.9827 0 1.3577 l -11.04 11.04 c -0.3749 0.3749 -0.9828 0.3749 -1.3577 -0 z";
    public const string PATH_SQUARE = "M12 12H0V0h12v12z";
    public const string PATH_LINE = "M 17.2026 6.7911 H 0.9875 C 0.4422 6.7911 0 7.2332 0 7.7784 v 2.6331 c 0 0.5453 0.442 0.9873 0.9875 0.9873 h 16.2151 c 0.5453 0 0.9873 -0.442 0.9873 -0.9873 v -2.6331 C 18.1901 7.2332 17.7481 6.7911 17.2026 6.7911 z";
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
    internal Label lblOption = new Label { VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Start, FontSize = GlobalSetting.FontSize, TextColor = GlobalSetting.TextColor, FontFamily = GlobalSetting.FontFamily, IsVisible = false };
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
    public CheckType Type { get => _type; set { _type = value; UpdateType(value); } }

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
    public Geometry CustomIconGeometry { get => (Geometry)GetValue(CustomIconGeometryProperty); set => SetValue(CustomIconGeometryProperty, value); }

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
    public static readonly BindableProperty CustomIconGeometryProperty = BindableProperty.Create(nameof(CustomIconGeometry), typeof(Geometry), typeof(CheckBox), defaultValue: GetGeometryFromString(PATH_CHECK), propertyChanged: (bo, ov, nv) => (bo as CheckBox).UpdateType(CheckType.Custom));
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

        if (Type == CheckType.Material)
        {
            outlineBox.Stroke = Color;
            outlineBox.Fill = IsChecked ? Color : Colors.Transparent;
            selectedIcon.Fill = Color.ToSurfaceColor();
        }
        else
        {
            outlineBox.Stroke = IsChecked ? Color : BorderColor;
            outlineBox.Fill = BoxBackgroundColor;
            selectedIcon.Fill = IconColor == Colors.Transparent ? Color : IconColor;
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

    void UpdateType(CheckType _Type = CheckType.Custom)
    {
        switch (_Type)
        {
            case CheckType.Box:
                selectedIcon.Data = GetGeometryFromString(PATH_SQUARE);
                break;
            case CheckType.Line:
                selectedIcon.Data = GetGeometryFromString(PATH_LINE);
                break;

            case CheckType.Check:
            case CheckType.Star:
            case CheckType.Cross:
                selectedIcon.Data = GetGeometryFromString(PATH_CHECK);
                break;
            case CheckType.Material:
                outlineBox.RadiusX = 5;
                selectedIcon.Data = GetGeometryFromString(PATH_CHECK);
                break;
            case CheckType.Custom:
                selectedIcon.Data = CustomIconGeometry;
                break;
        }

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

    internal static Geometry GetGeometryFromString(string path)
    {
        return (Geometry)PathGeometryConverter.ConvertFromInvariantString(path);
    }
    #endregion

    public enum CheckType
    {
        Box,
        Check,
        [Obsolete("This option is removed. Use another one.")]
        Cross,
        [Obsolete("This option is removed. Use another one.")]
        Star,
        Material,
        Line,
        Custom = 90
    }
}