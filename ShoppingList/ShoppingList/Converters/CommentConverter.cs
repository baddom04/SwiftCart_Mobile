using Avalonia.Data.Converters;
using ShoppingList.Core;
using System;
using System.Globalization;

namespace ShoppingList.Converters
{
    internal class CommentConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not Comment comment) return value;

            return $"{comment.Content}";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
