using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace ShoppingList.Shared.Converters
{
    public class AppendFtConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if(value == null) return null;
            return value.ToString() + " Ft";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
