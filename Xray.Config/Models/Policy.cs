using System.Text.Json.Serialization;

namespace Xray.Config.Models;

/// <summary>
/// A local policy that allows you to configure different user levels and corresponding policies.
/// <para><see href="https://xtls.github.io/config/policy.html">Docs</see></para>
/// </summary>
public class PolicyConfig
{
    /// <summary>
    /// Xray will apply different local policies based on the actual user level.
    /// </summary>
    [JsonPropertyName("levels")]
    public Dictionary<string, LevelPolicy>? Levels { get; set; }

    /// <summary>
    /// Xray system level policies.
    /// </summary>
    [JsonPropertyName("system")]
    public SystemPolicy? System { get; set; }
}

public class LevelPolicy
{
    /// <summary>
    /// Connection establishment time limit (handshake).
    /// <para>Measured in seconds.</para>
    /// <para>The default value is <b>4</b>.</para>
    /// <para>When processing a new connection by the incoming proxy, if the time spent on the handshake exceeds this value, the connection is terminated.</para>
    /// </summary>
    [JsonPropertyName("handshake")]
    public int? Handshake { get; set; }

    /// <summary>
    /// Connection idle timeout.
    /// <para>Measured in seconds.</para>
    /// <para>The default value is <b>300</b>.</para>
    /// <para>When processing a connection by an incoming or outgoing proxy, if no data is transferred for a period of time (including outgoing and incoming data), the connection is terminated.connIdle.</para>
    /// </summary>
    [JsonPropertyName("connIdle")]
    public int? ConnIdle { get; set; }

    /// <summary>
    /// The timeout limit after closing the outgoing connection channel.
    /// <para>Measured in seconds.</para>
    /// <para>The default value is <b>2</b>.</para>
    /// <para>When a server (e.g., a remote website) closes the outgoing connection, the outgoing proxy terminates the connection after uplinkOnly seconds.</para>
    /// </summary>
    [JsonPropertyName("uplinkOnly")]
    public int? UplinkOnly { get; set; }

    /// <summary>
    /// The timeout limit after closing an incoming connection.
    /// <para>Measured in seconds.</para>
    /// <para>The default value is <b>5</b>.</para>
    /// <para>When a client (e.g., a browser) closes an incoming connection, the incoming proxy terminates the connection after downlinkOnly seconds.</para>
    /// </summary>
    [JsonPropertyName("downlinkOnly")]
    public int? DownlinkOnly { get; set; }

    /// <summary>
    /// If the value is true, enable outgoing traffic accounting for all users of the current level.
    /// </summary>
    [JsonPropertyName("statsUserUplink")]
    public bool? StatsUserUplink { get; set; }

    /// <summary>
    /// If the value is true, enable incoming traffic accounting for all users of the current level.
    /// </summary>
    [JsonPropertyName("statsUserDownlink")]
    public bool? StatsUserDownlink { get; set; }

    /// <summary>
    /// If the value is true, enable online user accounting for all current-level users via email.
    /// </summary>
    [JsonPropertyName("statsUserOnline")]
    public bool? StatsUserOnline { get; set; }

    /// <summary>
    /// The internal buffer size for each request, in kilobytes.
    /// <para>Note that multiple requests can be multiplexed over a single connection (e.g., when using mux.coolGRPC). This means that even if they use the same underlying connection, their buffer pools are independent.</para>
    /// Default value:
    /// <list type="bullet">
    ///     <item>On ARM, MIPS, MIPSLE platforms the default value is 0;</item>
    ///     <item>On ARM64, MIPS64, MIPS64LE platforms the default value is 4;</item>
    ///     <item>On other platforms, the default value is 512.</item>
    /// </list>
    /// <para>The default value can be overridden using an environment variable XRAY_RAY_BUFFER_SIZE. Note that the unit of measurement for environment variables is megabytes (MB) (e.g., setting a value 1in an environment variable is equivalent to setting it 1024in the configuration).</para>
    /// </summary>
    [JsonPropertyName("bufferSize")]
    public long? BufferSize { get; set; }
}

/// <summary>
/// Xray system level policies.
/// </summary>
public class SystemPolicy
{
    /// <summary>
    /// If the value is true, enable outgoing traffic accounting for all incoming connections.
    /// </summary>
    [JsonPropertyName("statsInboundUplink")]
    public bool? StatsInboundUplink { get; set; }

    /// <summary>
    /// If the value is true, enable incoming traffic accounting for all incoming connections.
    /// </summary>
    [JsonPropertyName("StatsInboundDownlink")]
    public bool? StatsInboundDownlink { get; set; }

    /// <summary>
    /// If the value is true, enable outgoing traffic accounting for all outgoing connections.
    /// </summary>
    [JsonPropertyName("statsOutboundUplink")]
    public bool? StatsOutboundUplink { get; set; }

    /// <summary>
    /// If the value is true, enable incoming traffic accounting for all outgoing connections.
    /// </summary>
    [JsonPropertyName("statsOutboundDownlink")]
    public bool? StatsOutboundDownlink { get; set; }
}