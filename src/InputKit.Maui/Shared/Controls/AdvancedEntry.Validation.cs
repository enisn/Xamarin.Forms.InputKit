using InputKit.Shared.Abstraction;
using InputKit.Shared.Validations;
using Path = Microsoft.Maui.Controls.Shapes.Path;

namespace InputKit.Shared.Controls;
public partial class AdvancedEntry : IValidatable
{
    protected Lazy<Label> labelValidation;

    protected Lazy<ContentView> iconValidation;

    protected virtual void InitializeValidation()
    {
        txtInput.TextChanged += (s, e) =>
        {
            TextChanged?.Invoke(this, e);

            CheckAndDisplayValidations();
        };

        labelValidation = new Lazy<Label>(() => new Label
        {
            TextColor = ValidationColor,
            FontFamily = FontFamily,
            HorizontalOptions = LayoutOptions.Start,
        });

        iconValidation = new Lazy<ContentView>(() => new ContentView
        {
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.End,
            WidthRequest = 25,
            HeightRequest = 25,
            Content = new Path
            {
                Fill = ValidationColor,
                Data = PredefinedShapes.ExclamationCircle
            }
        });
    }

    public List<IValidation> Validations { get; } = new();

    public bool IsValid => ValidationResults().All(x => x.isValid);

    public LabelPosition ValidationPosition
    {
        get => (LabelPosition)GetValue(ValidationPositionProperty);
        set => SetValue(ValidationPositionProperty, value);
    }

    public static readonly BindableProperty ValidationPositionProperty = BindableProperty.Create(
                               propertyName: nameof(ValidationPosition), declaringType: typeof(AdvancedEntry),
                               returnType: typeof(LabelPosition), defaultBindingMode: BindingMode.TwoWay,
                               defaultValue: GlobalSetting.LabelPosition,
                               propertyChanged: (bo, ov, nv) => (bo as AdvancedEntry).ApplyValidationPosition());

    public Color ValidationColor
    {
        get => (Color)GetValue(ValidationColorProperty);
        set => SetValue(ValidationColorProperty, value);
    }

    public static readonly BindableProperty ValidationColorProperty = BindableProperty.Create(
        nameof(ValidationColor),
        typeof(Color),
        typeof(AdvancedEntry),
        defaultValue: Colors.Red,
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            var advEntry = (bindable as AdvancedEntry);

            if (advEntry.iconValidation.IsValueCreated)
            {
                if (advEntry.iconValidation.Value.Content is Path path)
                {
                    path.Fill = (Color)newValue;
                }
            }

            if (advEntry.labelValidation.IsValueCreated)
            {
                advEntry.labelValidation.Value.TextColor = (Color)newValue;
            }
        });

    /// <summary>
    /// Triggers to display annotation message
    /// </summary>
    public void DisplayValidation()
    {
        CheckAndDisplayValidations();
    }

    private bool lastValidationState = true;

    protected virtual void CheckAndDisplayValidations()
    {
        var results = ValidationResults().ToArray();
        var isValidationPassed = results.All(a => a.isValid);

        var isStateChanged = isValidationPassed != lastValidationState;

        lastValidationState = isValidationPassed;

        if (isValidationPassed)
        {
            if (isStateChanged)
            {
                RemoveValidationWarning();
                OnPropertyChanged(nameof(IsValid));
            }
        }
        else
        {
            var message = string.Join(",\n", results.Where(x => !x.isValid).Select(s => s.message));
            labelValidation.Value.Text = message;

            if (isStateChanged)
            {
                ShowValidationWarning();
                OnPropertyChanged(nameof(IsValid));
            }
        }
    }

    protected IEnumerable<(bool isValid, string message)> ValidationResults()
    {
        foreach (var validation in Validations)
        {
            var validated = validation.Validate( txtInput.Text );
            yield return new(validated, validation.Message);
        }
    }

    protected virtual void ShowValidationWarning()
    {
        inputGrid.Add(iconValidation.Value, column: 1);
        ApplyValidationPosition();
    }

    protected virtual void RemoveValidationWarning()
    {
        inputGrid.Remove(iconValidation.Value);
        Children.Remove(labelValidation.Value);
    }

    private void ApplyValidationPosition()
    {
        Children.Remove(labelValidation.Value);
        switch (ValidationPosition)
        {
            case LabelPosition.Before:
                Children.Insert(1, labelValidation.Value);
                break;
            case LabelPosition.After:
                Children.Add(labelValidation.Value);
                break;
        }
    }
}