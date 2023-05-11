using InputKit.Shared.Abstraction;
using InputKit.Shared.Configuration;
using InputKit.Shared.Helpers;
using InputKit.Shared.Layouts;
using InputKit.Shared.Validations;
using Microsoft.Maui;
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
        LabelPosition = LabelPosition.After,
        LineBreakMode = LineBreakMode.WordWrap
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
        LineBreakMode = GlobalSetting.LineBreakMode,
        VerticalOptions = LayoutOptions.Center,
        HorizontalOptions = LayoutOptions.Start,
        FontSize = GlobalSetting.FontSize,
        TextColor = GlobalSetting.TextColor,
        FontFamily = GlobalSetting.FontFamily,
        IsVisible = false
    };

    protected Lazy<Path> iconValidation;

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
        ApplyIsCheckedAction = ApplyIsChecked;
        ApplyIsPressedAction = ApplyIsPressed;

        IconLayout = new Grid
        {
            MinimumWidthRequest = GlobalSetting.Size,
            HeightRequest = GlobalSetting.Size,
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Center,
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
            Command = new Command(() => { if (IsDisabled) return; IsChecked = !IsChecked; }),
        });

        iconValidation = new Lazy<Path>(() => new Path
        {
            WidthRequest = 25,
            HeightRequest = 25,
            Fill = ValidationColor,
            Data = PredefinedShapes.ExclamationCircle
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
    public CheckType Type { get => (CheckType)GetValue(TypeProperty); set => SetValue(TypeProperty, value); }

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
    /// Fontfamily of CheckBox Text
    /// </summary>
    public string FontFamily { get => (string)GetValue(FontFamilyProperty); set => SetValue(FontFamilyProperty, value); }

    [Obsolete("This option is removed. Use CustomIconGeometry", error: true)]
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

    public Color ValidationColor
    {
        get => (Color)GetValue(ValidationColorProperty);
        set => SetValue(ValidationColorProperty, value);
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

    #region Validation
    public List<IValidation> Validations { get; } = new();
    public bool IsValid => ValidationResults().All(x => x.isValid);
    protected IEnumerable<(bool isValid, string message)> ValidationResults()
    {
        foreach (var validation in Validations)
        {
            var validated = validation.Validate(this.IsChecked ? true : null);
            yield return new(validated, validation.Message);
        }
    }

    /// <summary>
    /// Not available for this control
    /// </summary>
    public virtual void DisplayValidation()
    {
        if (IsValid)
        {
            this.Remove(iconValidation.Value);
        }
        else
        {
            if (this.Children.Contains(iconValidation.Value))
            {
                return;
            }

            this.Add(iconValidation.Value);
        }

        OnPropertyChanged(nameof(IsValid));
    }

    public virtual void ResetValidation()
    {
        this.Remove(iconValidation.Value);
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

    public static readonly BindableProperty ValidationColorProperty = BindableProperty.Create(
        nameof(ValidationColor),
        typeof(Color),
        typeof(CheckBox),
        defaultValue: Colors.Red,
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            var checkBox = (bindable as CheckBox);

            if (checkBox.iconValidation?.IsValueCreated ?? false)
            {
                checkBox.iconValidation.Value.Fill = (Color)newValue;
            }
        });

    public static readonly BindableProperty TypeProperty = BindableProperty.Create(nameof(Type), typeof(CheckType), typeof(CheckBox), defaultValue: CheckType.Regular,
        propertyChanged: (bindable, oldValue, newValue) => (bindable as CheckBox).UpdateType());

    public static readonly BindableProperty LineBreakModeProperty = BindableProperty.Create(nameof(LineBreakMode), typeof(LineBreakMode), typeof(CheckBox), defaultValue: GlobalSetting.LineBreakMode,
        propertyChanged: (bindable, oldValue, newValue) => (bindable as CheckBox).lblOption.LineBreakMode = (LineBreakMode)newValue);

    public static readonly BindableProperty IconVerticalOptionsProperty = BindableProperty.Create(nameof(IconVerticalOptions), typeof(LayoutOptions), typeof(CheckBox), defaultValue: LayoutOptions.Center,
        propertyChanged: (bindable, oldValue, newValue) => (bindable as CheckBox).IconLayout.VerticalOptions = (LayoutOptions)newValue);

    public static readonly BindableProperty IconHorizontalOptionsProperty = BindableProperty.Create(nameof(IconHorizontalOptions), typeof(LayoutOptions), typeof(CheckBox), defaultValue: LayoutOptions.Center,
        propertyChanged: ( bindable, oldValue, newValue ) => (bindable as CheckBox).IconLayout.HorizontalOptions = (LayoutOptions)newValue);

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

    protected override async void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

        // TODO: Remove this logic after resolution of https://github.com/dotnet/maui/issues/8873
        // This is a workaround.

#if ANDROID
        await Task.Delay(1);
#endif
        if (IconLayout.Width != -1 && lblOption.Width > this.Width)
        {
            lblOption.MaximumWidthRequest = this.Width - this.Spacing - IconLayout.Width;
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
    public static void ApplyIsChecked(CheckBox checkBox, bool isChecked)
    {
        checkBox.selectedIcon.ScaleTo(isChecked ? CHECK_SIZE_RATIO : 0, 160);

        checkBox.UpdateColors();

        checkBox.ExecuteCommand();

        checkBox.CheckChanged?.Invoke(checkBox, new EventArgs());

        if (checkBox.iconValidation.IsValueCreated && isChecked)
        {
            checkBox.DisplayValidation();
        }
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
