namespace InputKit.Shared.Validations;
public class RequiredValidation : BindableObject, IValidation
{
    public string Message
    {
        get => (string)GetValue(MessageProperty);
        set => SetValue(MessageProperty, value);
    }

    public static readonly BindableProperty MessageProperty = BindableProperty.Create(
        nameof(Message),
        typeof(string),
        typeof(RequiredValidation),
        "This field is required");

    public bool Validate(object value)
    {
        if (value is string text)
        {
            return !string.IsNullOrEmpty(text);
        }

        return value != null;
    }
}
