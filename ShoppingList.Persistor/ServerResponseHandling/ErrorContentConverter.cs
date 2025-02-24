using System.Text.Json;
using System.Text.Json.Serialization;

namespace ShoppingList.Persistor.ServerResponseHandling
{
    internal class ErrorContentConverter : JsonConverter<ErrorContent>
    {
        public override ErrorContent Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var errorContent = new ErrorContent();

            if (reader.TokenType == JsonTokenType.StartObject)
            {
                var dict = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(ref reader, options);
                errorContent.FieldErrors = dict;
            }
            else if (reader.TokenType == JsonTokenType.String)
            {
                errorContent.GeneralError = reader.GetString();
                //reader.Read();
            }
            else
            {
                throw new JsonException("Unexpected token type when parsing error content.");
            }

            return errorContent;
        }

        public override void Write(Utf8JsonWriter writer, ErrorContent value, JsonSerializerOptions options)
        {
            if (value.FieldErrors != null)
            {
                JsonSerializer.Serialize(writer, value.FieldErrors, options);
            }
            else if (!string.IsNullOrEmpty(value.GeneralError))
            {
                writer.WriteStringValue(value.GeneralError);
            }
            else
            {
                writer.WriteNullValue();
            }
        }
    }
}
