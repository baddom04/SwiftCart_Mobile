using ShoppingList.Core.Enums;
using System.Text.Json.Serialization;

namespace ShoppingList.Core
{
    public class MapSegment
    {
        public int Id { get; init; }
        public int X { get; init; }
        public int Y { get; init; }

        [JsonPropertyName("map_id")]
        public int MapId { get; init; }

        [JsonPropertyName("section_id")]
        public int? SectionId { get; init; }
        public SegmentType Type { get; init; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; init; }

        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; init; }
        public required IEnumerable<Product> Products { get; init; }
    }
}
