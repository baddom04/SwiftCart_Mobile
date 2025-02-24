using System.Text.Json.Serialization;

namespace ShoppingList.Persistor.ServerResponseHandling
{
    internal class ErrorResponse
    {
        [JsonPropertyName("error")]
        [JsonConverter(typeof(ErrorContentConverter))]
        public ErrorContent? Error { get; set; }
    }
}
