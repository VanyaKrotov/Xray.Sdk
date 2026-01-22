using System.Text.Json.Serialization;
using Xray.Config.Enums;

namespace Xray.Config.Models;

/// <summary>
/// mKCP configuration for the current connection, valid only if this connection uses mKCP.
/// </summary>
public class KcpSettings
{
    /// <summary>
    /// Maximum transmission unit.
    /// <para>Select a value between 576 and 1460.</para>
    /// <para>By default 1350.</para>
    /// </summary>
    [JsonPropertyName("mtu")]
    public int? Mtu { get; set; }

    /// <summary>
    /// Transmission time interval, in milliseconds (ms), mKCP will send data at this rate.
    /// <para>Select a value between 10 and 100.</para>
    /// <para>By default 50.</para>
    /// </summary>
    [JsonPropertyName("tti")]
    public int? Tti { get; set; }

    /// <summary>
    /// The sending channel throughput, i.e. the maximum bandwidth used by the host to send data, in MB/s (note these are bytes, not bits).
    /// <para>Can be set to 0, meaning very little throughput.</para>
    /// <para>By default 5</para>
    /// </summary>
    [JsonPropertyName("uplinkCapacity")]
    public int? UplinkCapacity { get; set; }

    /// <summary>
    /// The receive channel throughput, i.e. the maximum bandwidth used by the host to receive data, in MB/s (note that these are bytes, not bits).
    /// <para>Can be set to 0, meaning very little throughput.</para>
    /// <para>By default 20.</para>
    /// </summary>
    [JsonPropertyName("downlinkCapacity")]
    public int? DownlinkCapacity { get; set; }

    /// <summary>
    /// Enable or disable overload control.
    /// <para>By default false.</para>
    /// </summary>
    [JsonPropertyName("congestion")]
    public bool? Congestion { get; set; }

    /// <summary>
    /// The read buffer size for a single connection, in MB.
    /// <para>By default 2.</para>
    /// </summary>
    [JsonPropertyName("readBufferSize")]
    public int? ReadBufferSize { get; set; }

    /// <summary>
    /// The write buffer size for a single connection, in MB.
    /// <para>By default 2.</para>
    /// </summary>
    [JsonPropertyName("writeBufferSize")]
    public int? WriteBufferSize { get; set; }

    /// <summary>
    /// Configuring data header masking
    /// </summary>
    [JsonPropertyName("header")]
    public KCPHeaders? Header { get; set; }

    /// <summary>
    /// Optional password encryption used to encrypt the data stream using the AES-128-GCM algorithm. The client and server must use the same password.
    /// </summary>
    [JsonPropertyName("seed")]
    public string? Seed { get; set; }
}

public class KCPHeaders
{
    /// <summary>
    /// Camouflage type.
    /// </summary>
    [JsonPropertyName("type")]
    public KcpHeaderType Type { get; set; } = KcpHeaderType.None;

    /// <summary>
    /// Used in conjunction with the masking type "dns", you can specify an arbitrary domain.
    /// </summary>
    [JsonPropertyName("domain")]
    public string? Domain { get; set; }
}