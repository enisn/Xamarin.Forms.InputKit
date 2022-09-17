using System;
using Xamarin.Forms;

namespace Plugin.InputKit.Shared.Validations
{
    public class MaxValueValidation : IValidation
    {
        private string message;
        public string Message { get => message ?? $"The field can't be greater than {MaxValue}."; set => message = value; }

        [TypeConverter(typeof(ComparableTypeConverter))]
        public IComparable MaxValue { get; set; }

        public bool Validate(object value)
        {
            if (value is null)
            {
                return true;
            }

            var converted = ComparableTypeConverter.Instance.ConvertFrom(value);
            if (converted is IComparable comparable && converted.GetType() == comparable.GetType())
            {
                return comparable.CompareTo(MaxValue) <= 0;
            }

            return false;
        }
    }
}