using System.Text.Json.Serialization;

namespace Xray.Config.Models;

/// <summary>
/// A low-level QUIC transport implementation for Hysteria2 in Xray. Typically used in conjunction with <see href="https://xtls.github.io/config/outbounds/hysteria.html">the hysteria2 outbound protocol</see>.
/// </summary>
public class HysteriaSettings
{
    /// <summary>
    /// Hysteria version must be 2. Default is 2.
    /// </summary>
    [JsonPropertyName("version")]
    public int Version { get; set; } = 2;

    /// <summary>
    /// Hysteria authentication password must match on both server and client.
    /// </summary>
    [JsonPropertyName("auth")]
    public required string Auth { get; set; }

    /// <summary>
    /// Upload speed limit. Default value is 0.
    /// <para>The format is user-friendly and supports various common bits per second notations, including 1000000, 100kb, 20 mb, 100 mbps, 1g, 1 tbps and so on. Case is not significant, and spaces between units are optional. Without units, bps (bits per second) is used by default; the value cannot be lower than 65535 bps.</para>
    /// </summary>
    [JsonPropertyName("up")]
    public string? Up { get; set; }

    /// <summary>
    /// Download speed limit. Default value is 0.
    /// <para>The format is user-friendly and supports various common bits per second notations, including 1000000, 100kb, 20 mb, 100 mbps, 1g, 1 tbps and so on. Case is not significant, and spaces between units are optional. Without units, bps (bits per second) is used by default; the value cannot be lower than 65535 bps.</para>
    /// </summary>
    [JsonPropertyName("down")]
    public string? Down { get; set; }

    /// <summary>
    /// Configuration of UDP port hopping.
    /// </summary>
    [JsonPropertyName("udphop")]
    public HysteriaUDPhop? UDPhop { get; set; }

    [JsonPropertyName("initStreamReceiveWindow")]
    public int? InitStreamReceiveWindow { get; set; }

    [JsonPropertyName("maxStreamReceiveWindow")]
    public int? MaxStreamReceiveWindow { get; set; }

    [JsonPropertyName("initConnectionReceiveWindow")]
    public int? InitConnectionReceiveWindow { get; set; }

    [JsonPropertyName("maxConnectionReceiveWindow")]
    public int? MaxConnectionReceiveWindow { get; set; }

    /// <summary>
    /// Maximum idle timeout (in seconds). How long the server will wait before closing the connection if it has not received any data from the client. Range: 4~120 seconds, default: 30 seconds.
    /// </summary>
    [JsonPropertyName("maxIdleTimeout")]
    public int? MaxIdleTimeout { get; set; }

    /// <summary>
    /// QUIC KeepAlive interval (in seconds). Range: 2~60 seconds. Disabled by default.
    /// </summary>
    [JsonPropertyName("keepAlivePeriod")]
    public int? KeepAlivePeriod { get; set; }

    /// <summary>
    /// Whether to disable Path MTU Discovery.
    /// </summary>
    [JsonPropertyName("disablePathMTUDiscovery")]
    public bool? DisablePathMTUDiscovery { get; set; }
}

public class HysteriaUDPhop
{
    /// <summary>
    /// Is the port range to jump to.
    /// <para>It can be a string with a number, such as "1234"; or a numeric range, such as "1145-1919", which represents 775 ports from 1145 to 1919.</para>
    /// <para>Commas can be used for separation, such as ,11,13,15-17 which represents 5 ports: 11, 13, and 15 through 17.</para>
    /// </summary>
    public required string Port { get; set; }

    /// <summary>
    /// UDP interval 
    /// </summary>
    public int Interval { get; set; }
}