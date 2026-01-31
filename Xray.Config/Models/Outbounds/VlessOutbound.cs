using System.Text.Json.Serialization;
using Xray.Config.Enums;

namespace Xray.Config.Models;

public class VlessOutbound : Outbound
{
    public VlessOutbound() : base(OutboundProtocol.Vless) { }

    [JsonPropertyName("settings")]
    public VlessSettings? Settings { get; set; }
}

public class VlessReverse
{
    /// <summary>
    /// This is the incoming proxy tag for this reverse proxy. When the server sends a request to the reverse proxy, it will enter the routing system through the incoming connection with this tag. Use the routing system to route it to the desired outgoing connection.
    /// </summary>
    [JsonPropertyName("tag")]
    public required string Tag { get; set; }
}

public class VlessUser : WithLevel
{
    /// <summary>
    /// The VLESS user identifier can be any string less than 30 bytes long or a valid UUID.
    /// </summary>
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("encryption")]
    public VlessEncryption Encryption { get; set; } = VlessEncryption.None;

    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("flow")]
    public XtlsFlow? Flow { get; set; }
}

public class VlessVNext : VNextModel<VlessUser> { }