using System.Text.Json.Serialization;
using Xray.Config.Enums;

namespace Xray.Config.Models;

/// <summary>
/// An implementation of the standard Socks protocol, compatible with <see href="http://ftp.icm.edu.pl/packages/socks/socks4/SOCKS4.protocol">Socks 4</see>, <see href="https://ftp.icm.edu.pl/packages/socks/socks4/SOCKS4A.protocol">Socks 4a</see>, Socks 5, and HTTP.
/// </summary>
public class SocksInbound : Inbound
{
    public SocksInbound() : base(InboundProtocol.Socks) { }

    [JsonPropertyName("settings")]
    public SocksSettings Settings { get; set; } = new();
}

public class SocksAccount
{
    /// <summary>
    /// Username, type - string.
    /// </summary>
    [JsonPropertyName("user")]
    public required string User { get; set; }

    /// <summary>
    /// Password, type - string.
    /// </summary>
    [JsonPropertyName("pass")]
    public required string Password { get; set; }

    public SocksAccount(string user, string password)
    {
        User = user;
        Password = password;
    }
}