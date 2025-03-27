using Avalonia.Data.Converters;
using ShoppingList.Core.Enums;
using ShoppingList.Utils;
using System;
using System.Globalization;

namespace ShoppingList.Converters
{
    internal class SegmentTypeToTextConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not SegmentType type) throw new ArgumentException(null, nameof(value));

            return StringProvider.GetString(type.ToString());
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
