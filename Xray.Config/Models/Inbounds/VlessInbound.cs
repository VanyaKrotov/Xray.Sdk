using System.Text.Json.Serialization;
using Xray.Config.Enums;

namespace Xray.Config.Models;

/// <summary>
/// VLESS is a lightweight, stateless transport protocol that is split into an inbound and outbound portion and can serve as a bridge between an Xray client and server.
/// </summary>
public class VlessInbound : Inbound
{
    public VlessInbound() : base(InboundProtocol.Vless) { }

    [JsonPropertyName("settings")]
    public VlessSettings Settings { get; set; } = new();
}

public class VlessClient : WithLevel
{   
    /// <summary>
    /// The VLESS user identifier can be any string less than 30 bytes long or a valid UUID.
    /// </summary>
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    /// <summary>
    /// User email address, used to separate traffic from different users (displayed in logs, statistics).
    /// </summary>
    [JsonPropertyName("email")]
    public required string Email { get; set; }

    /// <summary>
    /// Flow control mode, used to select the XTLS algorithm.
    /// </summary>
    [JsonPropertyName("flow")]
    public XtlsFlow Flow { get; set; } = XtlsFlow.None;
}