using Avalonia.Data.Converters;
using ShoppingList.Utils;
using System;
using System.Globalization;

namespace ShoppingList.Converters
{
    internal class ExpandCommentsButtonTextConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if(value is not bool seeComments) throw new ArgumentException(null, nameof(value));

            return StringProvider.GetString(seeComments ? "HideComments" : "OpenComments");
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
