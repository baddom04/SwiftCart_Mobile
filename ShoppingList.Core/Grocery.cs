using ShoppingList.Core.Enums;
using System.Text.Json.Serialization;

namespace ShoppingList.Core
{
    public class Grocery
    {
        public int Id { get; init; }
        public required string Name { get; init; }
        public int? Quantity { get; init; }
        public UnitType? Unit { get; init; }
        public string? Description { get; init; }

        [JsonPropertyName("household_id")]
        public int HouseholdId { get; init; }

        [JsonPropertyName("user_id")]
        public int UserId { get; init; }

        [JsonPropertyName("comment_count")]
        public int CommentCount { get; init; }

        public required User User { get; init; }
    }
}
