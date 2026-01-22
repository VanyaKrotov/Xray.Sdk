using System.Text.Json.Serialization;
using Xray.Config.Enums;

namespace Xray.Config.Models;

/// <summary>
/// Tunnel, or Dokodemo door, can listen on a local port and send all data received on this port through outbound to the specified server port, thereby achieving the effect of port forwarding.
/// </summary>
public class DokodemoDoorInbound : Inbound
{
    public DokodemoDoorInbound() : base(InboundProtocol.DokodemoDoor) { }

    [JsonPropertyName("settings")]
    public DokodemoDoorSettings? Settings { get; set; }
}