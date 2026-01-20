using System.Text.Json.Serialization;
using Xray.Config.Enums;

namespace Xray.Config.Models;

public class Hysteria2Outbound : Outbound
{
    public Hysteria2Outbound() : base(OutboundProtocol.Hysteria2) { }

    [JsonPropertyName("settings")]
    public Hysteria2Settings? Settings { get; set; }
}