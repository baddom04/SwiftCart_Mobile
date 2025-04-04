using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace ShoppingList.Converters
{
    internal class AppendColonConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null) return null;
            if (value is not string name) throw new ArgumentException(null, nameof(value));

            return name + ":";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
