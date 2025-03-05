using System.Text.Json.Serialization;

namespace ShoppingList.Persistor.DTO
{
    public class HouseholdsMetaData
    {
        [JsonPropertyName("last_page")]
        public int MaxPages { get; init; }
    }
}
