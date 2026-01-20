using System.Text.Json.Serialization;
using Xray.Config.Enums;

namespace Xray.Config.Models;

public class HysteriaOutbound : Outbound
{
    public HysteriaOutbound() : base(OutboundProtocol.Hysteria) { }

    [JsonPropertyName("settings")]
    public HysteriaSettings? Settings { get; set; }
}