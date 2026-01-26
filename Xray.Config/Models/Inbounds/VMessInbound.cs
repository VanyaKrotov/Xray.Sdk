using System.Text.Json.Serialization;
using Xray.Config.Enums;

namespace Xray.Config.Models;

/// <summary>
/// VMess is an encrypted transport protocol that is typically used as a bridge between Xray clients and servers.
/// </summary>
public class VMessInbound : Inbound
{
    public VMessInbound() : base(InboundProtocol.VMess) { }

    [JsonPropertyName("settings")]
    public VMessSettings Settings { get; set; } = new();
}

public class VMessClient : WithLevel
{
    /// <summary>
    /// The VMess user ID. This can be any string less than 30 bytes long or a valid UUID.
    /// </summary>
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    /// <summary>
    /// The user's email address, used to separate traffic from different users.
    /// </summary>
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

}

public class VMessDefaultSettings : WithLevel { }