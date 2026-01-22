using System.Text.Json.Serialization;
using Xray.Config.Enums;

namespace Xray.Config.Models;

/// <summary>
/// DNS is an outbound protocol that is primarily used to intercept and forward DNS queries.
/// <para>This outgoing protocol can only accept DNS traffic (including UDP and TCP queries), other types of traffic will cause an error.</para>
/// </summary>
public class DnsOutbound : Outbound
{
    public DnsOutbound() : base(OutboundProtocol.Dns) { }

    [JsonPropertyName("settings")]
    public DnsSettings? Settings { get; set; }
}