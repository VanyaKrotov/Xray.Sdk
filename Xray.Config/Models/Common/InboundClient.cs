using System.Text.Json.Serialization;

namespace Xray.Config.Models;

public abstract class InboundClient : WithLevel
{
    /// <summary>
    /// User email address, used to separate traffic from different users (displayed in logs, statistics).
    /// </summary>
    [JsonPropertyName("email")]
    public required string Email { get; set; }
}