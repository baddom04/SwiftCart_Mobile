using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Media;
using ShoppingList.Core.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace ShoppingList.Converters
{
    internal class RoleToColorConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not UserRole role) throw new ArgumentException(null, nameof(value));

            string key = role == UserRole.Admin ? "Gold" : "MainBtnBG";

            Application.Current!.TryFindResource(key, out var resource);

            return resource as IBrush ?? throw new KeyNotFoundException();
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
