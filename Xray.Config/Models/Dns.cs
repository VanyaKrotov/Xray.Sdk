using System.Text.Json.Serialization;
using Xray.Config.Enums;

namespace Xray.Config.Models;

public class DnsConfig
{
    [JsonPropertyName("hosts")]
    public Dictionary<string, List<string>>? Hosts { get; set; }

    [JsonPropertyName("servers")]
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