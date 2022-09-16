using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InputKit.Shared.Validations;
public class DigitsOnlyValidation : IValidation
{
    public string Message { get; set; } = "The field should contain only digits.";

    public bool Validate(object value)
    {
        if (value is string text)
        {
            return text.All(char.IsDigit);
        }

        return false;
    }
}
