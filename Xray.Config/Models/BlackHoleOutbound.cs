using System.Text.Json.Serialization;
using Xray.Config.Enums;

namespace Xray.Config.Models;

/// <summary>
/// Blackhole is an outbound protocol that blocks all outgoing data. When combined with routing configuration, it can be used to deny access to specific websites.
/// </summary>
public class BlackHoleOutbound : Outbound
{
    public BlackHoleOutbound() : base(OutboundProtocol.BlackHole) { }

    [JsonPropertyName("settings")]
    public BlackHoleSettings? Settings { get; set; }
}

public class BlackHoleResponse
{
    /// <summary>
    /// If type equal "none"(the default value), Blackhole will simply close the connection.
    /// <para>If type equal to "http", Blackhole will return a simple HTTP 403 packet and then close the connection.</para>
    /// </summary>
    [JsonPropertyName("type")]
    public HeadersType Type { get; set; } = HeadersType.None;
}