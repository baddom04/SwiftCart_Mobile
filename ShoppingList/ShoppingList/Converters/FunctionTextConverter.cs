using Avalonia.Data.Converters;
using ShoppingList.Utils;
using System;
using System.Globalization;

namespace ShoppingList.Converters
{
    internal class FunctionTextConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            (bool condition, string appendage) = value switch
            {
                bool isUpdating => (isUpdating, "Item"),
                int householdId => (householdId != 0, "Household"),
                _ => throw new ArgumentException(null, nameof(value))
            };

            string res = StringProvider.GetString(condition ? $"Update{appendage}" : $"Create{appendage}");

            return parameter is string param && param == "Btn" ? res.Split(' ')[0] : res;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
