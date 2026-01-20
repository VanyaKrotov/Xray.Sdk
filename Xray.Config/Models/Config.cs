using System.Text.Json;
using System.Text.Json.Serialization;

namespace Xray.Config.Models;

/// <summary>
/// The configuration file of Xray is in JSON format, and the configuration format for the client and server is the same, except for the actual configuration content.
/// <para><see href="https://xtls.github.io/en/config">Docs</see></para>
/// </summary>
public class XrayConfig
{
    /// <summary>
    /// Log configuration controls how Xray outputs logs.
    /// </summary>
    [JsonPropertyName("log")]
    public LogConfig? Log { get; set; }

    /// <summary>
    /// It controls the version on which this config can run. When sharing config files, this prevents accidental execution on unwanted client versions. During execution, the client will check if its current version meets this requirement. Support of v25.8.3+.
    /// </summary>
    [JsonPropertyName("version")]
    public VersionConfig? Version { get; set; }

    /// <summary>
    /// API interface configuration provides a set of APIs based on gRPC for remote invocation.
    /// </summary>
    [JsonPropertyName("api")]
    public ApiConfig? Api { get; set; }

    /// <summary>
    /// Config for Built-in DNS server.
    /// </summary>
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