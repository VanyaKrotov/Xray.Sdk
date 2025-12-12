using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using Xray.Config.Enums;

namespace Xray.Config.Models;

public class TransportConfig : StreamSettings { }

public class StreamSettings
{
    [JsonPropertyName("network")]
    public StreamNetwork Network { get; set; } = StreamNetwork.Raw;

    [JsonPropertyName("security")]
    public StreamSecurity? Security { get; set; }

    [JsonPropertyName("tlsSettings")]
    public TlsSettings? TlsSettings { get; set; }

    [JsonPropertyName("realitySettings")]
    public RealitySettings? RealitySettings { get; set; }

    [JsonPropertyName("rawSettings")]
    public RawSettings? RawSettings { get; set; }

    [JsonPropertyName("xhttpSettings")]
    public XHttpSettings? XHttpSettings { get; set; }

    [JsonPropertyName("kcpSettings")]
    public KcpSettings? KcpSettings { get; set; }

    [JsonPropertyName("grpcSettings")]
    public GRPCSettings? GRPCSettings { get; set; }

    [JsonPropertyName("wsSettings")]
    public WSSettings? WSSettings { get; set; }

    [JsonPropertyName("httpupgradeSettings")]
    public HttpUpgradeSettings? HttpUpgradeSettings { get; set; }

    [JsonPropertyName("sockopt")]
    public Sockopt? Sockopt { get; set; }
}

public class Sockopt
{
    [JsonPropertyName("mark")]
    public int? Mark { get; set; }

    [JsonPropertyName("tcpFastOpen")]
    public int? TcpFastOpen { get; set; }

    [JsonPropertyName("domainStrategy")]
    public DomainStrategy? DomainStrategy { get; set; }

    [JsonPropertyName("tproxy")]
    public TProxy TProxy { get; set; } = TProxy.Off;

    [JsonPropertyName("happyEyeballs")]
    public HappyEyeballs? happyEyeballs { get; set; }

    [JsonPropertyName("dialerProxy")]
    public string? DialerProxy { get; set; }

    [JsonPropertyName("acceptProxyProtocol")]
    public bool AcceptProxyProtocol { get; set; }

    [JsonPropertyName("tcpKeepAliveInterval")]
    public int TcpKeepAliveInterval { get; set; }

    [JsonPropertyName("tcpcongestion")]
    public TcpCongestion? TcpCongestion { get; set; }

    [JsonPropertyName("interface")]
    public string? Interface { get; set; }

    [JsonPropertyName("tcpMptcp")]
    public bool tcpMptcp { get; set; }

    [JsonPropertyName("tcpNoDelay")]
    public bool tcpNoDelay { get; set; }
}

public class HappyEyeballs
{
    [JsonPropertyName("tryDelayMs")]
    public int? TryDelayMs { get; set; }

    [JsonPropertyName("prioritizeIPv6")]
    public bool? PrioritizeIPv6 { get; set; }

    [JsonPropertyName("interleave")]
    public int? Interleave { get; set; }

    [JsonPropertyName("maxConcurrentTry")]
    public int? MaxConcurrentTry { get; set; }
}

public class TlsSettings
{
    [JsonPropertyName("serverName")]
    public string? ServerName { get; set; }

    [JsonPropertyName("rejectUnknownSni")]
    public bool RejectUnknownSni { get; set; }

    [JsonPropertyName("verifyPeerCertInNames")]
    public List<string>? VerifyPeerCertInNames { get; set; }

    [JsonPropertyName("allowInsecure")]
    public bool AllowInsecure { get; set; }

    [JsonPropertyName("alpn")]
    public List<string>? Alpn { get; set; }

    [JsonPropertyName("minVersion")]
    public string? MinVersion { get; set; }

    [JsonPropertyName("maxVersion")]
    public string? MaxVersion { get; set; }

    [JsonPropertyName("cipherSuites")]
    public string? cipherSuites { get; set; }

    [JsonPropertyName("certificates")]
    public List<TlsCertificate>? Certificates { get; set; } = new();

    [JsonPropertyName("disableSystemRoot")]
    public bool DisableSystemRoot { get; set; }

    [JsonPropertyName("enableSessionResumption")]
    public bool EnableSessionResumption { get; set; }

    [JsonPropertyName("fingerprint")]
    public Fingerprint? Fingerprint { get; set; }

    [JsonPropertyName("pinnedPeerCertificateChainSha256")]
    public List<string>? PinnedPeerCertificateChainSha256 { get; set; }

    [JsonPropertyName("masterKeyLog")]
    public string? MasterKeyLog { get; set; }
}

public class TlsCertificate
{
    [JsonPropertyName("ocspStapling")]
    public int? OcspStapling { get; set; }

    [JsonPropertyName("oneTimeLoading")]
    public bool? OneTimeLoading { get; set; }

    [JsonPropertyName("usage")]
    public CertificateUsageType? Usage { get; set; }

    [JsonPropertyName("buildChain")]
    public bool? BuildChain { get; set; }

    [JsonPropertyName("certificateFile")]
    public string? CertificateFile { get; set; }

    [JsonPropertyName("keyFile")]
    public string? KeyFile { get; set; }

    [JsonPropertyName("certificate")]
    public List<string>? Certificate { get; set; }

    [JsonPropertyName("key")]
    public List<string>? Key { get; set; }
}

public class RealitySettings
{
    [JsonPropertyName("show")]
    public bool Show { get; set; } = true;

    [JsonPropertyName("dest")]
    public string? Dest { get; set; }

    [JsonPropertyName("xver")]
    public int? XVer { get; set; }

    [JsonPropertyName("serverNames")]
    public List<string>? ServerNames { get; set; }

    [JsonPropertyName("privateKey")]
    public string? PrivateKey { get; set; }

    [JsonPropertyName("minClientVer")]
    public string? MinClientVer { get; set; }

    [JsonPropertyName("maxClientVer")]
    public string? MaxClientVer { get; set; }

    [JsonPropertyName("maxTimeDiff")]
    public int? MaxTimeDiff { get; set; }

    [JsonPropertyName("shortIds")]
    public List<string>? ShortIds { get; set; }

    [JsonPropertyName("fingerprint")]
    public Fingerprint? Fingerprint { get; set; }

    [JsonPropertyName("serverName")]
    public string? ServerName { get; set; }

    [JsonPropertyName("publicKey")]
    public required string PublicKey { get; set; }

    [JsonPropertyName("shortId")]
    public string? ShortId { get; set; }

    [JsonPropertyName("spiderX")]
    public string? SpiderX { get; set; }
}

public class RawSettings
{
    [JsonPropertyName("acceptProxyProtocol")]
    public bool AcceptProxyProtocol { get; set; }

    [JsonPropertyName("header")]
    public SettingsHeaders? Header { get; set; }
}

public abstract class SettingsHeaders
{
    [JsonPropertyName("type")]
    public RawHeadersType Type { get; set; } = RawHeadersType.None;

    [JsonPropertyName("request")]
    public HttpRequest? Request { get; set; }

    [JsonPropertyName("response")]
    public HttpRequest? HttpResponse { get; set; }
}

public class HttpRequest
{
    [JsonPropertyName("version")]
    public string? Version { get; set; }

    [JsonPropertyName("method")]
    public string? Method { get; set; }

    [JsonPropertyName("path")]
    public List<string>? Path { get; set; }

    [JsonPropertyName("headers")]
    public HttpHeaders? Headers { get; set; }
}

public class HttpResponse
{
    [JsonPropertyName("version")]
    public string? Version { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("reason")]
    public string? Reason { get; set; }

    [JsonPropertyName("headers")]
    public HttpHeaders? Headers { get; set; }
}

public class XHttpSettings
{
    [JsonPropertyName("host")]
    public string? Host { get; set; }

    [JsonPropertyName("path")]
    public string? Path { get; set; }

    [JsonPropertyName("mode")]
    public string? Mode { get; set; }

    [JsonPropertyName("extra")]
    public XHttpExtraSettings? Extra { get; set; }
}

public class XHttpExtraSettings
{
    [JsonPropertyName("header")]
    public Dictionary<string, string> Headers { get; set; } = new();

    [JsonPropertyName("xPaddingBytes")]
    public string? XPaddingBytes { get; set; }

    [JsonPropertyName("noGRPCHeader")]
    public bool? NoGRPCHeader { get; set; }

    [JsonPropertyName("noSSEHeader")]
    public bool? NoSSEHeader { get; set; }

    [JsonPropertyName("scMaxEachPostBytes")]
    public long? ScMaxEachPostBytes { get; set; }

    [JsonPropertyName("scMinPostsIntervalMs")]
    public int? ScMinPostsIntervalMs { get; set; }

    [JsonPropertyName("scMaxBufferedPosts")]
    public int? ScMaxBufferedPosts { get; set; }

    [JsonPropertyName("scStreamUpServerSecs")]
    public string? ScStreamUpServerSecs { get; set; }

    [JsonPropertyName("xmux")]
    public XMux? XMux { get; set; }

    [JsonPropertyName("downloadSettings")]
    public XHttpDownloadSettings? DownloadSettings { get; set; }
}

public class XMux
{
    [JsonPropertyName("maxConcurrency")]
    public string? MaxConcurrency { get; set; }

    [JsonPropertyName("maxConnections")]
    public int? MaxConnections { get; set; }

    [JsonPropertyName("cMaxReuseTimes")]
    public int? CMaxReuseTimes { get; set; }

    [JsonPropertyName("hMaxRequestTimes")]
    public string? HMaxRequestTimes { get; set; }

    [JsonPropertyName("hMaxReusableSecs")]
    public string? HMaxReusableSecs { get; set; }

    [JsonPropertyName("hKeepAlivePeriod")]
    public int? HKeepAlivePeriod { get; set; }
}

public class XHttpDownloadSettings : StreamSettings
{
    [JsonPropertyName("address")]
    public string? Address { get; set; }

    [JsonPropertyName("port")]
    public int? Port { get; set; }
}

public class KcpSettings
{
    [JsonPropertyName("mtu")]
    public int? Mtu { get; set; }

    [JsonPropertyName("tti")]
    public int? Tti { get; set; }

    [JsonPropertyName("uplinkCapacity")]
    public int? UplinkCapacity { get; set; }

    [JsonPropertyName("downlinkCapacity")]
    public int? DownlinkCapacity { get; set; }

    [JsonPropertyName("congestion")]
    public bool? Congestion { get; set; }

    [JsonPropertyName("readBufferSize")]
    public int? ReadBufferSize { get; set; }

    [JsonPropertyName("writeBufferSize")]
    public int? WriteBufferSize { get; set; }

    [JsonPropertyName("header")]
    public SettingsHeaders? Header { get; set; }

    [JsonPropertyName("seed")]
    public string? Seed { get; set; }
}

public class GRPCSettings
{
    [JsonPropertyName("serviceName")]
    public string? ServiceName { get; set; }

    [JsonPropertyName("multiMode")]
    public bool MultiMode { get; set; }

    [JsonPropertyName("idle_timeout")]
    public int? IdleTimeout { get; set; }

    [JsonPropertyName("health_check_timeout")]
    public int? HealthCheckTimeout { get; set; }

    [JsonPropertyName("permit_without_stream")]
    public bool PermitWithoutStream { get; set; }

    [JsonPropertyName("initial_windows_size")]
    public int? InitialWindowsSize { get; set; }
}

public class WSSettings
{
    [JsonPropertyName("acceptProxyProtocol")]
    public bool? AcceptProxyProtocol { get; set; }

    [JsonPropertyName("path")]
    public string? Path { get; set; }

    [JsonPropertyName("host")]
    public string? Host { get; set; }

    [JsonPropertyName("headers")]
    public Dictionary<string, string>? Headers { get; set; }
}

public class HttpUpgradeSettings
{
    [JsonPropertyName("acceptProxyProtocol")]
    public bool? AcceptProxyProtocol { get; set; }

    [JsonPropertyName("path")]
    public string? Path { get; set; }

    [JsonPropertyName("host")]
    public string? Host { get; set; }

    [JsonPropertyName("headers")]
    public Dictionary<string, string> Headers { get; set; } = new();
}
