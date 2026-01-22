using System.Text.Json.Serialization;
using Xray.Config.Enums;

namespace Xray.Config.Models;

public class WireguardOutbound : Outbound
{
    public WireguardOutbound() : base(OutboundProtocol.Wireguard) { }

    [JsonPropertyName("settings")]
    public WireguardSettings? Settings { get; set; }
}

public class WireguardOutboundPeer
{   
    /// <summary>
    /// Server address.
    /// </summary>
    [JsonPropertyName("endpoint")]
    public required string Endpoint { get; set; }

    /// <summary>
    /// The server's public key for verification.
    /// </summary>
    [JsonPropertyName("publicKey")]
    public required string PublicKey { get; set; }
    
    /// <summary>
    /// Additional symmetric encryption key.
    /// </summary>
    [JsonPropertyName("preSharedKey")]
    public string? PreSharedKey { get; set; }

    /// <summary>
    /// Heartbeat packet sending interval, in seconds. The default value is 0 (no heartbeat).
    /// </summary>
    [JsonPropertyName("keepAlive")]
    public bool? KeepAlive { get; set; }

    /// <summary>
    /// Wireguard only allows traffic from certain IP addresses.
    /// </summary>
    [JsonPropertyName("allowedIPs")]
    public List<string>? allowedIPs { get; set; }
}