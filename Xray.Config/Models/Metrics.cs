using System.Text.Json.Serialization;

namespace Xray.Config.Models;

/// <summary>
/// MetricsConfig
/// <para><see href="https://xtls.github.io/config/metrics.html">Docs</see></para>
/// </summary>
public class MetricsConfig
{
    /// <summary>
    /// Outbound proxy tag for metrics. By configuring AnyDoor's incoming connection and AnyDoor routing to this outbound proxy, you can access metrics through AnyDoor.
    /// </summary>
    [JsonPropertyName("tag")]
    public string? Tag { get; set; }

    /// <summary>
    /// A simpler way is to simply listen on the specified address and port to provide the service.
    /// <para>If this field is empty when set tag, it is automatically set to Metrics. If both fields are unset, the kernel will not start.</para>
    /// </summary>
    [JsonPropertyName("listen")]
    public string? Listen { get; set; }
}