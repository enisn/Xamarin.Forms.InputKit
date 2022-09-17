using System;
using System.ComponentModel;

namespace Plugin.InputKit.Shared.Validations
{
    public class MinValueValidation : IValidation
    {
        private string message;

        public string Message { get => message ?? $"The field can't be less than {MinValue}."; set => message = value; }

        [TypeConverter(typeof(ComparableTypeConverter))]
        public IComparable MinValue { get; set; }

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
}