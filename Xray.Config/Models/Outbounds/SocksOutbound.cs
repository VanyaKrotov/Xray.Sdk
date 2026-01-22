using System.Text.Json.Serialization;
using Xray.Config.Enums;

namespace Xray.Config.Models;

public class SocksOutbound : Outbound
{
    public SocksOutbound() : base(OutboundProtocol.Socks) { }

    [JsonPropertyName("settings")]
    public SocksSettings? Settings { get; set; }
}