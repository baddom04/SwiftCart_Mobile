using System.Text.Json.Serialization;

namespace ShoppingList.Persistor.DTO
{
    public class PaginatedResponse<T>
    {
        [JsonPropertyName("data")]
        public required IEnumerable<T> QueryResult { get; init; }

        [JsonPropertyName("meta")]
        public MetaData Meta { get; init; } = null!;

        [JsonPropertyName("last_page")]
        public int MaxPages { get; init; }
    }
}
