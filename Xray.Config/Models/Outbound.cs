using System.Text.Json;
using System.Text.Json.Serialization;
using Xray.Config.Enums;

namespace Xray.Config.Models;

[JsonConverter(typeof(OutboundConfigConverter))]
public abstract class Outbound
{
    [JsonPropertyName("sendThrough")]
    public string? SendThrough { get; set; }

    [JsonPropertyName("tag")]
    public string? Tag { get; set; }

    [JsonPropertyName("protocol")]
    public OutboundProtocol Protocol { get; set; }

    [JsonPropertyName("streamSettings")]
    public StreamSettings? StreamSettings { get; set; }

    [JsonPropertyName("proxySettings")]
    public ProxySettings? ProxySettings { get; set; }

    [JsonPropertyName("mux")]
    public Mux? mux { get; set; }

    public Outbound(OutboundProtocol protocol)
    {
        Protocol = protocol;
    }

    public class BlackHoleSettings
    {
        [JsonPropertyName("response")]
        public BlackHoleResponse? Response { get; set; }
    }

    public class DnsSettings
    {
        [JsonPropertyName("network")]
        public TransportProtocol? Network { get; set; }

        [JsonPropertyName("address")]
        public string? Address { get; set; }

        [JsonPropertyName("port")]
        public int? Port { get; set; }

        [JsonPropertyName("nonIPQuery")]
        public NonIPQueryType? NonIPQuery { get; set; }

        [JsonPropertyName("blockTypes")]
        public List<string>? BlockTypes { get; set; }

    }

    public class FreedomSettings
    {
        [JsonPropertyName("domainStrategy")]
        public DomainStrategy? DomainStrategy { get; set; }

        [JsonPropertyName("redirect")]
        public string? Redirect { get; set; }

        [JsonPropertyName("userLevel")]
        public int UserLevel { get; set; } = 0;

        [JsonPropertyName("fragment")]
        public Dictionary<string, string>? Fragment { get; set; }

        [JsonPropertyName("noises")]
        public List<Dictionary<string, string>>? Noises { get; set; }

        [JsonPropertyName("proxyProtocol")]
        public int? ProxyProtocol { get; set; }
    }

    public class HttpSettings
    {
        [JsonPropertyName("address")]
        public string? Address { get; set; }

        [JsonPropertyName("port")]
        public int? Port { get; set; }

        [JsonPropertyName("user")]
        public string? User { get; set; }

        [JsonPropertyName("pass")]
        public string? Pass { get; set; }

        [JsonPropertyName("level")]
        public int Level { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("headers")]
        public Dictionary<string, string>? Headers { get; set; }
    }

    public class LoopbackSettings
    {
        [JsonPropertyName("inboundTag")]
        public required string InboundTag { get; set; }
    }

    public class ShadowSocksSettings
    {
        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("address")]
        public string? Address { get; set; }

        [JsonPropertyName("port")]
        public int? Port { get; set; }

        [JsonPropertyName("method")]
        public EncryptionMethod? Method { get; set; }

        [JsonPropertyName("password")]
        public string? Password { get; set; }

        [JsonPropertyName("uot")]
        public bool? Uot { get; set; }

        [JsonPropertyName("UoTVersion")]
        public int? UoTVersion { get; set; }

        [JsonPropertyName("level")]
        public int Level { get; set; }
    }

    public class SocksSettings
    {
        [JsonPropertyName("address")]
        public string? Address { get; set; }

        [JsonPropertyName("port")]
        public int? Port { get; set; }

        [JsonPropertyName("user")]
        public string? User { get; set; }

        [JsonPropertyName("pass")]
        public string? Pass { get; set; }

        [JsonPropertyName("level")]
        public int Level { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }
    }

    public class TrojanSettings
    {
        [JsonPropertyName("address")]
        public string? Address { get; set; }

        [JsonPropertyName("port")]
        public int? Port { get; set; }

        [JsonPropertyName("password")]
        public string? Password { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; }

        [JsonPropertyName("level")]
        public int Level { get; set; }
    }

    public class VlessSettings
    {
        [JsonPropertyName("address")]
        public string? Address { get; set; }

        [JsonPropertyName("port")]
        public required int Port { get; set; }

        [JsonPropertyName("id")]
        public required string Id { get; set; }

        [JsonPropertyName("encryption")]
        public string Encryption { get; set; } = "none";

        [JsonPropertyName("flow")]
        public Flow? Flow { get; set; }

        [JsonPropertyName("level")]
        public int Level { get; set; } = 0;

        [JsonPropertyName("reverse")]
        public VlessReverse? Reverse { get; set; }
    }

    public class VMessSettings
    {
        [JsonPropertyName("address")]
        public string? Address { get; set; }

        [JsonPropertyName("port")]
        public int? Port { get; set; }

        [JsonPropertyName("id")]
        public required string Id { get; set; }

        [JsonPropertyName("security")]
        public VMessSecurity? Security { get; set; }

        [JsonPropertyName("level")]
        public int Level { get; set; }

        [JsonPropertyName("experiments")]
        public string? Experiments { get; set; }
    }

    public class WireguardSettings
    {
        [JsonPropertyName("secretKey")]
        public string? SecretKey { get; set; }

        [JsonPropertyName("address")]
        public List<string>? Address { get; set; }

        [JsonPropertyName("peers")]
        public List<WireguardOutboundPeer>? Peers { get; set; }

        [JsonPropertyName("noKernelTun")]
        public bool? NoKernelTun { get; set; }

        [JsonPropertyName("mtu")]
        public int? Mtu { get; set; }

        [JsonPropertyName("reserved")]
        public List<int>? Reserved { get; set; }

        [JsonPropertyName("workers")]
        public int? Workers { get; set; }

        [JsonPropertyName("domainStrategy")]
        public DomainStrategy? DomainStrategy { get; set; }
    }
}

public record ProxySettings(string Tag);

public record VlessReverse(string Tag);

public class WireguardOutboundPeer
{
    [JsonPropertyName("endpoint")]
    public required string Endpoint { get; set; }

    [JsonPropertyName("publicKey")]
    public required string PublicKey { get; set; }

    [JsonPropertyName("preSharedKey")]
    public string? PreSharedKey { get; set; }

    [JsonPropertyName("keepAlive")]
    public bool? KeepAlive { get; set; }

    [JsonPropertyName("allowedIPs")]
    public List<string>? allowedIPs { get; set; }
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

public record BlackHoleResponse(string Type = "none");

public class BlackHoleOutbound : Outbound
{
    public BlackHoleOutbound() : base(OutboundProtocol.BlackHole) { }

    [JsonPropertyName("settings")]
    public BlackHoleSettings? Settings { get; set; }
}

public class DnsOutbound : Outbound
{
    public DnsOutbound() : base(OutboundProtocol.Dns) { }

    [JsonPropertyName("settings")]
    public DnsSettings? Settings { get; set; }
}

public class FreedomOutbound : Outbound
{
    public FreedomOutbound() : base(OutboundProtocol.Freedom) { }

    [JsonPropertyName("settings")]
    public FreedomSettings? Settings { get; set; }
}

public class HttpOutbound : Outbound
{
    public HttpOutbound() : base(OutboundProtocol.Http) { }

    [JsonPropertyName("settings")]
    public HttpSettings? Settings { get; set; }
}

public class LoopbackOutbound : Outbound
{
    public LoopbackOutbound() : base(OutboundProtocol.Loopback) { }

    [JsonPropertyName("settings")]
    public LoopbackSettings? Settings { get; set; }
}

public class ShadowSocksOutbound : Outbound
{
    public ShadowSocksOutbound() : base(OutboundProtocol.ShadowSocks) { }

    [JsonPropertyName("settings")]
    public ShadowSocksSettings? Settings { get; set; }
}

public class SocksOutbound : Outbound
{
    public SocksOutbound() : base(OutboundProtocol.Socks) { }

    [JsonPropertyName("settings")]
    public SocksSettings? Settings { get; set; }
}

public class TrojanOutbound : Outbound
{
    public TrojanOutbound() : base(OutboundProtocol.Trojan) { }

    [JsonPropertyName("settings")]
    public TrojanSettings? Settings { get; set; }
}

public class VlessOutbound : Outbound
{
    public VlessOutbound() : base(OutboundProtocol.Vless) { }

    [JsonPropertyName("settings")]
    public VlessSettings? Settings { get; set; }
}

public class VMessOutbound : Outbound
{
    public VMessOutbound() : base(OutboundProtocol.VMess) { }

    [JsonPropertyName("settings")]
    public VMessSettings? Settings { get; set; }
}

public class WireguardOutbound : Outbound
{
    public WireguardOutbound() : base(OutboundProtocol.Wireguard) { }

    [JsonPropertyName("settings")]
    public WireguardSettings? Settings { get; set; }
}

// convertors

public class OutboundConfigConverter : JsonConverter<Outbound>
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