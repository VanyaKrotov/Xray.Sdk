using System.Text.Json.Serialization;

namespace Xray.Config.Models;

public class BurstObservatoryConfig
{
    [JsonPropertyName("subjectSelector")]
    public required List<string> SubjectSelector { get; set; }

    [JsonPropertyName("pingConfig")]
    public required PingConfig PingConfig { get; set; }
}

public class PingConfig
{
    [JsonPropertyName("destination")]
    public required string Destination { get; set; }

    [JsonPropertyName("connectivity")]
    public string? Connectivity { get; set; }

    [JsonPropertyName("interval")]
    public string? Interval { get; set; }

    [JsonPropertyName("sampling")]
    public int? Sampling { get; set; }

    [JsonPropertyName("timeout")]
    public string? Timeout { get; set; }
}