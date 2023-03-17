namespace InputKit.Shared.Validations;

public class DigitsOnlyValidation : BindableObject, IValidation
{
    public string Message
    {
        get => (string)GetValue(MessageProperty);
        set => SetValue(MessageProperty, value);
    }

    public static readonly BindableProperty MessageProperty = BindableProperty.Create(
        nameof(Message),
        typeof(string),
        typeof(DigitsOnlyValidation),
        "The field should contain only digits.");

    public bool Validate(object value)
    {
        if (value is string text)
        {
            return text.All(char.IsDigit);
        }

        return false;
    }
}
