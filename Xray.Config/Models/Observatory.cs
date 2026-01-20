using System.Text.Json.Serialization;

namespace Xray.Config.Models;

/// <summary>
/// The connection monitoring component uses HTTP pings to check the connection status of outbound proxies.
/// <para><see href="https://xtls.github.io/config/observatory.html">Docs</see></para>
/// </summary>
public class ObservatoryConfig
{
    /// <summary>
    /// An array of strings, each element of which will be used to match an outbound connection tag prefix.
    /// <para>For example, for the following outbound connection tags: [ "a", "ab", "c", "ba" ], "subjectSelector": ["a"] will match [ "a", "ab" ].</para>
    /// </summary>
    [JsonPropertyName("subjectSelector")]
    public required List<string> SubjectSelector { get; set; }

    /// <summary>
    /// The URL used to check the outbound proxy connection status.
    /// </summary>
    [JsonPropertyName("probeUrl")]
    public required string ProbeUrl { get; set; }

    /// <summary>
    /// The interval between checks.
    /// <para>Time format: number + unit, for example "10s", "2h45m".</para>
    /// <para>Supported units: ns, us, ms, s, m, h(nanoseconds, microseconds, milliseconds, seconds, minutes, hours).</para>
    /// </summary>
    [JsonPropertyName("probeInterval")]
    public required string ProbeInterval { get; set; }

    /// <summary>
    /// <list type="bullet">
    ///     <item>true - check all relevant outgoing proxies simultaneously, then pause for the time specified in probeInterval.</item>
    ///     <item>false- check the corresponding outgoing proxies in turn, pausing for the time specified in probeIntervalafter checking each proxy.</item>
    /// </list>
    /// </summary>
    [JsonPropertyName("enableConcurrency")]
    public bool EnableConcurrency { get; set; }
}