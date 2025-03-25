using ShoppingList.Core.Enums;
using System.Text.Json.Serialization;

namespace ShoppingList.Core
{
    public class User
    {
        public int Id { get; init; }
        public required string Name { get; init; }
        public required string Email { get; init; }
        public DateTime? EmailVerifiedAt { get; init; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; init; }

        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; init; }
        public UserRole Admin { get; init; }
    }
}
