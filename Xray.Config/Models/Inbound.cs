using System.Text.Json;
using System.Text.Json.Serialization;
using Xray.Config.Enums;
using Xray.Config.Utilities;

namespace Xray.Config.Models;

[JsonConverter(typeof(InboundConfigConverter))]
public abstract class Inbound
{
    [JsonPropertyName("listen")]
    public string? Listen { get; set; }

    [JsonPropertyName("port")]
    public required int Port { get; set; }

    [JsonPropertyName("protocol")]
    public InboundProtocol Protocol { get; set; }

    [JsonPropertyName("tag")]
    public required string Tag { get; set; }

    [JsonPropertyName("sniffing")]
    public InboundSniffing? Sniffing { get; set; }

    [JsonPropertyName("allocate")]
    public InboundAllocate? Allocate { get; set; }

    [JsonPropertyName("streamSettings")]
    public StreamSettings StreamSettings { get; set; } = new();

    public Inbound(InboundProtocol protocol)
    {
        Protocol = protocol;
    }

    public class DokodemoDoorSettings
    {
        [JsonPropertyName("address")]
        public string? Address { get; set; }

        [JsonPropertyName("port")]
        public int? Port { get; set; }

        [JsonPropertyName("network")]
        [JsonConverter(typeof(SplitEnumConverter<TransportProtocol>))]
        public List<TransportProtocol>? Network { get; set; }

        [JsonPropertyName("followRedirect")]
        public bool FollowRedirect { get; set; }

        [JsonPropertyName("userLevel")]
        public int UserLevel { get; set; } = 0;
    }

    public class HttpSettings
    {
        [JsonPropertyName("accounts")]
        public List<HttpAccount> Accounts { get; set; } = new();

        [JsonPropertyName("allowTransparent")]
        public bool AllowTransparent { get; set; }

        [JsonPropertyName("userLevel")]
        public int UserLevel { get; set; } = 0;
    }

    public class ShadowSocksSettings : ShadowSocksClient
    {
        [JsonPropertyName("clients")]
        public List<ShadowSocksClient> Clients { get; set; } = new();

        [JsonPropertyName("network")]
        [JsonConverter(typeof(SplitEnumConverter<TransportProtocol>))]
        public List<TransportProtocol>? Network { get; set; }
    }

    public class SocksSettings
    {
        [JsonPropertyName("auth")]
        public SocksAuth Auth { get; set; } = SocksAuth.NoAuth;

        [JsonPropertyName("accounts")]
        public List<SocksAccount> Accounts { get; set; } = new();

        [JsonPropertyName("udp")]
        public bool Udp { get; set; }

        [JsonPropertyName("ip")]
        public string? Ip { get; set; }

        [JsonPropertyName("userLevel")]
        public int UserLevel { get; set; } = 0;
    }

    public class TrojanSettings
    {
        [JsonPropertyName("clients")]
        public List<TrojanClient> Clients { get; set; } = new();

        [JsonPropertyName("fallbacks")]
        public List<Fallback>? Fallbacks { get; set; }
    }

    public class VlessSettings
    {
        [JsonPropertyName("clients")]
        public List<VlessClient> Clients { get; set; } = new();

        [JsonPropertyName("decryption")]
        public VlessDecryption Decryption { get; set; } = VlessDecryption.None;

        [JsonPropertyName("fallbacks")]
        public List<Fallback>? Fallbacks { get; set; }
    }

    public class VMessSettings
    {
        [JsonPropertyName("clients")]
        public List<VMessClient> Clients { get; set; } = new();

        [JsonPropertyName("default")]
        public VMessDefaultSettings? Default { get; set; }
    }

    public class WireguardSettings
    {
        [JsonPropertyName("secretKey")]
        public required string SecretKey { get; set; }

        [JsonPropertyName("peers")]
        public List<WireguardPeer> Peers { get; set; } = new();

        [JsonPropertyName("mtu")]
        public int? Mtu { get; set; }
    }
}

public record HttpAccount(string User, string Pass);

public record SocksAccount(string User, string Pass);

public record TrojanClient(string Email, string Password, int Level = 0);

public record VMessClient(string Id, string Email, int Level = 0);

public record VMessDefaultSettings(int Level = 0);

public class WireguardPeer
{
    [JsonPropertyName("publicKey")]
    public required string PublicKey { get; set; }

    [JsonPropertyName("allowedIPs")]
    public List<string>? AllowedIPs { get; set; }
}

public class VlessClient
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("level")]
    public int Level { get; set; } = 0;

    [JsonPropertyName("email")]
    public required string Email { get; set; }

    [JsonPropertyName("flow")]
    public Flow Flow { get; set; } = Flow.None;
}

public class ShadowSocksClient
{
    [JsonPropertyName("password")]
    public string? Password { get; set; }

    [JsonPropertyName("method")]
    public EncryptionMethod? Method { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("level")]
    public int Level { get; set; } = 0;
}

public class InboundSniffing
{
    [JsonPropertyName("enabled")]
    public bool Enabled { get; set; }

    [JsonPropertyName("destOverride")]
    public List<TrafficType>? DestOverride { get; set; }

    [JsonPropertyName("metadataOnly")]
    public bool MetadataOnly { get; set; }

    [JsonPropertyName("domainsExcluded")]
    public List<string>? DomainsExcluded { get; set; }

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

public class DokodemoDoorInbound : Inbound
{
    public DokodemoDoorInbound() : base(InboundProtocol.DokodemoDoor) { }

    [JsonPropertyName("settings")]
    public DokodemoDoorSettings? Settings { get; set; }
}

public class HttpInbound : Inbound
{
    public HttpInbound() : base(InboundProtocol.Http) { }

    [JsonPropertyName("settings")]
    public HttpSettings? Settings { get; set; }
}

public class SocksInbound : Inbound
{
    public SocksInbound() : base(InboundProtocol.Socks) { }

    [JsonPropertyName("settings")]
    public SocksSettings Settings { get; set; } = new();
}

public abstract class SharedInbound : Inbound
{
    protected SharedInbound(InboundProtocol protocol) : base(protocol) { }

    [JsonIgnore]
    public abstract string SharedLink { get; }
}

public class ShadowSocksInbound : SharedInbound
{
    public ShadowSocksInbound() : base(InboundProtocol.ShadowSocks) { }

    [JsonPropertyName("settings")]
    public ShadowSocksSettings? Settings { get; set; }

    public override string SharedLink => throw new NotImplementedException();
}

public class VlessInbound : SharedInbound
{
    public VlessInbound() : base(InboundProtocol.Vless) { }

    [JsonPropertyName("settings")]
    public VlessSettings Settings { get; set; } = new();

    public override string SharedLink => throw new NotImplementedException();
}

public class VMessInbound : SharedInbound
{
    public VMessInbound() : base(InboundProtocol.VMess) { }

    [JsonPropertyName("settings")]
    public VMessSettings Settings { get; set; } = new();

    public override string SharedLink => throw new NotImplementedException();
}

public class TrojanInbound : Inbound
{
    public TrojanInbound() : base(InboundProtocol.Trojan) { }

    [JsonPropertyName("settings")]
    public TrojanSettings Settings { get; set; } = new();
}

public class WireguardInbound : SharedInbound
{
    public WireguardInbound() : base(InboundProtocol.Wireguard) { }

    [JsonPropertyName("settings")]
    public WireguardSettings? Settings { get; set; }

    public override string SharedLink => throw new NotImplementedException();
}

// convertors

class InboundConfigConverter : JsonConverter<Inbound>
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
        { InboundProtocol.Wireguard, typeof(WireguardInbound) }
    };

    public override Inbound Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var protocol = doc.RootElement.GetProperty("protocol").Deserialize<InboundProtocol>(options);

        if (_protocolMap.TryGetValue(protocol, out var targetType))
        {
            return (Inbound?)doc.Deserialize(targetType, options)!;
        }

        throw new JsonException($"Unsupported protocol: {protocol}");

    }

    public override void Write(Utf8JsonWriter writer, Inbound value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
    }
}