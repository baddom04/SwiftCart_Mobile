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
            if(value is not int householdId) throw new ArgumentException(null, nameof(value));

            string res = StringProvider.GetString(householdId != 0 ? "UpdateHousehold" : "NewHousehold");

            return parameter is string param && param == "Btn" ? res.Split(' ')[0] : res;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
