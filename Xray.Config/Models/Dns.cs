using System.Text.Json;
using System.Text.Json.Serialization;
using Xray.Config.Enums;

namespace Xray.Config.Models;

public class DnsConfig
{
    [JsonPropertyName("hosts")]
    public Dictionary<string, List<string>>? Hosts { get; set; }

    [JsonPropertyName("servers")]
    [JsonConverter(typeof(DnsServersJsonConverter))]
    public List<DnsServer>? Servers { get; set; }

    [JsonPropertyName("clientIp")]
    public string? ClientIP { get; set; }

    [JsonPropertyName("queryStrategy")]
    public DnsQueryStrategy? QueryStrategy { get; set; }

    [JsonPropertyName("disableCache")]
    public bool DisableCache { get; set; }

    [JsonPropertyName("disableFallback")]
    public bool DisableFallback { get; set; }

    [JsonPropertyName("disableFallbackIfMatch")]
    public bool DisableFallbackIfMatch { get; set; }

    [JsonPropertyName("useSystemHosts")]
    public bool UseSystemHosts { get; set; }

    [JsonPropertyName("tag")]
    public string? Tag { get; set; }
}

public class DnsServer
{
    [JsonPropertyName("address")]
    public string? Address { get; set; }

    [JsonPropertyName("port")]
    public int? Port { get; set; }

    [JsonPropertyName("domains")]
    public List<string>? Domains { get; set; }

    [JsonPropertyName("expectedIPs")]
    public List<string>? ExpectedIPs { get; set; }

    [JsonPropertyName("skipFallback")]
    public bool SkipFallback { get; set; }

    [JsonPropertyName("clientIP")]
    public string? ClientIP { get; set; }

    [JsonPropertyName("queryStrategy")]
    public DnsQueryStrategy? QueryStrategy { get; set; }

    [JsonPropertyName("timeoutMs")]
    public int? TimeoutMs { get; set; }

    [JsonPropertyName("disableCache")]
    public bool DisableCache { get; set; }

    [JsonPropertyName("finalQuery")]
    public bool FinalQuery { get; set; }
}

// convertors

class DnsServersJsonConverter : JsonConverter<List<DnsServer>>
{
    public override List<DnsServer>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var servers = new List<DnsServer>();

        if (reader.TokenType != JsonTokenType.StartArray)
            throw new JsonException("Expected StartArray for servers");

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndArray) { break; }

            if (reader.TokenType == JsonTokenType.String)
            {
                servers.Add(new DnsServer { Address = reader.GetString() });
            }
            else if (reader.TokenType == JsonTokenType.StartObject)
            {
                using (var doc = JsonDocument.ParseValue(ref reader))
                {
                    var server = doc.Deserialize<DnsServer>(options);
                    if (server != null)
                    {
                        servers.Add(server);
                    }
                }
            }
            else
            {
                throw new JsonException($"Unexpected token {reader.TokenType} in servers array");
            }
        }

        return servers;
    }

    public override void Write(Utf8JsonWriter writer, List<DnsServer> value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();

        foreach (var server in value)
        {
            JsonSerializer.Serialize(writer, server, options);
        }

        writer.WriteEndArray();
    }
}