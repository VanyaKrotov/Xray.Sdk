using System.Text.Json.Serialization;

namespace Xray.Config.Models;

/// <summary>
/// Fallback provides Xray with a high degree of protection against active probing and has a unique first packet reservation mechanism.
/// </summary>
public class Fallback
{
    /// <summary>
    /// Attempt to match TLS SNI (Server Name Indication), any value or empty string, defaults to "".
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Attempt to match the result of the TLS ALPN negotiation, any value or the empty string, defaults to "".
    /// </summary>
    [JsonPropertyName("alpn")]
    public string? Alpn { get; set; }

    /// <summary>
    /// Attempt to match HTTP path of first packet, any value or empty string, default is empty string, if not empty must start with /, h2c is not supported.
    /// </summary>
    [JsonPropertyName("path")]
    public string? Path { get; set; }

    /// <summary>
    /// Specifies where TCP traffic is redirected after TLS decryption, two types of addresses are currently supported (this field is required, otherwise the launch will fail):
    /// <list type="bullet">
    ///     <item>TCP, format "addr:port", where addr supports IPv4, domain name, IPv6, if domain name is specified, TCP connection will be established directly (without using built-in DNS).</item>
    ///     <item>Unix domain socket, format - absolute path, for example, "/dev/shm/domain.socket", at the beginning you can add @to denote abstract , @@ - to denote abstract with filling.</item>
    /// </list>
    /// <para>Note: Starting with version v25.7.26.</para>
    /// </summary>
    [JsonPropertyName("dest")]
    public int? Dest { get; set; }

    /// <summary>
    /// Sending <see href="https://www.haproxy.org/download/2.2/doc/proxy-protocol.txt">the PROXY protocol</see>, specifically for transmitting the real source IP address and port of the request, is filled with version 1 or 2; the default is 0, meaning it is not sent. If necessary, it is recommended to specify 1.
    /// </summary>
    [JsonPropertyName("xver")]
    public int? XVer { get; set; }
}