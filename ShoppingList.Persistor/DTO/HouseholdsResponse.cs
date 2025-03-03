using ShoppingList.Core;
using System.Text.Json.Serialization;

namespace ShoppingList.Persistor.DTO
{
    public class HouseholdsResponse
    {
        [JsonPropertyName("data")]
        public required IEnumerable<Household> QueryResult { get; init; }

        [JsonPropertyName("last_page")]
        public int MaxPages { get; init; }
    }
}
