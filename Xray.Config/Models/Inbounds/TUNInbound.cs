using System.Text.Json.Serialization;
using Xray.Config.Enums;

namespace Xray.Config.Models;

/// <summary>
/// Creates a TUN interface; traffic sent to this interface will be processed by Xray. Currently, only Windows and Linux are supported.
/// </summary>
public class TUNInbound : Inbound
{
    public TUNInbound() : base(InboundProtocol.Tun) { }

    [JsonPropertyName("settings")]
    public TUNSettings Settings { get; set; } = new();
}