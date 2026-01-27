using System.Collections.Specialized;
using System.Text.Json.Serialization;

namespace Xray.Config.Models;

/// <summary>
/// WebSocket configuration for the current connection, valid only if this connection uses WebSocket.
/// </summary>
public class WSSettings
{
    /// <summary>
    /// For incoming connections only, specifies whether to accept the PROXY protocol.
    /// <para>If set true, then after establishing a TCP connection at the lowest level, the requesting party must first send PROXY protocol v1 or v2, otherwise the connection will be closed.</para>
    /// </summary>
    [JsonPropertyName("acceptProxyProtocol")]
    public bool? AcceptProxyProtocol { get; set; }

    /// <summary>
    /// The path used by WebSocket in the HTTP protocol, the default value is "/".
    /// </summary>
    [JsonPropertyName("path")]
    public string? Path { get; set; }

    /// <summary>
    /// The host sent in the WebSocket HTTP request; the default value is empty. If the server-side value is empty, the host value sent by the client is not checked.
    /// </summary>
    [JsonPropertyName("host")]
    public string? Host { get; set; }

    /// <summary>
    /// Custom HTTP headers are key-value pairs where each key represents the name of an HTTP header and the corresponding value is a string.
    /// <para>Default value: empty.</para>
    /// </summary>
    [JsonPropertyName("headers")]
    public NameValueCollection Headers { get; set; } = new();

    /// <summary>
    /// Specifies the time interval for sending Ping messages to maintain a connection. If not specified or set to 0, Ping messages are not sent (the current behavior is the default).
    /// </summary>
    [JsonPropertyName("heartbeatPeriod")]
    public int? HeartbeatPeriod { get; set; }
}