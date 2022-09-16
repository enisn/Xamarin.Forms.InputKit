using System.ComponentModel;

namespace InputKit.Shared.Validations;
public class MinValueValidation : IValidation
{
    private string message;

    public string Message { get => message ?? $"The field can't be less than {MinValue}."; set => message = value; }

    [TypeConverter(typeof(ComparableTypeConverter))]
    public IComparable MinValue { get; set; }

    public bool Validate(object value)
    {
        if (value is null)
        {
            return true;
        }

        var converted = ComparableTypeConverter.Instance.ConvertFrom(value);

        if (converted is IComparable comparable)
        {
            return comparable.CompareTo(MinValue) >= 0;
        }

        return false;
    }
}
