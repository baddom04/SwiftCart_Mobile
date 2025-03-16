using ShoppingList.Core;
using System.Text.Json.Serialization;

namespace ShoppingList.Persistor.DTO
{
    public class GroceriesResponse
    {
        [JsonPropertyName("data")]
        public required IEnumerable<Grocery> QueryResult { get; init; }
    }
}
