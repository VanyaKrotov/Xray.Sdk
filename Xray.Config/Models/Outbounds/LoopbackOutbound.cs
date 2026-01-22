using System.Text.Json.Serialization;
using Xray.Config.Enums;

namespace Xray.Config.Models;

public class LoopbackOutbound : Outbound
{
    public LoopbackOutbound() : base(OutboundProtocol.Loopback) { }

    [JsonPropertyName("settings")]
    public LoopbackSettings? Settings { get; set; }
}