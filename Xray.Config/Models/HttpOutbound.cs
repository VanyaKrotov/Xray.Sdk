using System.Text.Json.Serialization;
using Xray.Config.Enums;

namespace Xray.Config.Models;

/// <summary>
/// HTTP protocol.
/// </summary>
public class HttpOutbound : Outbound
{
    public HttpOutbound() : base(OutboundProtocol.Http) { }

    [JsonPropertyName("settings")]
    public HttpSettings? Settings { get; set; }
}
