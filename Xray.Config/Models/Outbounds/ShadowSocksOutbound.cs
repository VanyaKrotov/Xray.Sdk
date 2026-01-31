using System.Text.Json.Serialization;
using Xray.Config.Enums;

namespace Xray.Config.Models;

public class ShadowSocksOutbound : Outbound
{
    public ShadowSocksOutbound() : base(OutboundProtocol.ShadowSocks) { }

    [JsonPropertyName("settings")]
    public ShadowSocksSettings? Settings { get; set; }
}

public class ShadowSocksServer : ClientServer
{
    /// <summary>
    /// Required parameter.
    /// </summary>
    [JsonPropertyName("method")]
    public EncryptionMethod? Method { get; set; }
}