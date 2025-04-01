using System.Text.Json.Serialization;

namespace ShoppingList.Core
{
    public class Map
    {
        public int Id { get; init; }

        [JsonPropertyName("x_size")]
        public int XSize { get; init; }

        [JsonPropertyName("y_size")]
        public int YSize { get; init; }

        [JsonPropertyName("store_id")]
        public int StoreId { get; init; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; init; }

        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; init; }
        public IEnumerable<Section> Sections { get; init; } = [];

        [JsonPropertyName("map_segments")]
        public IEnumerable<MapSegment> MapSegments { get; init; } = [];
    }
}
