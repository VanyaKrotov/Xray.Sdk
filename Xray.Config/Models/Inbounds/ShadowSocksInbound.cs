using System.Text.Json.Serialization;
using Xray.Config.Enums;

namespace Xray.Config.Models;

/// <summary>
/// Shadowsocks protocol, compatible with most other implementations.
/// <para><see href="https://wikipedia.org/wiki/Shadowsocks">Docs</see></para>
/// </summary>
public class ShadowSocksInbound : Inbound
{
    public ShadowSocksInbound() : base(InboundProtocol.ShadowSocks) { }

    [JsonPropertyName("settings")]
    public ShadowSocksSettings? Settings { get; set; }
}

public class ShadowSocksClient : WithLevel
{
    /// <summary>
    /// Required for Shadowsocks 2022. A pre-shared key similar to WireGuard is used as the password.
    /// </summary>
    [JsonPropertyName("password")]
    public string? Password { get; set; }

    /// <summary>
    /// Encryption method, see available options above.
    /// </summary>
    [JsonPropertyName("method")]
    public EncryptionMethod? Method { get; set; }

    /// <summary>
    /// User email address, used to separate traffic from different users (displayed in logs, statistics).
    /// </summary>
    [JsonPropertyName("email")]
    public string? Email { get; set; }
}