using System.Text.Json.Serialization;
using Xray.Config.Enums;

namespace Xray.Config.Models;

/// <summary>
/// We will now show how a trojan server will react to a valid Trojan Protocol and other protocols.
/// </summary>
public class TrojanInbound : Inbound
{
    public TrojanInbound() : base(InboundProtocol.Trojan) { }

    [JsonPropertyName("settings")]
    public TrojanSettings Settings { get; set; } = new();
}

public class TrojanClient : WithLevel
{
    /// <summary>
    /// Required parameter, any string.
    /// </summary>
    [JsonPropertyName("password")]
    public required string Password { get; set; }

    /// <summary>
    /// Email address, optional, used to identify the user.
    /// </summary>
    [JsonPropertyName("email")]
    public required string Email { get; set; }
}