using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace ShoppingList.Converters
{
    internal class RandomHouseIconConverter : IValueConverter
    {
        private readonly List<string> _buildingKeys = [
            "building_shop_regular",
            "building_bank_regular",
            "building_retail_regular",
            "building_government_regular",
            "building_multiple_regular",
            "building_skyscraper_regular",
            "building_regular"
            ];
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            string randomKey = _buildingKeys[new Random().Next(_buildingKeys.Count)];

            Application.Current!.TryFindResource(randomKey, out var resource);
            return resource as StreamGeometry ?? throw new KeyNotFoundException(randomKey);
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
