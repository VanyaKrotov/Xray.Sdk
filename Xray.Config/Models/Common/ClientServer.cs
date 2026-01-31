using System.Text.Json.Serialization;

namespace Xray.Config.Models;

public abstract class ClientServer
{
    /// <summary>
    /// Server address; IPv4, IPv6, and domain names are supported.
    /// </summary>
    [JsonPropertyName("address")]
    public required string Address { get; set; }

    /// <summary>
    /// The server port, usually the same as the port the server is listening on.
    /// </summary>
    [JsonPropertyName("port")]
    public required int Port { get; set; }

    /// <summary>
    /// Email address, optional, used to identify the user.
    /// </summary>
    [JsonPropertyName("email")]
    public string? Email { get; set; }

    /// <summary>
    /// Password. Required, any string.
    /// </summary>
    [JsonPropertyName("password")]
    public required string Password { get; set; }
}