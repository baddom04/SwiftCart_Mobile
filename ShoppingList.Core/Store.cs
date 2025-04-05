using ShoppingList.Core.JsonConverters;
using System.Text.Json.Serialization;

namespace ShoppingList.Core
{
    public class Store
    {
        public int Id { get; init; }
        public required string Name { get; init; }

        [JsonConverter(typeof(BoolFromIntegerConverter))]
        public bool Published { get; init; }

        [JsonPropertyName("user_id")]
        public int UserId { get; init; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; init; }

        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; init; }
        public Location? Location { get; init; }
        public Map? Map { get; init; }
    }
}
