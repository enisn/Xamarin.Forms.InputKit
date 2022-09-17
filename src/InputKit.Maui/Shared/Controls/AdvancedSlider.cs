using InputKit.Shared.Abstraction;
using InputKit.Shared.Configuration;
using InputKit.Shared.Validations;

namespace InputKit.Shared.Controls;

public class AdvancedSlider : StackLayout, IValidatable
{
    public static GlobalSetting GlobalSetting { get; private set; } = new GlobalSetting
    {
        FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label)),
        TextColor = (Color)Label.TextColorProperty.DefaultValue,
        Color = InputKitOptions.GetAccentColor(),
    };

    Slider slider = new Slider { ThumbColor = GlobalSetting.Color, MinimumTrackColor = GlobalSetting.Color };
    Label lblTitle = new Label { FontSize = GlobalSetting.FontSize, FontFamily = GlobalSetting.FontFamily, Margin = new Thickness(20, 0), InputTransparent = true, FontAttributes = FontAttributes.Bold, TextColor = GlobalSetting.TextColor, };
    Label lblValue = new Label { FontSize = GlobalSetting.FontSize, FontFamily = GlobalSetting.FontFamily, InputTransparent = true, TextColor = GlobalSetting.TextColor, };
    Label lblMinValue = new Label { FontSize = GlobalSetting.FontSize, FontFamily = GlobalSetting.FontFamily, TextColor = GlobalSetting.TextColor, };
    Label lblMaxValue = new Label { FontSize = GlobalSetting.FontSize, FontFamily = GlobalSetting.FontFamily, TextColor = GlobalSetting.TextColor, };
    protected Label labelValidation = new Label { HorizontalOptions = LayoutOptions.Start, IsVisible = false };
    private Color _textColor;

    public AdvancedSlider()
    {
        Children.Add(lblTitle);
        Children.Add(new HorizontalStackLayout
        {
            Children =
                {
                    lblMinValue,
                    new Grid
                    {
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Margin = new Thickness(0,30,0,0),
                        Children =
                        {
                            slider,
                            lblValue,
                        }
                    },
                    lblMaxValue,
                }
        });
        labelValidation.TextColor = ValidationColor;
        this.Children.Add(labelValidation);

        slider.ValueChanged += Slider_ValueChanged;

        lblValue.SizeChanged += (s, e) =>
        {
            UpdateView();
        };
    }

    private void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        var mod = e.NewValue - (int)(e.NewValue / StepValue) * StepValue;
        if (mod != 0)
        {
            slider.Value = Math.Round(e.NewValue / StepValue) * StepValue;
            return;
        }

        SetValue(ValueProperty, slider.Value);
        UpdateValueText();
        UpdateView();
    }

    /// <summary>
    /// Value of slider which user selected
    /// </summary>
    public double Value { get => (double)GetValue(ValueProperty); set => SetValue(ValueProperty, value); }

    /// <summary>
    /// Title of slider, It'll be shown tp of slider
    /// </summary>
    public string Title { get => (string)GetValue(TitleProperty); set => SetValue(TitleProperty, value); }

    /// <summary>
    /// It will be displayed start of value 
    /// </summary>
    public string ValueSuffix { get => (string)GetValue(ValueSuffixProperty); set => SetValue(ValueSuffixProperty, value); }

    /// <summary>
    /// It'll be displayed end of value
    /// </summary>
    public string ValuePrefix { get => (string)GetValue(ValuePrefixProperty); set => SetValue(ValuePrefixProperty, value); }

    /// <summary>
    /// This will be displayed start of MinValue Text if <see cref="DisplayMinMaxValue"/> is true/>
    /// </summary>
    public string MinValuePrefix { get => (string)GetValue(MinValuePrefixProperty); set => SetValue(MinValuePrefixProperty, value); }

    /// <summary>
    /// This will be displayed start of MaxValue Text if <see cref="DisplayMinMaxValue"/> is true/>
    /// </summary>
    public string MaxValuePrefix { get => (string)GetValue(MaxValuePrefixProperty); set => SetValue(MaxValuePrefixProperty, value); }

    /// <summary>
    /// This will be displayed end of MinValue Text if <see cref="DisplayMinMaxValue"/> is true/>
    /// </summary>
    public string MinValueSuffix { get => (string)GetValue(MinValueSuffixProperty); set => SetValue(MinValueSuffixProperty, value); }

    /// <summary>
    /// This will be displayed end of MaxValue Text if <see cref="DisplayMinMaxValue"/> is"true/>
    /// </summary>
    public string MaxValueSuffix { get => (string)GetValue(MaxValueSuffixProperty); set => SetValue(MaxValueSuffixProperty, value); }

    /// <summary>
    /// Minimum value, user can slide
    /// </summary>
    public double MinValue { get => (double)GetValue(MinValueProperty); set => SetValue(MinValueProperty, value); }

    /// <summary>
    /// Maximum value, user can slide
    /// </summary>
    public double MaxValue { get => (double)GetValue(MaxValueProperty); set => SetValue(MaxValueProperty, value); }

    /// <summary>
    /// Slider Increase number
    /// </summary>
    public double StepValue { get => (double)GetValue(StepValueProperty); set => SetValue(StepValueProperty, value); }

    /// <summary>
    /// Visibility of Min value and Max value at right and left
    /// </summary>
    public bool DisplayMinMaxValue { get => (bool)GetValue(DisplayMinMaxValueProperty); set => SetValue(DisplayMinMaxValueProperty, value); }


    /// <summary>
    /// Text color of labels
    /// </summary>
    public Color TextColor
    {
        get => _textColor; set
        {
            _textColor = value;
            lblMaxValue.TextColor = value;
            lblMinValue.TextColor = value;
            lblTitle.TextColor = value;
            lblValue.TextColor = value;
        }
    }

    public Color ThumbColor { get => (Color)GetValue(ThumbColorProperty); set => SetValue(ThumbColorProperty, value); }

    #region BindableProperties
    public static readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(double), typeof(AdvancedSlider), 0.0, BindingMode.TwoWay, propertyChanged: (bo, ov, nv) => (bo as AdvancedSlider).slider.Value = (double)nv);
    public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(AdvancedSlider), Colors.Gray, propertyChanged: (bo, ov, nv) => (bo as AdvancedSlider).TextColor = (Color)nv);
    public static readonly BindableProperty StepValueProperty = BindableProperty.Create(nameof(StepValue), typeof(double), typeof(AdvancedSlider), 1d, propertyChanged: (bo, ov, nv) => (bo as AdvancedSlider).OnStepValueChanged((double)ov, (double)nv));
    public static readonly BindableProperty DisplayMinMaxValueProperty = BindableProperty.Create(nameof(StepValue), typeof(bool), typeof(AdvancedSlider), false, propertyChanged: (bo, ov, nv) => (bo as AdvancedSlider).OnDisplayMinMaxValueChanged((bool)nv));
    public static readonly BindableProperty MaxValueProperty = BindableProperty.Create(nameof(MaxValue), typeof(double), typeof(AdvancedSlider), 1d, propertyChanged: (bo, ov, nv) => (bo as AdvancedSlider).OnMaxValueChanged((double)nv));
    public static readonly BindableProperty MinValueProperty = BindableProperty.Create(nameof(MinValue), typeof(double), typeof(AdvancedSlider), 0d, propertyChanged: (bo, ov, nv) => (bo as AdvancedSlider).OnMinValueChanged((double)nv));
    public static readonly BindableProperty MaxValueSuffixProperty = BindableProperty.Create(nameof(MaxValueSuffix), typeof(string), typeof(AdvancedSlider), string.Empty, propertyChanged: (bo, ov, nv) => (bo as AdvancedSlider).UpdateMinMaxValueText());
    public static readonly BindableProperty MinValueSuffixProperty = BindableProperty.Create(nameof(MinValueSuffix), typeof(string), typeof(AdvancedSlider), string.Empty, propertyChanged: (bo, ov, nv) => (bo as AdvancedSlider).UpdateMinMaxValueText());
    public static readonly BindableProperty MaxValuePrefixProperty = BindableProperty.Create(nameof(MaxValuePrefix), typeof(string), typeof(AdvancedSlider), string.Empty, propertyChanged: (bo, ov, nv) => (bo as AdvancedSlider).UpdateMinMaxValueText());
    public static readonly BindableProperty MinValuePrefixProperty = BindableProperty.Create(nameof(MinValuePrefix), typeof(string), typeof(AdvancedSlider), string.Empty, propertyChanged: (bo, ov, nv) => (bo as AdvancedSlider).UpdateMinMaxValueText());
    public static readonly BindableProperty ValuePrefixProperty = BindableProperty.Create(nameof(ValuePrefix), typeof(string), typeof(AdvancedSlider), string.Empty, propertyChanged: (bo, ov, nv) => (bo as AdvancedSlider).UpdateValueText());
    public static readonly BindableProperty ValueSuffixProperty = BindableProperty.Create(nameof(ValueSuffix), typeof(string), typeof(AdvancedSlider), string.Empty, propertyChanged: (bo, ov, nv) => (bo as AdvancedSlider).UpdateValueText());
    public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(AdvancedSlider), string.Empty, propertyChanged: (bo, ov, nv) => (bo as AdvancedSlider).OnTitleChanged((string)nv));
    public static readonly BindableProperty ThumbColorProperty = BindableProperty.Create(nameof(ThumbColor), typeof(Color), typeof(AdvancedSlider), Slider.ThumbColorProperty.DefaultValue,
        propertyChanged: (bindable, oldValue, newValue) => (bindable as AdvancedSlider).slider.ThumbColor = (Color)newValue);
    #endregion

    /// <summary>
    /// doesn't affect
    /// </summary>
    public event EventHandler ValidationChanged;

    protected void UpdateMinMaxValueText()
    {
        lblMinValue.Text = $"{MinValuePrefix}{MinValue}{MinValueSuffix}";
        lblMaxValue.Text = $"{MaxValuePrefix}{MaxValue}{MaxValueSuffix}";
    }

    protected void UpdateValueText()
    {
        lblValue.Text = $"{ValuePrefix} {Value} {ValueSuffix}";
    }

    protected void UpdateView()
    {
        var totalLength = MaxValue - MinValue;
        var normalizedValue = Value - MinValue;

        // TODO: Keep animation disabled until resolution of https://github.com/dotnet/maui/issues/3353

        lblValue.TranslationX = normalizedValue * ((slider.Width - 30) / totalLength);
        lblValue.TranslationY = slider.TranslationY - lblValue.Height * 0.9;

        //lblValue.TranslateTo(
        //    normalizedValue * ((slider.Width - 30) / totalLength), //pos X  /* -30 is used to make smaller label horizontal movable region and prevent touching minValue and maxValue labels*/
        //    slider.TranslationY - lblValue.Height * 0.9, //pos Y
        //    40 //Latency
        //    );
    }

    protected virtual void OnStepValueChanged(double oldValue, double newValue)
    {
        UpdateValueText();
        UpdateView();
    }

    protected virtual void OnDisplayMinMaxValueChanged(bool newValue)
    {
        lblMaxValue.IsVisible = newValue;
        lblMinValue.IsVisible = newValue;
    }

    protected virtual void OnMaxValueChanged(double newValue)
    {
        slider.Maximum = newValue;
        UpdateMinMaxValueText();
    }
    protected virtual void OnMinValueChanged(double newValue)
    {
        slider.Minimum = newValue;
        UpdateMinMaxValueText();
    }

    protected virtual void OnTitleChanged(string newValue)
    {
        lblTitle.Text = newValue;
        lblTitle.IsVisible = !string.IsNullOrEmpty(newValue);
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);
        UpdateValueText();
        UpdateMinMaxValueText();
    }
    public List<IValidation> Validations { get; } = new List<IValidation>();
    public bool IsValid => ValidationResults().All(x => x.isValid);
    protected IEnumerable<(bool isValid, string message)> ValidationResults()
    {
        foreach (var validation in Validations)
        {
            var validated = validation.Validate(this.Value);
            yield return (validated, validation.Message);
        }
    }

    /// <summary>
    /// It's not available for this control
    /// </summary>
    public void DisplayValidation()
    {
        labelValidation.Text = string.Join("\n", ValidationResults().Where(x => !x.isValid).Select(s => s.message));
        labelValidation.IsVisible = !IsValid;
    }

    public Color ValidationColor
    {
        get => (Color)GetValue(ValidationColorProperty);
        set => SetValue(ValidationColorProperty, value);
    }

    public static readonly BindableProperty ValidationColorProperty = BindableProperty.Create(
      nameof(ValidationColor),
      typeof(Color),
      typeof(AdvancedSlider),
      defaultValue: Colors.Red,
      propertyChanged: (bindable, oldValue, newValue) =>
          (bindable as AdvancedSlider).labelValidation.TextColor = (Color)newValue
      );
}
