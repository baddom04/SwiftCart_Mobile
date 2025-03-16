using System.Text.Json.Serialization;

namespace ShoppingList.Core.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum UnitType
    {
        none,
        pieces,
        pair,
        kilogram,
        pound,
        inch,
        ounce,
        liter,
        decagram,
        deciliter,
    }
}
