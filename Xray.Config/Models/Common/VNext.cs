using System.Text.Json.Serialization;

namespace Xray.Config.Models;

public abstract class VNextModel<T> where T : WithLevel
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

    [JsonPropertyName("users")]
    public required IEnumerable<T> Users { get; set; }
}