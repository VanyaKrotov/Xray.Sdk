using System.Text.Json;
using System.Text.Json.Serialization;

namespace Xray.Config.Models;

[JsonConverter(typeof(PortValueConverter))]
public class Port
{
    public int? Single { get; set; }
    public (int From, int To)? Range { get; set; }
    public string? Env { get; set; }

    public Port(int port)
    {
        Single = port;
    }

    public Port(int from, int to)
    {
        Range = (from, to);
    }

    public Port(string env)
    {
        Env = env;
    }

    public bool HasValue => Single.HasValue || Env != null || Range.HasValue;

    public override string ToString()
    {
        if (Single.HasValue)
        {
            return ((int)Single).ToString();
        }

        if (Range.HasValue)
        {
            return $"{Range.Value.From}-{Range.Value.To}";
        }

        return $"env:{Env}";
    }
}

class PortValueConverter : JsonConverter<Port?>
{
    public override Port? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.Null:
                return null;

            case JsonTokenType.Number:
                return new Port(reader.GetInt32());

            case JsonTokenType.String:
                string raw = reader.GetString()!;

                if (raw.StartsWith("env:", StringComparison.OrdinalIgnoreCase))
                {
                    return new Port(raw.Substring(4));
                }

                if (raw.Contains('-'))
                {
                    var parts = raw.Split('-', 2, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length == 2 && int.TryParse(parts[0], out int from) && int.TryParse(parts[1], out int to))
                    {
                        return new Port(from, to);
                    }

                    throw new JsonException($"Invalid port range format: {raw}");
                }

                if (int.TryParse(raw, out int single))
                {
                    return new Port(single);
                }

                throw new JsonException($"Invalid port value: {raw}");

            default:
                throw new JsonException("Port must be number or string");
        }
    }

    public override void Write(Utf8JsonWriter writer, Port? value, JsonSerializerOptions options)
    {
        if (value == null)
        {
            writer.WriteNullValue();

            return;
        }

        if (value.Single.HasValue)
        {
            writer.WriteNumberValue(value.Single.Value);

            return;
        }

        if (value.HasValue)
        {
            writer.WriteStringValue(value.ToString());

            return;
        }
    }
}
