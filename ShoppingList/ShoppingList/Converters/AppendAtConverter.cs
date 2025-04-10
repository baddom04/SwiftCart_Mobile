﻿using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace ShoppingList.Converters
{
    internal class AppendAtConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null) return null;
            if (value is not string str) throw new ArgumentException(null, nameof(value));

            return '@' + str;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
