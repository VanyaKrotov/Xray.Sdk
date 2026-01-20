using System.Text.Json.Serialization;
using Xray.Config.Enums;

namespace Xray.Config.Models;

/// <summary>
/// Freedom is an outgoing protocol that can be used to send (plain) TCP or UDP data to any network.
/// </summary>
public class FreedomOutbound : Outbound
{
    public FreedomOutbound() : base(OutboundProtocol.Freedom) { }

    [JsonPropertyName("settings")]
    public FreedomSettings? Settings { get; set; }
}
