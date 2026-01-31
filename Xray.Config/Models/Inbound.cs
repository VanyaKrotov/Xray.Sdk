using System.Text.Json;
using System.Text.Json.Serialization;
using Xray.Config.Enums;
using Xray.Config.Utilities;

namespace Xray.Config.Models;

/// <summary>
/// Incoming connections are used to receive data. Available protocols can be found in <see href="https://xtls.github.io/config/inbounds/">the Incoming Protocols section</see> section.
/// <para><see href="https://xtls.github.io/config/inbound.html">Docs</see></para>
/// </summary>
[JsonConverter(typeof(InboundConfigConverter))]
public abstract class Inbound
{
    /// <summary>
    /// Listening address, IP address, or Unix domain socket.
    /// <para>The default value is "0.0.0.0", which accepts connections on all network interfaces.</para>
    /// </summary>
    [JsonPropertyName("listen")]
    public string? Listen { get; set; }

    /// <summary>
    /// Port.
    /// </summary>
    [JsonPropertyName("port")]
    public required Port Port { get; set; }

    /// <summary>
    /// The name of the connection protocol. For a list of available protocols, see <see href="https://xtls.github.io/config/inbounds/">the Incoming Protocols section</see> in the left menu.
    /// </summary>
    [JsonPropertyName("protocol")]
    public InboundProtocol Protocol { get; set; }

    /// <summary>
    /// The tag for this incoming connection, used to identify this connection in other settings.
    /// </summary>
    [JsonPropertyName("tag")]
    public required string Tag { get; set; }

    /// <summary>
    /// Traffic detection is primarily used for transparent proxying and other purposes.
    /// </summary>
    [JsonPropertyName("sniffing")]
    public InboundSniffing? Sniffing { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("allocate")]
    public InboundAllocate? Allocate { get; set; }

    /// <summary>
    /// Transport type is the way the current Xray node interacts with other nodes.
    /// </summary>
    [JsonPropertyName("streamSettings")]
    public StreamSettings StreamSettings { get; set; } = new();

    protected Inbound(InboundProtocol protocol)
    {
        Protocol = protocol;
    }

    public class DokodemoDoorSettings : WithUserLevel
    {
        /// <summary>
        /// Redirect traffic to this address. This can be an IP address, such as "1.2.3.4", or a domain name, such as , "xray.com"by default "localhost".
        /// <para>If followRedirect(see below) is equal to true, then it addressmay be empty.</para>
        /// </summary>
        [JsonPropertyName("address")]
        public string? Address { get; set; }

        /// <summary>
        /// Redirects traffic to the specified port of the target address, range [0, 65535], numeric type. If empty or equal to 0, the listening address port is used by default.
        /// </summary>
        [JsonPropertyName("port")]
        public int? Port { get; set; }

        /// <summary>
        /// This is a mapping map between local ports and the required remote addresses/ports (if inbound listening on multiple ports). If a local port is not specified in this map, processing will be performed according to the address/ settings port.
        /// </summary>
        [JsonPropertyName("portMap")]
        public Dictionary<string, string>? PortMap { get; set; }

        /// <summary>
        /// Supported network protocol types. For example, if specified "tcp", only TCP traffic will be accepted. Default value: "tcp".
        /// </summary>
        [JsonPropertyName("network")]
        [JsonConverter(typeof(SplitEnumConverter<TransportProtocol>))]
        public List<TransportProtocol>? Network { get; set; }

        /// <summary>
        /// If the value is true, dokodemo-door will recognize data redirected by iptables and forward it to the appropriate target address.
        /// <para>See the setting tproxy in the <see href="https://xtls.github.io/config/transport.html#sockoptobject">Transport Configuration</see> section.</para>
        /// </summary>
        [JsonPropertyName("followRedirect")]
        public bool FollowRedirect { get; set; }
    }

    public class HttpSettings : WithUserLevel
    {
        /// <summary>
        /// An array where each element represents a user account. Default value: an empty array.
        /// <para>If accounts not empty, the HTTP proxy will perform Basic Authentication authentication for incoming connections.</para>
        /// </summary>
        [JsonPropertyName("accounts")]
        public List<HttpAccount> Accounts { get; set; } = new();

        /// <summary>
        /// If set true, all HTTP requests will be redirected, not just proxy requests.
        /// </summary>
        [JsonPropertyName("allowTransparent")]
        public bool AllowTransparent { get; set; }
    }

    public class ShadowSocksSettings : ShadowSocksCommon
    {
        /// <summary>
        /// ShadowSocks clients
        /// </summary>
        [JsonPropertyName("clients")]
        public List<ShadowSocksClient> Clients { get; set; } = new();

        /// <summary>
        /// Supported network protocol types. For example, if specified "tcp", only TCP traffic will be accepted. Default value: "tcp".
        /// </summary>
        [JsonPropertyName("network")]
        [JsonConverter(typeof(SplitEnumConverter<TransportProtocol>))]
        public List<TransportProtocol>? Network { get; set; }
    }

    public class SocksSettings : WithUserLevel
    {
        /// <summary>
        /// Authentication method for the Socks protocol. Both anonymous "noauth" and password-based methods are supported "password".
        /// <para>The default value is "noauth".</para>
        /// </summary>
        [JsonPropertyName("auth")]
        public SocksAuth Auth { get; set; } = SocksAuth.NoAuth;

        /// <summary>
        /// An array where each element represents a user account.
        /// <para>This parameter is only valid if the parameter auth is set to password.</para>
        /// <para>The default value is an empty array.</para>
        /// </summary>
        [JsonPropertyName("accounts")]
        public List<SocksAccount> Accounts { get; set; } = new();

        /// <summary>
        /// Whether to enable UDP protocol support.
        /// <para>The default value is false.</para>
        /// </summary>
        [JsonPropertyName("udp")]
        public bool Udp { get; set; }

        /// <summary>
        /// If UDP support is enabled, Xray must know the IP address of the local computer.
        /// <para>Warning : If you have multiple IP addresses configured on your machine, this may affect how inbound works when using UDP 0.0.0.0.</para>
        /// </summary>
        [JsonPropertyName("ip")]
        public string? Ip { get; set; }
    }

    public class TrojanSettings
    {
        /// <summary>
        /// An array representing a group of users approved by the server.
        /// </summary>
        [JsonPropertyName("clients")]
        public List<TrojanClient> Clients { get; set; } = new();

        /// <summary>
        /// An array containing a series of fallback routing configurations (optional). For details on configuring fallbacks, see <see href="https://xtls.github.io/en/config/features/fallback.html">FallbackObject</see>.
        /// </summary>
        [JsonPropertyName("fallbacks")]
        public List<Fallback>? Fallbacks { get; set; }
    }

    public class VlessSettings
    {
        /// <summary>
        /// An array representing a group of users approved by the server.
        /// </summary>
        [JsonPropertyName("clients")]
        public List<VlessClient> Clients { get; set; } = new();

        /// <summary>
        /// <see href="https://github.com/XTLS/Xray-core/pull/5067">VLESS encryption settings</see>. This cannot be empty; to disable it, you must explicitly set the value "none".
        /// </summary>
        [JsonPropertyName("decryption")]
        public VlessDecryption Decryption { get; set; } = VlessDecryption.None;

        /// <summary>
        /// An array containing a series of fallback routing configurations (optional). For details on configuring fallbacks, see FallbackObject.
        /// </summary>
        [JsonPropertyName("fallbacks")]
        public List<Fallback>? Fallbacks { get; set; }
    }

    public class VMessSettings
    {
        /// <summary>
        /// An array representing a group of users approved by the server.
        /// </summary>
        [JsonPropertyName("clients")]
        public List<VMessClient> Clients { get; set; } = new();

        /// <summary>
        /// Default configuration for clients. Only valid when used with detour.
        /// </summary>
        [JsonPropertyName("default")]
        public VMessDefaultSettings? Default { get; set; }
    }

    public class WireguardSettings
    {
        /// <summary>
        /// Private key.
        /// </summary>
        [JsonPropertyName("secretKey")]
        public required string SecretKey { get; set; }

        /// <summary>
        /// List of peer servers, each entry represents the configuration of one server.
        /// </summary>
        [JsonPropertyName("peers")]
        public List<WireguardPeer> Peers { get; set; } = new();

        /// <summary>
        /// Wireguard tun layer fragmentation size.
        /// </summary>
        [JsonPropertyName("mtu")]
        public int? Mtu { get; set; }
    }

    public class TUNSettings : WithUserLevel
    {
        /// <summary>
        /// The name of the created TUN interface. Default is "xray0".
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// The MTU of the interface. Default is 1500.
        /// </summary>
        [JsonPropertyName("MTU")]
        public int MTU { get; set; } = 1500;
    }
}

/// <summary>
/// Traffic detection is primarily used for transparent proxying and other purposes.
/// </summary>
public class InboundSniffing
{
    /// <summary>
    /// Enable traffic detection.
    /// </summary>
    [JsonPropertyName("enabled")]
    public bool Enabled { get; set; }

    /// <summary>
    /// Replace the current connection's target address with the specified types if the traffic matches them.
    /// </summary>
    [JsonPropertyName("destOverride")]
    public List<TrafficType>? DestOverride { get; set; }

    /// <summary>
    /// If this setting is enabled, only connection metadata will be used to detect the target address.
    /// </summary>
    [JsonPropertyName("metadataOnly")]
    public bool MetadataOnly { get; set; }

    /// <summary>
    /// [WIP] A list of domain names that will not be redirected if discovered via sniffing.
    /// </summary>
    [JsonPropertyName("domainsExcluded")]
    public List<string>? DomainsExcluded { get; set; }

    /// <summary>
    /// Use discovered domain names for routing only.
    /// <para>The proxy target remains the IP address.</para>
    /// <para>The default value is false.</para>
    /// </summary>
    [JsonPropertyName("routeOnly")]
    public bool RouteOnly { get; set; }
}

public class InboundAllocate
{
    [JsonPropertyName("strategy")]
    public AllocateStrategy? Strategy { get; set; }

    [JsonPropertyName("refresh")]
    public int? Refresh { get; set; }

    [JsonPropertyName("concurrency")]
    public int? Concurrency { get; set; }
}

// convertors
public class InboundConfigConverter : JsonConverter<Inbound>
{
    private static readonly Dictionary<InboundProtocol, Type> _protocolMap = new()
    {
        { InboundProtocol.DokodemoDoor, typeof(DokodemoDoorInbound) },
        { InboundProtocol.Http, typeof(HttpInbound) },
        { InboundProtocol.ShadowSocks, typeof(ShadowSocksInbound) },
        { InboundProtocol.Socks, typeof(SocksInbound) },
        { InboundProtocol.Trojan, typeof(TrojanInbound) },
        { InboundProtocol.Vless, typeof(VlessInbound) },
        { InboundProtocol.VMess, typeof(VMessInbound) },
        { InboundProtocol.Wireguard, typeof(WireguardInbound) },
        { InboundProtocol.Tun, typeof(TUNInbound) }
    };

    public override Inbound Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var protocol = doc.RootElement.GetProperty("protocol").Deserialize<InboundProtocol>(options);

        if (_protocolMap.TryGetValue(protocol, out var targetType))
        {
            return (Inbound)doc.Deserialize(targetType, options)!;
        }

        throw new JsonException($"Unsupported protocol: {protocol}");

    }

    public override void Write(Utf8JsonWriter writer, Inbound value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}

