using System.Net.Http.Headers;
using System.Text.Json.Serialization;

namespace Xray.Config.Models;

/// <summary>
/// HTTPUpgrade configuration for the current connection, valid only if this connection uses HTTPUpgrade.
/// </summary>
public class HttpUpgradeSettings
{
    /// <summary>
    /// Used only for incoming connections and specifies whether to accept the PROXY protocol.
    /// </summary>
    [JsonPropertyName("acceptProxyProtocol")]
    public bool? AcceptProxyProtocol { get; set; }

    /// <summary>
    /// The HTTP path used by HTTPUpgrade, by default "/".
    /// </summary>
    [JsonPropertyName("path")]
    public string? Path { get; set; }

    /// <summary>
    /// The host sent in the HTTPUpgrade HTTP request is empty by default. If the server-side value is empty, the host value sent by the client is not validated.
    /// </summary>
    [JsonPropertyName("host")]
    public string? Host { get; set; }

    /// <summary>
    /// Custom HTTP headers, a key-value pair where each key represents the name of an HTTP header and the corresponding value is a string.
    /// <para>Empty by default.</para>
    /// </summary>
    [JsonPropertyName("headers")]
    public HttpHeaders? Headers { get; set; }
}