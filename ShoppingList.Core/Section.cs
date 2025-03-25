using System.Text.Json.Serialization;

namespace ShoppingList.Core
{
    public class Section
    {
        public int Id { get; init; }
        public required string Name { get; init; }

        [JsonPropertyName("map_id")]
        public int MapId { get; init; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; init; }

        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; init; }
    }
}
