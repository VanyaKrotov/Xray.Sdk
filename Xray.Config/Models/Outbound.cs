using System.Text.Json;
using System.Text.Json.Serialization;
using Xray.Config.Enums;

namespace Xray.Config.Models;

/// <summary>
/// Outgoing connections are used to send data. Available protocols can be found in <see href="https://xtls.github.io/config/outbounds">the Outgoing Protocols</see> section.
/// <para><see href="https://xtls.github.io/config/outbound.html">Docs</see></para>
/// </summary>
[JsonConverter(typeof(OutboundConfigConverter))]
public abstract class Outbound
{
    /// <summary>
    /// The IP address used to send data. This parameter is used if the host is configured with multiple IP addresses. The default value is "0.0.0.0".
    /// </summary>
    [JsonPropertyName("sendThrough")]
    public string? SendThrough { get; set; }

    /// <summary>
    /// The tag of this outgoing connection, used to identify this connection in other settings.
    /// </summary>
    [JsonPropertyName("tag")]
    public string? Tag { get; set; }

    /// <summary>
    /// The name of the connection protocol. For a list of available protocols, see <see href="https://xtls.github.io/config/outbounds">the Outgoing Protocols</see> section in the left-hand menu.
    /// </summary>
    [JsonPropertyName("protocol")]
    public OutboundProtocol Protocol { get; set; }

    /// <summary>
    /// Transport type is the way the current Xray node interacts with other nodes.
    /// </summary>
    [JsonPropertyName("streamSettings")]
    public StreamSettings? StreamSettings { get; set; }

    /// <summary>
    /// Outbound proxy configuration.
    /// </summary>
    [JsonPropertyName("proxySettings")]
    public ProxySettings? ProxySettings { get; set; }

    /// <summary>
    /// Mux settings. Mux allows you to multiplex multiple TCP connections over a single TCP connection. Mux has an additional feature: transmitting UDP connections as XUDP.
    /// </summary>
    [JsonPropertyName("mux")]
    public Mux? Mux { get; set; }

    /// <summary>
    /// When an outgoing connection sends a request to a domain name, this option controls whether and how it will be resolved to the outgoing IP address.
    /// <para>The default value is AsIs, meaning sending to the remote server "as is."</para>
    /// </summary>
    [JsonPropertyName("targetStrategy")]
    public DomainStrategy? TargetStrategy { get; set; }

    public Outbound(OutboundProtocol protocol)
    {
        Protocol = protocol;
    }

    public class BlackHoleSettings
    {
        /// <summary>
        /// Configuring Blackhole Response.
        /// </summary>
        [JsonPropertyName("response")]
        public BlackHoleResponse? Response { get; set; }
    }

    public class DnsSettings
    {
        /// <summary>
        /// Changes the transport protocol for DNS traffic. Valid values ​​are "tcp"and "udp". If not specified, the original transport protocol is used.
        /// </summary>
        [JsonPropertyName("network")]
        public TransportProtocol? Network { get; set; }

        /// <summary>
        /// Changes the DNS server address. If not specified, the address specified in the source is used.
        /// </summary>
        [JsonPropertyName("address")]
        public string? Address { get; set; }

        /// <summary>
        /// Changes the DNS server port. If not specified, the port specified in the source is used.
        /// </summary>
        [JsonPropertyName("port")]
        public int? Port { get; set; }

        /// <summary>
        /// Controls requests that are not for IP addresses (not A or AAAA).
        /// <para>The default value is "reject".</para>
        /// </summary>
        [JsonPropertyName("nonIPQuery")]
        public NonIPQueryType? NonIPQuery { get; set; }

        /// <summary>
        /// An array of integers ( int) specifying the DNS request types to block. For example, [ ] "blockTypes": [65,28] means blocking type 65 (HTTPS) and 28 (AAAA). A common use case is blocking type 65 to prevent browsers from initializing ECH.
        /// </summary>
        [JsonPropertyName("blockTypes")]
        public List<string>? BlockTypes { get; set; }
    }

    public class FreedomSettings : WithUserLevel
    {
        /// <summary>
        /// All parameters are similar in meaning domainStrategy to sockopt.
        /// <para>The default value is "AsIs".</para>
        /// </summary>
        [JsonPropertyName("domainStrategy")]
        public DomainStrategy? DomainStrategy { get; set; }

        /// <summary>
        /// Freedom will force all data to be sent to the specified address (not the address specified in the incoming connection).
        /// <para>The value is a string, for example: "127.0.0.1:80", ":1234".</para>
        /// </summary>
        [JsonPropertyName("redirect")]
        public string? Redirect { get; set; }

        /// <summary>
        /// Several key-value pairs used to control outgoing TCP fragmentation. In some cases, this can circumvent censorship systems, such as SNI blacklists. "length" These "interval"are of the Int32Range type.
        /// <list type="bullet">
        ///     <item>packets: two fragmentation modes are supported: "1-3" - TCP stream fragmentation, applied to the first three client-side data write operations; "tlshello" - TLS handshake packet fragmentation.</item>
        ///     <item>length: fragment length (in bytes).</item>
        ///     <item>interval: interval between fragments (in ms).</item>
        /// </list>
        /// <para>If the value is equal to 0and is set "packets": "tlshello", a fragmented Client Hello packet will be sent in a single TCP packet (unless its original size exceeds the MSS or MTU, which causes automatic fragmentation by the system).</para>
        /// </summary>
        [JsonPropertyName("fragment")]
        public Dictionary<string, string>? Fragment { get; set; }

        /// <summary>
        /// UDP noise, used to send random data as "noise" before establishing a UDP connection. The presence of this structure is considered enabled. It can fool sniffers, but it can also disrupt a normal connection. Use at your own risk. For this reason, it bypasses port 53, as this disrupts DNS.
        /// </summary>
        [JsonPropertyName("noises")]
        public List<Dictionary<string, string>>? Noises { get; set; }

        /// <summary>
        /// The PROXY protocol is typically used in conjunction with redirect to redirect to an Nginx server or another server with the PROXY protocol enabled. If the server doesn't support the PROXY protocol, the connection will be terminated.
        /// <para>proxyProtocol takes the value of the PROXY protocol version number - 1or 2. If not specified, the default value is 0(the protocol is not used).</para>
        /// </summary>
        [JsonPropertyName("proxyProtocol")]
        public int? ProxyProtocol { get; set; }
    }

    public class HttpSettings : WithLevel
    {
        /// <summary>
        /// HTTP proxy server address, required. 
        /// </summary>
        [JsonPropertyName("address")]
        public required string Address { get; set; }

        /// <summary>
        /// HTTP proxy server port, required.
        /// </summary>
        [JsonPropertyName("port")]
        public required int Port { get; set; }

        /// <summary>
        /// Username, string type. Required if authentication is required to connect to the server; otherwise, leave this option unchecked.
        /// </summary>
        [JsonPropertyName("user")]
        public string? User { get; set; }

        /// <summary>
        /// Password, string type. Required if authentication is required to connect to the server; otherwise, leave this option unchecked.
        /// </summary>
        [JsonPropertyName("pass")]
        public string? Password { get; set; }

        /// <summary>
        /// The email address used to identify the user. This is optional if authentication is required to connect to the server. Otherwise, leave this option unchecked.
        /// </summary>
        [JsonPropertyName("email")]
        public string? Email { get; set; }

        /// <summary>
        /// HTTP headers, which are key-value pairs. Each key represents the name of an HTTP header, and all key-value pairs will be attached to every request.
        /// </summary>
        [JsonPropertyName("headers")]
        public Dictionary<string, string>? Headers { get; set; }
    }

    public class LoopbackSettings
    {
        /// <summary>
        /// The incoming protocol identifier used for rerouting.
        /// </summary>
        [JsonPropertyName("inboundTag")]
        public required string InboundTag { get; set; }
    }

    public class ShadowSocksSettings : WithLevel
    {
        /// <summary>
        /// Email address, optional, used to identify the user.
        /// </summary>
        [JsonPropertyName("email")]
        public string? Email { get; set; }

        /// <summary>
        /// Shadowsocks server address; IPv4, IPv6, and domain names are supported. Required.
        /// </summary>
        [JsonPropertyName("address")]
        public string? Address { get; set; }

        /// <summary>
        /// Shadowsocks server port.
        /// </summary>
        [JsonPropertyName("port")]
        public int? Port { get; set; }

        /// <summary>
        /// Required parameter.
        /// </summary>
        [JsonPropertyName("method")]
        public EncryptionMethod? Method { get; set; }

        /// <summary>
        /// Required parameter.
        /// </summary>
        [JsonPropertyName("password")]
        public string? Password { get; set; }

        /// <summary>
        /// Turn on udp over tcp.
        /// </summary>
        [JsonPropertyName("uot")]
        public bool? Uot { get; set; }

        /// <summary>
        /// Implementation version UDP over TCP.
        /// <para>Valid values: 1, 2.</para>
        /// </summary>
        [JsonPropertyName("UoTVersion")]
        public int? UoTVersion { get; set; }

        [JsonPropertyName("servers")]
        public IEnumerable<ShadowSocksServer>? Servers { get; set; }
    }

    public class SocksSettings : WithLevel
    {
        /// <summary>
        /// Server address, required parameter.
        /// </summary>
        [JsonPropertyName("address")]
        public required string Address { get; set; }

        /// <summary>
        /// Server port, required parameter.
        /// </summary>
        [JsonPropertyName("port")]
        public required int Port { get; set; }

        /// <summary>
        /// Username, data type: string
        /// </summary>
        [JsonPropertyName("user")]
        public string? User { get; set; }

        /// <summary>
        /// Password, data type: string.
        /// </summary>
        [JsonPropertyName("pass")]
        public string? Password { get; set; }

        /// <summary>
        /// The email address used to identify the user.
        /// </summary>
        [JsonPropertyName("email")]
        public string? Email { get; set; }
    }

    public class TrojanSettings : WithLevel
    {
        /// <summary>
        /// Server address; IPv4, IPv6, and domain names are supported.
        /// </summary>
        [JsonPropertyName("address")]
        public string? Address { get; set; }

        /// <summary>
        /// The server port, usually the same as the port the server is listening on.
        /// </summary>
        [JsonPropertyName("port")]
        public int? Port { get; set; }

        /// <summary>
        /// Password.
        /// </summary>
        [JsonPropertyName("password")]
        public string? Password { get; set; }

        /// <summary>
        /// Email address, optional, used to identify the user.
        /// </summary>
        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("servers")]
        public IEnumerable<TrojanServer>? Servers { get; set; }
    }

    public class VlessSettings : WithLevel
    {
        /// <summary>
        /// Server address pointing to the server, domain names, IPv4 and IPv6 are supported.
        /// </summary>
        [JsonPropertyName("address")]
        public string? Address { get; set; }

        /// <summary>
        /// The server port, usually the same as the port the server is listening on.
        /// </summary>
        [JsonPropertyName("port")]
        public int? Port { get; set; }

        /// <summary>
        /// The VLESS user identifier can be any string less than 30 bytes long or a valid UUID.
        /// </summary>
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        /// <summary>
        /// VLESS encryption settings . This cannot be empty; to disable it, you must explicitly set the value "none".
        /// </summary>
        [JsonPropertyName("encryption")]
        public string? Encryption { get; set; }

        /// <summary>
        /// Flow control mode, used to select the XTLS algorithm.
        /// </summary>
        [JsonPropertyName("flow")]
        public XtlsFlow? Flow { get; set; }

        /// <summary>
        /// A simplified configuration for the VLESS reverse proxy. It serves the same purpose as the built-in universal reverse proxy, but with a simpler setup.
        /// </summary>
        [JsonPropertyName("reverse")]
        public VlessReverse? Reverse { get; set; }

        /// <summary>
        /// Client only. External server endpoints.
        /// </summary>
        [JsonPropertyName("vnext")]
        public IEnumerable<VlessVNext>? VNext { get; set; }
    }

    public class VMessSettings : WithLevel
    {
        /// <summary>
        /// Server address, IP address or domain name supported.
        /// </summary>
        [JsonPropertyName("address")]
        public string? Address { get; set; }

        /// <summary>
        /// The port number the server listens on is a required parameter.
        /// </summary>
        [JsonPropertyName("port")]
        public int? Port { get; set; }

        /// <summary>
        /// The VMess user ID, can be any string less than 30 bytes long or a valid UUID.
        /// </summary>
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        /// <summary>
        /// Encryption method. The client will send data using the configured encryption method; the server will automatically detect it; no configuration is required on the server.
        /// </summary>
        [JsonPropertyName("security")]
        public VMessSecurity? Security { get; set; }

        /// <summary>
        /// Enabled experimental features of the VMess protocol. (These features are unstable and may be removed at any time.) Multiple enabled experiments can be separated by the | symbol, for example, "AuthenticatedLength|NoTerminationSignal".
        /// <list type="bullet">
        ///     <item>AuthenticatedLength - enables the authenticated packet length experiment. This experiment must be enabled on both the client and server, and the same version of the program must be running.</item>
        ///     <item>NoTerminationSignal - enables an experiment with disabling the connection termination signal. This experiment may impact the stability of the proxied connection.</item>
        /// </list>
        /// </summary>
        [JsonPropertyName("experiments")]
        public string? Experiments { get; set; }

        [JsonPropertyName("vnext")]
        public IEnumerable<VMessVNext>? VNext { get; set; }
    }

    public class WireguardSettings
    {
        /// <summary>
        /// User's personal key. Required field.
        /// </summary>
        [JsonPropertyName("secretKey")]
        public required string SecretKey { get; set; }

        /// <summary>
        /// Wireguard launches a local virtual network interface (tun). It supports one or more IP addresses, including IPv6.
        /// </summary>
        [JsonPropertyName("address")]
        public List<string>? Address { get; set; }

        /// <summary>
        /// A list of Wireguard servers. Each entry represents the configuration of one server.
        /// </summary>
        [JsonPropertyName("peers")]
        public List<WireguardOutboundPeer>? Peers { get; set; }

        /// <summary>
        /// By default, the system checks whether it's running Linux and whether the user has CAP_NET_ADMIN privileges to decide whether to use the system virtual interface. If it's not used, gvisor is used. The system virtual interface provides better performance. Please note that this only applies to IP packet processing and is not related to the Wireguard kernel.
        /// </summary>
        [JsonPropertyName("noKernelTun")]
        public bool? NoKernelTun { get; set; }

        /// <summary>
        /// MTU of the lower level tun in Wireguard.
        /// </summary>
        [JsonPropertyName("mtu")]
        public int? Mtu { get; set; }

        /// <summary>
        /// Wireguard reserved bytes, filled as needed.
        /// </summary>
        [JsonPropertyName("reserved")]
        public List<int>? Reserved { get; set; }

        /// <summary>
        /// The number of Wireguard threads. The default value is equal to the number of processor cores.
        /// </summary>
        [JsonPropertyName("workers")]
        public int? Workers { get; set; }

        /// <summary>
        /// Unlike most proxy protocols, Wireguard does not allow domain names to be passed as targets. If a domain name is passed as a target, it is resolved to an IP address via Xray's built-in DNS. See the domainStrategyoutbound field for Freedomdetails. The default is ForceIP.
        /// </summary>
        [JsonPropertyName("domainStrategy")]
        public DomainStrategy? DomainStrategy { get; set; }
    }

    public class HysteriaSettings
    {
        /// <summary>
        /// Version of protocol
        /// </summary>
        [JsonPropertyName("version")]
        public int? Version { get; set; }

        /// <summary>
        /// Hysteria2 proxy server address.
        /// </summary>
        [JsonPropertyName("address")]
        public required string Address { get; set; }

        /// <summary>
        /// Hysteria2 proxy server port.
        /// </summary>
        [JsonPropertyName("port")]
        public required int Port { get; set; }
    }
}

/// <summary>
/// Outbound proxy configuration.
/// </summary>
public class ProxySettings
{
    /// <summary>
    /// If another Outbound tag is specified, data originating from that Outbound will be redirected through the specified Outbound.
    /// </summary>
    [JsonPropertyName("tag")]
    public string? Tag { get; set; }

    /// <summary>
    /// trueConverts this setting to SockOpt.dialerProxy support transport-level redirection. Default false(no conversion).
    /// </summary>
    [JsonPropertyName("transportLayer")]
    public bool TransportLayer { get; set; }
}

public class Mux
{
    [JsonPropertyName("enabled")]
    public bool? Enabled { get; set; }

    [JsonPropertyName("concurrency")]
    public int? Concurrency { get; set; }

    [JsonPropertyName("xudpConcurrency")]
    public int? XudpConcurrency { get; set; }

    [JsonPropertyName("xudpProxyUDP443")]
    public string? XudpProxyUDP443 { get; set; }
}

// convertors

class OutboundConfigConverter : JsonConverter<Outbound>
{
    private static readonly Dictionary<OutboundProtocol, Type> _protocolMap = new()
    {
        { OutboundProtocol.BlackHole, typeof(BlackHoleOutbound) },
        { OutboundProtocol.Dns, typeof(DnsOutbound) },
        { OutboundProtocol.Freedom, typeof(FreedomOutbound) },
        { OutboundProtocol.Http, typeof(HttpOutbound) },
        { OutboundProtocol.Loopback, typeof(LoopbackOutbound) },
        { OutboundProtocol.ShadowSocks, typeof(ShadowSocksOutbound) },
        { OutboundProtocol.Socks, typeof(SocksOutbound) },
        { OutboundProtocol.Trojan, typeof(TrojanOutbound) },
        { OutboundProtocol.Vless, typeof(VlessOutbound) },
        { OutboundProtocol.VMess, typeof(VMessOutbound) },
        { OutboundProtocol.Wireguard, typeof(WireguardOutbound) },
        { OutboundProtocol.Hysteria, typeof(HysteriaOutbound) },
    };

    public override Outbound Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var protocol = doc.RootElement.GetProperty("protocol").Deserialize<OutboundProtocol>(options);

        if (_protocolMap.TryGetValue(protocol, out var targetType))
        {
            return (Outbound?)doc.Deserialize(targetType, options)!;
        }

        throw new JsonException($"Unsupported protocol: {protocol}");

    }

    public override void Write(Utf8JsonWriter writer, Outbound value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}