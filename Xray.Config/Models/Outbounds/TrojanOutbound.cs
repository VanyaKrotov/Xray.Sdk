using System.Text.Json.Serialization;
using Xray.Config.Enums;

namespace Xray.Config.Models;

public class TrojanOutbound : Outbound
{
    public TrojanOutbound() : base(OutboundProtocol.Trojan) { }

    [JsonPropertyName("settings")]
    public TrojanSettings? Settings { get; set; }
}

public class TrojanServer : ClientServer { }