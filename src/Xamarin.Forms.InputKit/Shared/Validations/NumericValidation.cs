using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.InputKit.Shared.Validations
{
    public class NumericValidation : IValidation
    {
        public string Message { get; }

        public bool Validate(object value)
        {
            if (value is string text)
            {
                return double.TryParse(text, out _);
            }

            return false;
        }
    }
}