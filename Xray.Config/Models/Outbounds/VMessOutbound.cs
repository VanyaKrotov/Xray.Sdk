using System.Text.Json.Serialization;
using Xray.Config.Enums;

namespace Xray.Config.Models;

public class VMessOutbound : Outbound
{
    public VMessOutbound() : base(OutboundProtocol.VMess) { }

    [JsonPropertyName("settings")]
    public VMessSettings? Settings { get; set; }
}

public class VMessVNext : VNextModel<VMessUser> { }

public class VMessUser : WithLevel
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("security")]
    public VMessSecurity Security { get; set; } = VMessSecurity.Auto;

    [JsonPropertyName("alterId")]
    public int? AlterId { get; set; }
}