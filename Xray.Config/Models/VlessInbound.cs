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

public class VlessClient
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("level")]
    public int Level { get; set; } = 0;

    [JsonPropertyName("email")]
    public required string Email { get; set; }

    [JsonPropertyName("flow")]
    public Flow Flow { get; set; } = Flow.None;
}