using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace ShoppingList.Core.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SegmentType
    {
        Shelf,
        Fridge,
        Empty,
        CashRegister,
        Entrance,
        Wall,
        Exit
    }
}
