using Avalonia.Data.Converters;
using ShoppingList.Core.Enums;
using ShoppingList.Utils;
using System;
using System.Globalization;

namespace ShoppingList.Converters
{
    internal class HouseholdRelationshipToStringConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not HouseholdRelationship relationship)
                throw new ArgumentException(null, nameof(value));

            return StringProvider.GetString(relationship.ToString());
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
