using System.Text.Json.Serialization;
using Xray.Config.Enums;

namespace Xray.Config.Models;

/// <summary>
/// User-space implementation of the Wireguard protocol.
/// </summary>
public class WireguardInbound : Inbound
{
    public WireguardInbound() : base(InboundProtocol.Wireguard) { }

    [JsonPropertyName("settings")]
    public WireguardSettings? Settings { get; set; }
}

public class WireguardPeer
{
    /// <summary>
    /// Public key for verification.
    /// </summary>
    [JsonPropertyName("publicKey")]
    public required string PublicKey { get; set; }

    /// <summary>
    /// Allowed source IP addresses.
    /// </summary>
    [JsonPropertyName("allowedIPs")]
    public List<string>? AllowedIPs { get; set; }
}