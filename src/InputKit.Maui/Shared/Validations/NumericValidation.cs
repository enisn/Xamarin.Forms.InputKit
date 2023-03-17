using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InputKit.Shared.Validations;
public class NumericValidation : BindableObject, IValidation
{
    public string Message
    {
        get => (string)GetValue(MessageProperty);
        set => SetValue(MessageProperty, value);
    }

    public static readonly BindableProperty MessageProperty = BindableProperty.Create(
        nameof(Message),
        typeof(string),
        typeof(NumericValidation),
        "The field should contain only numeric values.");

    public bool Validate(object value)
    {
        if (value is string text)
        {
            return double.TryParse(text, out _);
        }

        return false;
    }
}
