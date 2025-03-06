using ShoppingList.Core;
using System.Text.Json.Serialization;

namespace ShoppingList.Persistor.DTO
{
    public class HouseholdsResponse
    {
        [JsonPropertyName("data")]
        public required IEnumerable<Household> QueryResult { get; init; }

        [JsonPropertyName("meta")]
        public HouseholdsMetaData Meta { get; init; } = null!;
    }
}
