using System.Text.Json;
using System.Text.Json.Serialization;

namespace Xray.Config.Models;

public class XrayConfig
{
    [JsonPropertyName("log")]
    public LogConfig? Log { get; set; }

    [JsonPropertyName("version")]
    public VersionConfig? Version { get; set; }

    [JsonPropertyName("api")]
    public ApiConfig? Api { get; set; }

    [JsonPropertyName("dns")]
    public DnsConfig? Dns { get; set; }

    [JsonPropertyName("routing")]
    public RoutingConfig? Routing { get; set; }

    [JsonPropertyName("policy")]
    public PolicyConfig? Policy { get; set; }

    [JsonPropertyName("inbounds")]
    public List<Inbound> Inbounds { get; set; } = new();

    [JsonPropertyName("outbounds")]
    public List<Outbound> Outbounds { get; set; } = new();

    [JsonPropertyName("transport")]
    public TransportConfig? Transport { get; set; }

    [JsonPropertyName("stats")]
    public StatsConfig? Stats { get; set; }

    [JsonPropertyName("reverse")]
    public ReverseConfig? Reverse { get; set; }

    [JsonPropertyName("fakedns")]
    public List<FaceDnsConfig>? FaceDns { get; set; }

    [JsonPropertyName("metrics")]
    public List<MetricsConfig>? Metrics { get; set; }

    [JsonPropertyName("observatory")]
    public ObservatoryConfig? Observatory { get; set; }

    [JsonPropertyName("burstObservatory")]
    public BurstObservatoryConfig? BurstObservatory { get; set; }

    private static JsonSerializerOptions _options = new JsonSerializerOptions()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };

    public string ToJson() => JsonSerializer.Serialize(this, _options);

    public override string ToString() => ToJson();

    public static XrayConfig FromJson(string json) => JsonSerializer.Deserialize<XrayConfig>(json, _options)!;
}