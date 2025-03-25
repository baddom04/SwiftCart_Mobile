using ShoppingList.Core.Enums;
using System.Text.Json.Serialization;

namespace ShoppingList.Core
{
    public class Household
    {
        public int Id { get; init; }
        public required string Name { get; init; }
        public required string Identifier { get; init; }

        [JsonPropertyName("user_id")]
        public int UserId { get; init; }
        public HouseholdRelationship? Relationship { get; set; }
    }
}
