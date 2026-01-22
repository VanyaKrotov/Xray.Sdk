using System.Text.Json.Serialization;
using Xray.Config.Enums;

namespace Xray.Config.Models;

/// <summary>
/// Using incoming connections http is more appropriate in a local area network or local environment where it can be used to listen for incoming connections and provide local services to other programs.
/// </summary>
public class HttpInbound : Inbound
{
    public HttpInbound() : base(InboundProtocol.Http) { }

    [JsonPropertyName("settings")]
    public HttpSettings? Settings { get; set; }
}

public class HttpAccount
{   
    /// <summary>
    /// Username, data type: string.
    /// </summary>
    [JsonPropertyName("user")]
    public required string User { get; set; }

    /// <summary>
    /// Password, data type: string.
    /// </summary>
    [JsonPropertyName("pass")]
    public required string Password { get; set; }

    public HttpAccount(string user, string password)
    {
        User = user;
        Password = password;
    }
}