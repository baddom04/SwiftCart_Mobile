using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Media;
using ShoppingList.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace ShoppingList.Converters
{
    internal class TypeToForegroundConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not NotificationType type) throw new ArgumentException(null, nameof(value));

            switch (type)
            {
                case NotificationType.Error:
                    Application.Current!.TryFindResource("ErrorSecondaryRed", out var error_res);
                    return error_res as SolidColorBrush ?? throw new KeyNotFoundException();
                case NotificationType.Warning:
                    Application.Current!.TryFindResource("WarningYellow", out var warning_res);
                    return warning_res as SolidColorBrush ?? throw new KeyNotFoundException();
                case NotificationType.Information:
                    Application.Current!.TryFindResource("InfoBlue", out var info_res);
                    return info_res as SolidColorBrush ?? throw new KeyNotFoundException();
                default:
                    throw new NotImplementedException();
            }
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
