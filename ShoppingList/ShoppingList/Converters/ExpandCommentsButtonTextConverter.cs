using Avalonia.Data.Converters;
using ShoppingList.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace ShoppingList.Converters
{
    internal class ExpandCommentsButtonTextConverter : IMultiValueConverter
    {
        public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
        {
            if (values.Count < 2)
                return string.Empty;

            if (values[0] is bool seeComments && values[1] is int commentCount)
            {
                string result = seeComments
                ? StringProvider.GetString("HideComments")
                : $"{StringProvider.GetString("OpenCommentsStart")} {commentCount} {StringProvider.GetString("OpenCommentsEnd")}";

                return result;
            }

            return "Invalid data";
        }
    }
}
