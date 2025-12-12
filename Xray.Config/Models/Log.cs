using System.Text.Json.Serialization;
using Xray.Config.Enums;

namespace Xray.Config.Models;

public class LogConfig
{
    [JsonPropertyName("access")]
    public string? Access { get; set; }

    [JsonPropertyName("error")]
    public string? Error { get; set; }

    [JsonPropertyName("loglevel")]
    public LogLevel? LogLevel { get; set; }

    [JsonPropertyName("dnsLog")]
    public bool DnsLog { get; set; }

    [JsonPropertyName("maskAddress")]
    public string? MaskAddress { get; set; }
}