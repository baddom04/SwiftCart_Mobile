using ShoppingList.Core;
using System.Text.Json.Serialization;

namespace ShoppingList.Persistor.DTO
{
    internal class CommentsResponse
    {
        [JsonPropertyName("data")]
        public required IEnumerable<Comment> QueryResult { get; init; }
    }
}
