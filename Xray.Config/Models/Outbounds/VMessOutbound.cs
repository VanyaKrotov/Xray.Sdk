using System.Text.Json.Serialization;
using Xray.Config.Enums;

namespace Xray.Config.Models;

public class VMessOutbound : Outbound
{
    public VMessOutbound() : base(OutboundProtocol.VMess) { }

    [JsonPropertyName("settings")]
    public VMessSettings? Settings { get; set; }
}
