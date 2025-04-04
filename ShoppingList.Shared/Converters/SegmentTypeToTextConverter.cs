using Avalonia.Data.Converters;
using ShoppingList.Core.Enums;
using ShoppingList.Shared.Utils;
using System.Globalization;

namespace ShoppingList.Shared.Converters
{
    public class SegmentTypeToTextConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null) return null;
            if (value is not SegmentType type) throw new ArgumentException(null, nameof(value));

            return StringProvider.GetString(type.ToString());
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
