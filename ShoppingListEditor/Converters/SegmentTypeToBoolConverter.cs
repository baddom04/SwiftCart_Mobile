using Avalonia.Data.Converters;
using ShoppingList.Core.Enums;
using System;
using System.Globalization;

namespace ShoppingListEditor.Converters
{
    internal class SegmentTypeToBoolConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if(value is null) return null;
            if (value is not SegmentType type || parameter is not SegmentType other) throw new ArgumentException(null, nameof(value));

            return type == other;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
