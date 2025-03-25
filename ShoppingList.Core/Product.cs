using System.Text.Json.Serialization;

namespace ShoppingList.Core
{
    public class Product
    {
        public int Id { get; init; }
        public required string Name { get; init; }
        public required string Brand { get; init; }
        public required string Description { get; init; }

        [JsonPropertyName("map_segment_id")]
        public int MapSegmentId { get; init; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; init; }

        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; init; }
    }
}
