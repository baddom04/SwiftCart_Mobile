using System.Text.Json;
using System.Text.Json.Serialization;

namespace ShoppingList.Core.JsonConverters
{
    internal class BoolFromIntegerConverter : JsonConverter<bool>
    {
        public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Number)
            {
                return reader.GetInt32() == 1;
            }

            if (reader.TokenType == JsonTokenType.True || reader.TokenType == JsonTokenType.False)
            {
                return reader.GetBoolean();
            }

            throw new JsonException("Unexpected token type for boolean value.");
        }

        public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
        {
            // Convert bool to 1 or 0 when writing the value
            writer.WriteNumberValue(value ? 1 : 0);
        }
    }
}
