using Avalonia.Data.Converters;
using ShoppingList.Core.Enums;
using System;
using System.Globalization;

namespace ShoppingList.Converters
{
    internal class IsNonMemberConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not HouseholdRelationship relationship)
                throw new ArgumentException(null, nameof(value));

            return relationship == HouseholdRelationship.NonMember;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
