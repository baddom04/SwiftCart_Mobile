using System.Text.Json.Serialization;

namespace ShoppingList.Core
{
    public class Location
    {
        public int Id { get; init; }
        public required string Country { get; init; }

        [JsonPropertyName("zip_code")]
        public required string ZipCode { get; init; }
        public required string City { get; init; }
        public required string Street { get; init; }
        public required string Detail { get; init; }

        [JsonPropertyName("store_id")]
        public int StoreId { get; init; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; init; }

        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; init; }

        public override string ToString()
        {
            return $"{Country},{ZipCode} {City}, {Street}, {Detail}";
        }
    }
}
