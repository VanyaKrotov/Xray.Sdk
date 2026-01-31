using System.Collections.Specialized;
using System.Text.Json;

namespace Xray.Config.Utilities;

public class NameValueCollectionConverter : System.Text.Json.Serialization.JsonConverter<NameValueCollection>
{
    public override NameValueCollection Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var result = new NameValueCollection();
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected StartObject");
        }

        reader.Read();

        while (reader.TokenType != JsonTokenType.EndObject)
        {
            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException("Expected PropertyName");
            }

            var key = reader.GetString();

            reader.Read();

            switch (reader.TokenType)
            {
                case JsonTokenType.String:
                    result.Add(key, reader.GetString()); reader.Read();

                    break;

                case JsonTokenType.StartArray:
                    reader.Read();
                    
                    while (reader.TokenType != JsonTokenType.EndArray)
                    {
                        if (reader.TokenType != JsonTokenType.String)
                        {
                            throw new JsonException("Expected string in array");
                        }

                        result.Add(key, reader.GetString()); reader.Read();
                    }

                    reader.Read();

                    break;

                default:
                    throw new JsonException("Unexpected token");
            }
        }

        return result;
    }

    public override void Write(Utf8JsonWriter writer, NameValueCollection value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        foreach (string? key in value.AllKeys)
        {
            var values = value.GetValues(key);
            if (key == null || values == null || values.Length == 0)
            {
                continue;
            }

            if (values.Length == 1)
            {
                writer.WriteString(key, values[0]);
            }
            else
            {
                writer.WritePropertyName(key);
                writer.WriteStartArray();

                foreach (var v in values)
                {
                    writer.WriteStringValue(v);
                }

                writer.WriteEndArray();
            }
        }

        writer.WriteEndObject();
    }
}
