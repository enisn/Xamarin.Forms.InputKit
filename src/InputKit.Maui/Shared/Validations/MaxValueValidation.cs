using System;
using System.ComponentModel;

namespace InputKit.Shared.Validations;
public class MaxValueValidation : IValidation
{
    private string message;
    public string Message { get => message ?? $"The field can't be greater than {MaxValue}."; set => message = value; }

    [TypeConverter(typeof(ComparableTypeConverter))]
    public IComparable MaxValue { get; set; }

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
