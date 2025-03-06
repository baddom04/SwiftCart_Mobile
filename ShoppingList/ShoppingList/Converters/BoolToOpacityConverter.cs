using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace ShoppingList.Converters
{
    internal class BoolToOpacityConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if(value is not bool isOwner) throw new ArgumentException(null, nameof(value));

            return isOwner ? 1d : 0d;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
