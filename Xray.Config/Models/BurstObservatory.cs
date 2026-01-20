using System.Text.Json.Serialization;

namespace Xray.Config.Models;

/// <summary>
/// The connection monitoring component uses HTTP pings to check the connection status of outbound proxies.
/// <para><see href="https://xtls.github.io/ru/config/observatory.html#burstobservatoryobject">Docs</see></para>
/// </summary>
public class BurstObservatoryConfig
{
    /// <summary>
    /// An array of strings, each element of which will be used to match an outbound connection tag prefix.
    /// <para>For example, for the following outbound connection tags: [ "a", "ab", "c", "ba" ], "subjectSelector": ["a"] will match [ "a", "ab" ].</para>
    /// </summary>
    [JsonPropertyName("subjectSelector")]
    public required List<string> SubjectSelector { get; set; }

    /// <summary>
    /// Ping schema
    /// </summary>
    [JsonPropertyName("pingConfig")]
    public required PingConfig PingConfig { get; set; }
}

/// <summary>
/// Ping schema
/// </summary>
public class PingConfig
{
    /// <summary>
    /// The URL used to check the outbound proxy connection status.
    /// <para>This URL must return HTTP status code 204.</para>
    /// </summary>
    [JsonPropertyName("destination")]
    public required string Destination { get; set; }

    /// <summary>
    /// The URL used to test local network connectivity.
    /// <para>An empty string means no local network connectivity testing is performed.</para>
    /// </summary>
    [JsonPropertyName("connectivity")]
    public string? Connectivity { get; set; }

    /// <summary>
    /// Check all matching outgoing proxies for the specified time, sending sampling + 1 requests to each proxy.
    /// <para>Time format: number + unit, e.g. "10s", "2h45m".</para>
    /// Supported units: ns, us, ms, s, m, h (nanoseconds, microseconds, milliseconds, seconds, minutes, hours).
    /// </summary>
    [JsonPropertyName("interval")]
    public string? Interval { get; set; }

    /// <summary>
    /// The number of recent test results to keep.
    /// </summary>
    [JsonPropertyName("sampling")]
    public int? Sampling { get; set; }

    /// <summary>
    /// Response timeout during verification. The format is the same as for interval.
    /// </summary>
    [JsonPropertyName("timeout")]
    public string? Timeout { get; set; }
}