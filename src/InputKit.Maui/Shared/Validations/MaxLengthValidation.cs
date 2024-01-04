namespace InputKit.Shared.Validations;
public class MaxLengthValidation : BindableObject, IValidation
{
    public string Message
    {
        get => string.Format((string)GetValue(MessageProperty), MaxLength);
        set => SetValue(MessageProperty, value);
    }

    public int MaxLength
    {
        get => (int)GetValue(MaxLengthProperty);
        set => SetValue(MaxLengthProperty, value);
    }

    public static readonly BindableProperty MessageProperty = BindableProperty.Create(
        nameof(Message),
        typeof(string),
        typeof(MaxLengthValidation),
        "The field should contain maximum {0} character.");

    public static readonly BindableProperty MaxLengthProperty = BindableProperty.Create(
        nameof(MaxLength),
        typeof(int),
        typeof(MaxLengthValidation),
        0);

    public bool Validate(object value)
    {
        if (value is string text)
        {
            return text.Length <= MaxLength;
        }

        return true;
    }
}
