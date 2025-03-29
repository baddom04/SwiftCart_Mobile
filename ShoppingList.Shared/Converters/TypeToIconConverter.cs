using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Media;
using ShoppingList.Shared.Utils;
using System.Globalization;

namespace ShoppingList.Shared.Converters
{
    public class TypeToIconConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not NotificationType type) throw new ArgumentException(null, nameof(value));

            switch (type)
            {
                case NotificationType.Error:
                    Application.Current!.TryFindResource("error_circle_regular", out var error_res);
                    return error_res as StreamGeometry ?? throw new KeyNotFoundException();
                case NotificationType.Warning:
                    Application.Current!.TryFindResource("warning_regular", out var warning_res);
                    return warning_res as StreamGeometry ?? throw new KeyNotFoundException();
                case NotificationType.Information:
                    Application.Current!.TryFindResource("info_regular", out var info_res);
                    return info_res as StreamGeometry ?? throw new KeyNotFoundException();
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
