namespace InputKit.Shared.Validations;
public class MinLengthValidation : BindableObject, IValidation
{
    public string Message
    {
        get => string.Format((string)GetValue(MessageProperty), MinLength);
        set => SetValue(MessageProperty, value);
    }

    public int MinLength
    {
        get => (int)GetValue(MinLengthProperty);
        set => SetValue(MinLengthProperty, value);
    }

    public static readonly BindableProperty MessageProperty = BindableProperty.Create(
        nameof(Message),
        typeof(string),
        typeof(MinLengthValidation),
        "The field should contain at least {0} character.");

    public static readonly BindableProperty MinLengthProperty = BindableProperty.Create(
        nameof(MinLength),
        typeof(int),
        typeof(MinLengthValidation),
        0);

    public bool Validate(object value)
    {
        if (value is string text)
        {
            return text.Length >= MinLength;
        }

        return true;
    }
}
