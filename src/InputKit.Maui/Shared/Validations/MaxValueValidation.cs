using System;
using System.ComponentModel;

namespace InputKit.Shared.Validations;
public class MaxValueValidation : BindableObject, IValidation
{
    public string Message
    {
        get => string.Format((string)GetValue(MessageProperty), MaxValue);
        set => SetValue(MessageProperty, value);
    }

    [TypeConverter(typeof(ComparableTypeConverter))]
    public IComparable MaxValue
    {
        get => (IComparable) GetValue(MaxValueProperty);
        set => SetValue(MaxValueProperty, value);
    }

    public static readonly BindableProperty MessageProperty = BindableProperty.Create(
        nameof(Message),
        typeof(string),
        typeof(MaxValueValidation),
        "The field can't be greater than {0}.");

    public static readonly BindableProperty MaxValueProperty = BindableProperty.Create(
        nameof(MaxValue),
        typeof(IComparable),
        typeof(MaxValueValidation),
        null);

    public bool Validate(object value)
    {
        if (value is null || (value is string text && string.IsNullOrEmpty(text)))
        {
            return true;
        }

        var type = MaxValue.GetType();

        if (value.GetType() != type)
        {
            value = Convert.ChangeType(value, type);
        }

        if (value is IComparable comparableValue)
        {
            return comparableValue.CompareTo(MaxValue) <= 0;
        }

        return false;
    }
}
