using Avalonia.Data.Converters;
using Avalonia.Media;
using ShoppingList.Core.Enums;
using System;
using System.Globalization;

namespace ShoppingList.Converters
{
    internal class SegmentTypeToColorConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if(value is not SegmentType type) throw new ArgumentException(null, nameof(value));

            return type switch
            {
                SegmentType.Entrance => Brushes.LightGreen,
                SegmentType.CashRegister => Brushes.LightSalmon,
                SegmentType.Shelf => Brushes.SaddleBrown,
                SegmentType.Fridge => Brushes.LightBlue,
                SegmentType.Wall => Brushes.DarkGray,
                SegmentType.Outside => Brushes.White,
                SegmentType.Empty => Brushes.White,
                _ => Brushes.White,
            };
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
