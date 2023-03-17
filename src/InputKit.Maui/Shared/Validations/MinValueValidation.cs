using System.ComponentModel;

namespace InputKit.Shared.Validations;
public class MinValueValidation : BindableObject, IValidation
{
    public string Message
    {
        get => string.Format((string)GetValue(MessageProperty), MinValue);
        set => SetValue(MessageProperty, value);
    }

    [TypeConverter(typeof(ComparableTypeConverter))]
    public IComparable MinValue
    {
        get => (IComparable)GetValue(MinValueProperty);
        set => SetValue(MinValueProperty, value);
    }

    public static readonly BindableProperty MessageProperty = BindableProperty.Create(
        nameof(Message),
        typeof(string),
        typeof(MinValueValidation),
        "The field can't be less than {0}.");

    public static readonly BindableProperty MinValueProperty = BindableProperty.Create(
        nameof(MinValue),
        typeof(IComparable),
        typeof(MinValueValidation),
        null);

    public bool Validate(object value)
    {
        if (value is null || (value is string text && string.IsNullOrEmpty(text)))
        {
            return true;
        }

        var type = MinValue.GetType();

        if (value.GetType() != type)
        {
            value = Convert.ChangeType(value, type);
        }

        if (value is IComparable comparableValue)
        {
            return comparableValue.CompareTo(MinValue) >= 0;
        }

        return false;
    }
}