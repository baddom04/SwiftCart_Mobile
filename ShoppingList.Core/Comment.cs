using System.Text.Json.Serialization;

namespace ShoppingList.Core
{
    public class Comment
    {
        public int Id { get; init; }
        [JsonPropertyName("user_id")]
        public int UserId { get; init; }
        [JsonPropertyName("grocery_id")]
        public int GroceryId { get; init; }
        public required string Content { get; init; }

        public required User User { get; init; }
    }
}
