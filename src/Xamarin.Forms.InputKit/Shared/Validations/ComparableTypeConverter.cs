using System;
using System.ComponentModel;
using System.Globalization;

namespace Plugin.InputKit.Shared.Validations
{
    public class ComparableTypeConverter : TypeConverter
    {
        private static ComparableTypeConverter instance;

        public static ComparableTypeConverter Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ComparableTypeConverter();
                }

                return instance;
            }
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType is IComparable)
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType is IComparable)
            {
                return true;
            }
            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {

            return value as IComparable;
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string text)
            {
                if (double.TryParse(text, out var number))
                {
                    return number;
                }

                if (DateTime.TryParse(text, out var dateTime))
                {
                    return dateTime;
                }

                if (TimeSpan.TryParse(text, out var timeSpan))
                {
                    return timeSpan;
                }

                if (string.IsNullOrEmpty(text))
                {
                    return null;
                }
            }

            return value as IComparable;
        }
    }
}