using System.Text.Json.Serialization;

namespace ShoppingList.Core
{
    public class Application
    {
        public int Id { get; set; }

        [JsonPropertyName("household_id")]
        public int HouseholdId { get; set; }

        [JsonPropertyName("user_id")]
        public int UserId { get; set; }
    }
}
