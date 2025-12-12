using System.Text.Json.Serialization;
using Xray.Config.Enums;

namespace Xray.Config.Models;

public class RoutingConfig
{
    [JsonPropertyName("domainStrategy")]
    public RoutingDomainStrategy? DomainStrategy { get; set; }

    [JsonPropertyName("domainMatcher")]
    public DomainMatcher? DomainMatcher { get; set; }

    [JsonPropertyName("rules")]
    public List<RoutingRule> Rules { get; set; } = new();

    [JsonPropertyName("balancers")]
    public List<RoutingBalancer> Balancers { get; set; } = new();
}

public class RoutingRule
{
    [JsonPropertyName("domainMatcher")]
    public DomainMatcher? DomainMatcher { get; set; }

    [JsonPropertyName("type")]
    public RoutingRuleType Type { get; set; } = RoutingRuleType.Field;

    [JsonPropertyName("domain")]
    public List<string>? Domain { get; set; }

    [JsonPropertyName("ip")]
    public List<string>? Ip { get; set; }

    [JsonPropertyName("port")]
    public string? Port { get; set; }

    [JsonPropertyName("sourcePort")]
    public string? SourcePort { get; set; }

    [JsonPropertyName("network")]
    public string? Network { get; set; }

    [JsonPropertyName("source")]
    public List<string>? Source { get; set; }

    [JsonPropertyName("user")]
    public List<string>? User { get; set; }

    [JsonPropertyName("inboundTag")]
    public List<string>? InboundTag { get; set; }

    [JsonPropertyName("protocol")]
    public List<NetProtocol>? Protocol { get; set; }

    [JsonPropertyName("attrs")]
    public Dictionary<string, string>? Attrs { get; set; }

    [JsonPropertyName("outboundTag")]
    public string? OutboundTag { get; set; }

    [JsonPropertyName("balancerTag")]
    public string? BalancerTag { get; set; }
}

public class RoutingBalancer
{
    [JsonPropertyName("tag")]
    public required string Tag { get; set; }

    [JsonPropertyName("selector")]
    public List<string>? Selector { get; set; }
}