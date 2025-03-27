using System.Text.Json.Serialization;

namespace ShoppingList.Persistor.DTO
{
    public class MetaData
    {
        [JsonPropertyName("last_page")]
        public int MaxPages { get; init; }
    }
}
