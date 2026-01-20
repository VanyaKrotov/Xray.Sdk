using System.Text.Json;
using System.Text.Json.Serialization;

namespace Xray.Config.Models;

/// <summary>
/// The configuration file of Xray is in JSON format, and the configuration format for the client and server is the same, except for the actual configuration content.
/// <para><see href="https://xtls.github.io/config">Docs</see></para>
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

    /// <summary>
    /// Configures routing. Specify rules to route connections through different outbounds.
    /// </summary>
    [JsonPropertyName("routing")]
    public RoutingConfig? Routing { get; set; }

    /// <summary>
    /// A local policy that allows you to configure different user levels and corresponding policies.
    /// </summary>
    [JsonPropertyName("policy")]
    public PolicyConfig? Policy { get; set; }

    /// <summary>
    /// ?
    /// </summary>
    [JsonPropertyName("inbounds")]
    public List<Inbound> Inbounds { get; set; } = new();

    /// <summary>
    /// ?
    /// </summary>
    [JsonPropertyName("outbounds")]
    public List<Outbound> Outbounds { get; set; } = new();

    /// <summary>
    /// ?
    /// </summary>
    [JsonPropertyName("transport")]
    public TransportConfig? Transport { get; set; }

    /// <summary>
    /// Used to configure the collection of traffic statistics.
    /// </summary>
    [JsonPropertyName("stats")]
    public StatsConfig? Stats { get; set; }

    /// <summary>
    /// A reverse proxy can redirect traffic from a server to a client, that is, perform reverse traffic forwarding.
    /// </summary>
    [JsonPropertyName("reverse")]
    public ReverseConfig? Reverse { get; set; }

    /// <summary>
    /// Setting up FakeDNS. Can be used in conjunction with transparent proxying to obtain real domain names.
    /// </summary>
    [JsonPropertyName("fakedns")]
    public List<FaceDnsConfig>? FaceDns { get; set; }

    /// <summary>
    /// An easier (and hopefully better) way to export statistics.
    /// </summary>
    [JsonPropertyName("metrics")]
    public MetricsConfig? Metrics { get; set; }

    /// <summary>
    /// The connection monitoring component uses HTTP pings to check the connection status of outbound proxies.
    /// </summary>
    [JsonPropertyName("observatory")]
    public ObservatoryConfig? Observatory { get; set; }

    /// <summary>
    /// The connection monitoring component uses HTTP pings to check the connection status of outbound proxies.
    /// </summary>
    [JsonPropertyName("burstObservatory")]
    public BurstObservatoryConfig? BurstObservatory { get; set; }

    //
    private static JsonSerializerOptions _options = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };

    /// <summary>
    /// Convert config to valid json string
    /// </summary>
    /// <returns>Json configuration</returns>
    public string ToJson() => JsonSerializer.Serialize(this, _options);

    /// <summary>
    /// Parse xray configuration from json 
    /// </summary>
    /// <param name="json">Valid xray configuration string</param>
    /// <returns>Xray configuration object</returns>
    public static XrayConfig FromJson(string json) => JsonSerializer.Deserialize<XrayConfig>(json, _options)!;
}