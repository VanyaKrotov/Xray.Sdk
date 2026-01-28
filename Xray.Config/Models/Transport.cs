using System.Text.Json.Serialization;
using Xray.Config.Enums;

namespace Xray.Config.Models;

/// <summary>
/// Transport is the way the current Xray node interacts with other nodes.
/// <para>A transport defines how data is transmitted. Typically, both ends of a network connection must use the same transport. For example, if one end uses WebSocket, the other end must also use WebSocket, otherwise the connection will fail.</para>
/// <para><see href="https://xtls.github.io/config/transport.html">Docs</see></para>
/// </summary>
public class TransportConfig : StreamSettings { }

public class StreamSettings
{
    /// <summary>
    /// The type of transport method used by the connection's data stream, by default "raw".
    /// </summary>
    [JsonPropertyName("network")]
    public StreamNetwork Network { get; set; } = StreamNetwork.Raw;

    /// <summary>
    /// Whether transport layer encryption is enabled.
    /// </summary>
    [JsonPropertyName("security")]
    public StreamSecurity Security { get; set; } = StreamSecurity.None;

    /// <summary>
    /// TLS configuration. TLS is provided by Golang, and TLS negotiation typically results in TLS 1.3; DTLS is not supported.
    /// </summary>
    [JsonPropertyName("tlsSettings")]
    public TlsSettings? TlsSettings { get; set; }

    /// <summary>
    /// Reality Configuration. Reality is Xray's original technology. Reality provides a higher level of security than TLS and is configured in the same way as TLS.
    /// </summary>
    [JsonPropertyName("realitySettings")]
    public RealitySettings? RealitySettings { get; set; }

    /// <summary>
    /// RAW configuration for the current connection, valid only if that connection uses RAW.
    /// </summary>
    [JsonPropertyName("rawSettings")]
    public RawSettings? RawSettings { get; set; }

    [JsonInclude]
    [JsonPropertyName("tcpSettings")]
    private RawSettings? TcpSettings
    {
        set => RawSettings = value;
    }

    /// <summary>
    /// XHTTP configuration for the current connection, valid only if this connection uses XHTTP.
    /// </summary>
    [JsonPropertyName("xhttpSettings")]
    public XHttpSettings? XHttpSettings { get; set; }

    /// <summary>
    /// mKCP configuration for the current connection, valid only if this connection uses mKCP.
    /// </summary>
    [JsonPropertyName("kcpSettings")]
    public KcpSettings? KcpSettings { get; set; }

    /// <summary>
    /// gRPC configuration for the current connection, valid only if this connection uses gRPC.
    /// </summary>
    [JsonPropertyName("grpcSettings")]
    public GRPCSettings? GRPCSettings { get; set; }

    /// <summary>
    /// WebSocket configuration for the current connection, valid only if this connection uses WebSocket.
    /// </summary>
    [JsonPropertyName("wsSettings")]
    public WSSettings? WSSettings { get; set; }

    /// <summary>
    /// HTTPUpgrade configuration for the current connection, valid only if this connection uses HTTPUpgrade.
    /// </summary>
    [JsonPropertyName("httpupgradeSettings")]
    public HttpUpgradeSettings? HttpUpgradeSettings { get; set; }

    /// <summary>
    /// A low-level QUIC transport implementation for Hysteria2 in Xray.
    /// </summary>
    [JsonPropertyName("hysteriaSettings")]
    public HysteriaSettings? HysteriaSettings { get; set; }

    /// <summary>
    /// Specific settings related to transparent proxying.
    /// </summary>
    [JsonPropertyName("sockopt")]
    public Sockopt? Sockopt { get; set; }
}

public class Sockopt
{
    /// <summary>
    /// An integer. If the value is nonzero, the outgoing connection is marked with this value using SO_MARK.
    /// </summary>
    [JsonPropertyName("mark")]
    public int? Mark { get; set; }

    /// <summary>
    /// Used to set the maximum TCP packet segment (Maximum Segment Size).
    /// </summary>
    [JsonPropertyName("tcpMaxSeg")]
    public object? TcpMaxSeg { get; set; }

    /// <summary>
    /// Enable <see href="https://en.wikipedia.org/wiki/TCP_Fast_Open">TCP Fast Open</see>.
    /// <para>If the value is equal to true or a positive integer , TFO is enabled; if the value is equal to false or a negative number , TFO is forcibly disabled; if the parameter is absent or equal to 0, the system default settings are used. Can be used for both incoming and outgoing connections.</para>
    /// </summary>
    [JsonPropertyName("tcpFastOpen")]
    public int? TcpFastOpen { get; set; }

    /// <summary>
    /// If the target address is a domain name, you can configure the corresponding value. Default value: "AsIs".
    /// </summary>
    [JsonPropertyName("domainStrategy")]
    public DomainStrategy? DomainStrategy { get; set; }

    /// <summary>
    /// Whether to enable transparent proxying (Linux only).
    /// </summary>
    [JsonPropertyName("tproxy")]
    public TProxy TProxy { get; set; } = TProxy.Off;

    /// <summary>
    /// Happy Eyeballs implementation (RFC-8305), applicable only to TCP. When the target is a domain, a connection race is performed between the resulting IP addresses, and the first successful result is chosen.
    /// </summary>
    [JsonPropertyName("happyEyeballs")]
    public HappyEyeballs? HappyEyeballs { get; set; }

    /// <summary>
    /// The outbound proxy identifier. If the value is not empty, the specified outbound proxy will be used to establish the connection. This option can be used to support chained forwarding at the transport level.
    /// </summary>
    [JsonPropertyName("dialerProxy")]
    public string? DialerProxy { get; set; }

    /// <summary>
    /// For inbound only, specifies whether to accept the PROXY protocol.
    /// </summary>
    [JsonPropertyName("acceptProxyProtocol")]
    public bool AcceptProxyProtocol { get; set; }

    /// <summary>
    /// TCP idle timeout threshold in seconds. When a TCP connection's idle timeout reaches this threshold, Keep-Alive packets are sent.
    /// </summary>
    [JsonPropertyName("tcpKeepAliveIdle")]
    public int TcpKeepAliveIdle { get; set; }

    /// <summary>
    /// The interval (in seconds) between keep-alive packets sent after the TCP connection enters the Keep-Alive state. The remaining behavior is described above.
    /// </summary>
    [JsonPropertyName("tcpKeepAliveInterval")]
    public int TcpKeepAliveInterval { get; set; }

    /// <summary>
    /// In milliseconds.
    /// <para><see href="https://github.com/grpc/proposal/blob/master/A18-tcp-user-timeout.md">More details</see></para>
    /// </summary>
    [JsonPropertyName("tcpUserTimeout")]
    public int? TcpUserTimeout { get; set; }

    /// <summary>
    /// TCP congestion control algorithm. Supported only on Linux. If this parameter is not configured, the system default is used.
    /// </summary>
    [JsonPropertyName("tcpcongestion")]
    public TcpCongestion? TcpCongestion { get; set; }

    /// <summary>
    /// Specifies the network interface name for outgoing traffic. Supported on Linux, iOS, macOS, and Windows.
    /// </summary>
    [JsonPropertyName("interface")]
    public string? Interface { get; set; }

    /// <summary>
    /// By default, this setting is set to false. Set it to true to enable <see href="https://en.wikipedia.org/wiki/Multipath_TCP">Multipath TCP</see>.
    /// </summary>
    [JsonPropertyName("tcpMptcp")]
    public bool tcpMptcp { get; set; }

    /// <summary>
    ///If set to true, the address ::accepts only IPv6 connections. Supported only on Linux.
    /// </summary>
    [JsonPropertyName("V6Only")]
    public bool? V6Only { get; set; }

    /// <summary>
    /// This option has been removed because golang enables TCP no delay by default. If you want to disable it, use custom Sockopt.
    /// </summary>
    [JsonPropertyName("tcpNoDelay")]
    public bool tcpNoDelay { get; set; }

    /// <summary>
    /// The declared window size is limited to this value. The kernel will choose the maximum value between this value and SOCK_MIN_RCVBUF/2.
    /// </summary>
    [JsonPropertyName("tcpWindowClamp")]
    public bool TcpWindowClamp { get; set; }

    /// <summary>
    /// Use SRV or TXT records to determine the destination address/port for outgoing traffic. Default none (disabled).
    /// </summary>
    [JsonPropertyName("addressPortStrategy")]
    public AddressPortStrategy? AddressPortStrategy { get; set; }

    /// <summary>
    /// An array allowing advanced users to specify any necessary sockopt options. Theoretically, all of the above connection-related settings can be configured here. Currently, Linux, Windows, and Darwin operating systems are supported. The example below is equivalent "tcpcongestion": "bbr"in the kernel.
    /// </summary>
    [JsonPropertyName("customSockopt")]
    public List<CustomSockopt>? CustomSockopt { get; set; }
}

public class CustomSockopt
{
    /// <summary>
    /// An optional field. Specifies the operating system for which this option will be applied. If the current operating system does not match the specified one, this option (sockopt) will be skipped.
    /// </summary>
    [JsonPropertyName("system")]
    public OperatingSystem? System { get; set; }

    /// <summary>
    /// Required parameter. Setting type. Acceptable values: int or str.
    /// </summary>
    [JsonPropertyName("type")]
    public required string Type { get; set; }

    /// <summary>
    /// Optional parameter. The protocol level that determines the scope. Default: 6 (TCP).
    /// </summary>
    [JsonPropertyName("level")]
    public string? Level { get; set; }

    /// <summary>
    /// The name of the option to set. Uses decimal notation (in the example, the TCP_CONGESTION value defined as 0xd is converted to decimal 13).
    /// </summary>
    [JsonPropertyName("opt")]
    public string? Opt { get; set; }

    /// <summary>
    /// The value to set for the option. In the example, the value is set to bbr.
    /// <para>If type specified as int, the value must be a decimal number.</para>
    /// </summary>
    [JsonPropertyName("value")]
    public string? Value { get; set; }
}

/// <summary>
/// Happy Eyeballs implementation (RFC-8305), applicable only to TCP. When the target is a domain, a connection race is performed between the resulting IP addresses, and the first successful result is chosen.
/// </summary>
public class HappyEyeballs
{
    /// <summary>
    /// The time interval between each "race" request, in milliseconds. The default is 0 (meaning the feature is disabled); the recommended value is 250.
    /// </summary>
    [JsonPropertyName("tryDelayMs")]
    public int? TryDelayMs { get; set; }

    /// <summary>
    /// The type of the first IP address when sorting IP addresses. This is the default false(meaning IPv4 will be first).
    /// </summary>
    [JsonPropertyName("prioritizeIPv6")]
    public bool? PrioritizeIPv6 { get; set; }

    /// <summary>
    /// "First Address Family count" from RFC-8305, default value is 1. This parameter specifies the alternating order in which IP addresses of different versions are sorted.
    /// <para>For example, the IP address queue for dialing would be sorted as 46464646 (if the value is 1) or 44664466 (if the value is 2) (where 6 is the IPv6 address and 4 is the IPv4 address).</para>
    /// </summary>
    [JsonPropertyName("interleave")]
    public int? Interleave { get; set; }

    /// <summary>
    /// Maximum number of simultaneous attempts. This is used to prevent the kernel from creating a large number of connections if many IP addresses are allowed and none of the connections are successful. The default is 4; setting this value to 0 disables happyEyeballs.
    /// </summary>
    [JsonPropertyName("maxConcurrentTry")]
    public int? MaxConcurrentTry { get; set; }
}

/// <summary>
/// TLS configuration. TLS is provided by Golang, and TLS negotiation typically results in TLS 1.3; DTLS is not supported.
/// </summary>
public class TlsSettings
{
    /// <summary>
    /// Specifies the domain name of the server certificate, useful when connecting via an IP address.
    /// </summary>
    [JsonPropertyName("serverName")]
    public string? ServerName { get; set; }

    /// <summary>
    /// If the value is true, the server will reject the TLS handshake if the received SNI does not match the certificate's domain name. The default value is false.
    /// </summary>
    [JsonPropertyName("rejectUnknownSni")]
    public bool RejectUnknownSni { get; set; }

    /// <summary>
    /// Client-only. The SNI list used for certificate validation (at least one SAN from the certificate must be in this list). This list overrides the list serverName used for validation and is intended for special purposes, such as domain fronting. Compared to the previous method of changing serverName and enabling allowInsecure, this method is more secure, as it still performs certificate signature verification.
    /// </summary>
    [JsonPropertyName("verifyPeerCertInNames")]
    public List<string>? VerifyPeerCertInNames { get; set; }

    /// <summary>
    /// An array of strings specifying the ALPN values ​​specified during the TLS handshake. Default value: ["h2", "http/1.1"].
    /// </summary>
    [JsonPropertyName("alpn")]
    public List<string>? Alpn { get; set; }

    /// <summary>
    /// This is the minimum acceptable TLS version.
    /// </summary>
    [JsonPropertyName("minVersion")]
    public string? MinVersion { get; set; }

    /// <summary>
    /// This is the maximum allowed TLS version.
    /// </summary>
    [JsonPropertyName("maxVersion")]
    public string? MaxVersion { get; set; }

    /// <summary>
    /// Whether to allow insecure connections (client only). Default value: false.
    /// <para>If the value is true, Xray will not check the validity of the TLS certificate provided by the remote host.</para>
    /// </summary>
    [JsonPropertyName("allowInsecure")]
    public bool AllowInsecure { get; set; }

    /// <summary>
    /// Used to configure a colon-separated list of supported cipher suites.
    /// </summary>
    [JsonPropertyName("cipherSuites")]
    public string? CipherSuites { get; set; }

    /// <summary>
    /// A list of certificates, each element of which represents a certificate (fullchain recommended).
    /// </summary>
    [JsonPropertyName("certificates")]
    public List<TlsCertificate>? Certificates { get; set; } = new();

    /// <summary>
    /// Whether to disable operating system root certificates. Default value: false.
    /// <para>If the value is true, Xray will only use certificates specified in certificates for the TLS handshake. If the value is false, Xray will only use operating system root certificates for the TLS handshake.</para>
    /// </summary>
    [JsonPropertyName("disableSystemRoot")]
    public bool DisableSystemRoot { get; set; }

    /// <summary>
    /// Whether to enable session restore. Disabled by default, it will only work if both the server and client support and have enabled this feature.
    /// </summary>
    [JsonPropertyName("enableSessionResumption")]
    public bool EnableSessionResumption { get; set; }

    /// <summary>
    /// This parameter is used to customize the specified fingerprint TLS Client Hello. Default value chrome.
    /// </summary>
    [JsonPropertyName("fingerprint")]
    public Fingerprint? Fingerprint { get; set; }

    /// <summary>
    /// Used to specify the SHA256 hash of a remote server's certificate; hex format, case-insensitive.
    /// <para>For example: e8e2d387fdbffeb38e9c9065cf30a97ee23c0e3d32ee6f78ffae40966befccc9. This encoding matches the SHA-256 certificate fingerprint in the Chrome certificate viewer, as well as the Certificate Fingerprints SHA-256 format in crt.sh.</para>
    /// </summary>
    [JsonPropertyName("pinnedPeerCertSha256")]
    public string? PinnedPeerCertSha256 { get; set; }

    /// <summary>
    /// An array of strings specifying the preferred curves for ECDHE execution during the TLS handshake. The supported curves are listed below (case insensitive): CurveP256 CurveP384 CurveP521 X25519 x25519Kyber768Draft00
    /// <para>For example, setting a value "curvePreferences":["x25519Kyber768Draft00"]enables an experimental algorithm. Since this algorithm is still in draft form, this field may change at any time.</para>
    /// </summary>
    [JsonPropertyName("curvePreferences")]
    public List<string>? CurvePreferences { get; set; }

    /// <summary>
    /// The (Pre)-Master-Secret log file, the path to which is specified here, can be used in Wireshark and other programs to decrypt TLS connections established by Xray.
    /// </summary>
    [JsonPropertyName("masterKeyLog")]
    public string? MasterKeyLog { get; set; }

    /// <summary>
    /// Client-only. Set by ECHConfig; if set, the client enables the Encrypted Client Hello. Two formats are supported.
    /// </summary>
    [JsonPropertyName("echConfigList")]
    public string? EchConfigList { get; set; }

    /// <summary>
    /// Server-only. Enables Encrypted Client Hello on the server side.
    /// <para>Create keys with the command xray tls ech --serverName example.com [where example.com] is the SNI that will be exposed externally (you can specify any). The Server Key also contains the ECHConfig; if the client Config is lost, it can be restored with [] xray tls ech -i "you server key".</para>
    /// </summary>
    [JsonPropertyName("echServerKeys")]
    public string? EchServerKeys { get; set; }

    /// <summary>
    /// Controls the policy when using DNS queries for ECH Config, the options available are none(default), half, full.
    /// </summary>
    [JsonPropertyName("echForceQuery")]
    public string? EchForceQuery { get; set; }

    /// <summary>
    /// Configures the socket connection-based settings used when performing DNS queries for records ECH.
    /// </summary>
    [JsonPropertyName("echSockopt")]
    public Sockopt? EchSockopt { get; set; }
}

/// <summary>
/// The server certificate will be automatically reloaded every 3600 seconds (that is, every hour).
/// </summary>
public class TlsCertificate
{
    /// <summary>
    /// OCSP Stapling refresh interval in seconds, defaults to 0. Any non-zero value will enable OCSP Stapling and override the default certificate warm reload time of 3600 seconds (OCSP Stapling is performed during reboot).
    /// </summary>
    [JsonPropertyName("ocspStapling")]
    public int? OcspStapling { get; set; }

    /// <summary>
    /// Download only once (default false). If set to true, the certificate hot reload and OCSP stapling features will be disabled.
    /// </summary>
    [JsonPropertyName("oneTimeLoading")]
    public bool? OneTimeLoading { get; set; }

    /// <summary>
    /// Certificate usage, default value: "encipherment".
    /// </summary>
    [JsonPropertyName("usage")]
    public CertificateUsageType? Usage { get; set; }

    /// <summary>
    /// Only effective when the certificate is in use "issue", if the value is true, the CA certificate will be embedded in the certificate chain when the certificate is issued.
    /// </summary>
    [JsonPropertyName("buildChain")]
    public bool? BuildChain { get; set; }

    /// <summary>
    /// Path to a certificate file, such as one generated using OpenSSL, with a .crt extension.
    /// </summary>
    [JsonPropertyName("certificateFile")]
    public string? CertificateFile { get; set; }

    /// <summary>
    /// The path to a key file, such as one generated using OpenSSL, with the .key extension. Password-protected key files are currently not supported.
    /// </summary>
    [JsonPropertyName("keyFile")]
    public string? KeyFile { get; set; }

    /// <summary>
    /// An array of strings representing the certificate contents, see the example for the format. Use either certificate or certificateFile.
    /// </summary>
    [JsonPropertyName("certificate")]
    public List<string>? Certificate { get; set; }

    /// <summary>
    /// An array of strings representing the key's contents; see the example for the format. Use either key or keyFile.
    /// </summary>
    [JsonPropertyName("key")]
    public List<string>? Key { get; set; }
}
