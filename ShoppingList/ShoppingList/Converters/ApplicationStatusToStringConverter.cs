using Avalonia.Data.Converters;
using ShoppingList.Utils;
using System;
using System.Globalization;

namespace ShoppingList.Converters
{
    internal class ApplicationStatusToStringConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if(value == null) return null;
            if (value is not UserStatus status) throw new ArgumentException(null, nameof(value));

            return StringProvider.GetString(status.ToString());
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
