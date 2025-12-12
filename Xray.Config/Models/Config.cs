using System.Text.Json.Serialization;
using Xray.Config.Utilities;

namespace Xray.Config.Models;

public class XRayConfig
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

    public override string ToString() => XrayConfigJsonSerializer.Serialize(this);
}