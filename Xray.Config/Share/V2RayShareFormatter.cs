using System.Collections.Specialized;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xray.Config.Enums;
using Xray.Config.Models;
using Xray.Config.Utilities;

namespace Xray.Config.Share;

public class V2RayShareFormatter : ShareFormatter
{
    private static string DEFAULT_ADDRESS = "0.0.0.0";

    public V2RayShareFormatter() : base("v2ray") { }

    #region Vless
    public override string FromInbound(VlessInbound inbound, VlessClient client)
    {
        var settings = inbound.Settings;
        var stream = inbound.StreamSettings;
        var options = new VlessOptions()
        {
            Encryption = EnumPropertyConverter.ToString(settings.Decryption),
            Type = EnumPropertyConverter.ToString(stream.Network),
            Security = EnumPropertyConverter.ToString(stream.Security)
        };

        switch (stream.Network)
        {
            case StreamNetwork.Raw:
                {
                    var raw = stream.RawSettings ?? new();
                    var header = raw.Header;
                    if (header != null && header is HttpSettingsHeaders)
                    {
                        var typedHeader = (HttpSettingsHeaders)header;

                        options.Path = typedHeader.Request.Path.FirstOrDefault();
                        options.Host = SearchHost(typedHeader.Request.Headers);
                        options.HeaderType = "http";
                    }
                }
                break;

            case StreamNetwork.Kcp:
                {
                    var kcp = stream.KcpSettings ?? new();
                    var header = kcp.Header ?? new();

                    options.HeaderType = EnumPropertyConverter.ToString(header.Type);
                    options.Seed = kcp.Seed;
                }
                break;

            case StreamNetwork.Ws:
                {
                    var ws = stream.WSSettings ?? new();

                    options.Path = ws.Path;
                    options.Host = ws.Host == null ? SearchHost(ws.Headers) : ws.Host;
                }
                break;

            case StreamNetwork.Grpc:
                {
                    var grpc = stream.GRPCSettings ?? new();

                    options.ServiceName = grpc.ServiceName;
                    options.Authority = grpc.Authority;

                    if (grpc.MultiMode)
                    {
                        options.Mode = "multi";
                    }
                }
                break;

            case StreamNetwork.HttpUpgrade:
                {
                    var httpUpgrade = stream.HttpUpgradeSettings ?? new();

                    options.Path = httpUpgrade.Path;
                    options.Host = httpUpgrade.Host == null ? SearchHost(httpUpgrade.Headers) : httpUpgrade.Host;
                }
                break;

            case StreamNetwork.XHttp:
                {
                    var xhttp = stream.XHttpSettings ?? new();

                    options.Path = xhttp.Path;
                    options.Host = xhttp.Host == null ? SearchHost(xhttp.Headers) : xhttp.Host;
                    options.Mode = EnumPropertyConverter.ToString(xhttp.Mode);
                }
                break;
        }

        switch (stream.Security)
        {
            case StreamSecurity.Tls:
                {
                    var tlsSettings = stream.TlsSettings ?? new();
                    var alpns = tlsSettings.Alpn ?? new();
                    if (alpns.Count > 0)
                    {
                        options.Alpn = string.Join(",", alpns);
                    }

                    if (tlsSettings.ServerName != null)
                    {
                        options.ServiceName = tlsSettings.ServerName;
                    }

                    if (tlsSettings.Fingerprint != null)
                    {
                        options.Fingerprint = EnumPropertyConverter.ToString((Fingerprint)tlsSettings.Fingerprint);
                    }

                    if (tlsSettings.AllowInsecure)
                    {
                        options.AllowInsecure = "1";
                    }
                }
                break;

            case StreamSecurity.Reality:
                {
                    var reality = stream.RealitySettings ?? new();
                    if (reality.ServerNames.Count > 0)
                    {
                        options.ServerName = reality.ServerNames.ElementAt(RandomUtilities.GetInRange(0, reality.ServerNames.Count));
                    }

                    if (reality.Password != null)
                    {
                        options.PublicKey = reality.Password;
                    }

                    if (reality.ShortIds != null && reality.ShortIds.Count > 0)
                    {
                        options.ShortId = reality.ShortIds.ElementAt(RandomUtilities.GetInRange(0, reality.ShortIds.Count));
                    }

                    if (!string.IsNullOrEmpty(reality.Mldsa65Verify))
                    {
                        options.Mldsa65Verify = reality.Mldsa65Verify;
                    }

                    options.Fingerprint = EnumPropertyConverter.ToString(reality.Fingerprint);
                    options.SpiderX = $"/{RandomUtilities.Seq(15)}";

                    if (stream.Network == StreamNetwork.Raw)
                    {
                        options.Flow = EnumPropertyConverter.ToString(client.Flow);
                    }
                }

                break;
        }

        var builder = new UriBuilder()
        {
            Scheme = "vless",
            Fragment = GetRemark(inbound),
            UserName = client.Id,
            Password = "",
            Path = "",
            Port = (int)inbound.Port.Single!,
            Host = GetAddressOrDefault(inbound.Listen),
            Query = options.ToQuery(),
        };

        return builder.Uri.ToString();
    }

    public override string FromInbound(VlessInbound inbound, string email)
    {
        var client = inbound.Settings.Clients.FirstOrDefault(x => x.Email == email);
        if (client == null)
        {
            throw new ArgumentException($"Client for Email = {email} not found in inbound {inbound.Tag}");
        }

        return FromInbound(inbound, client);
    }

    #endregion

    #region VMess
    public override string FromInbound(VMessInbound inbound, VMessClient client)
    {
        var stream = inbound.StreamSettings;
        var options = new VMessParams()
        {
            Address = GetAddressOrDefault(inbound.Listen),
            Port = (int)inbound.Port.Single!,
            Type = "none",
            Version = "2",
            Id = client.Id,
            Network = EnumPropertyConverter.ToString(stream.Network),
            Remark = GetRemark(inbound),
            Tls = EnumPropertyConverter.ToString(stream.Security)
        };

        switch (stream.Network)
        {
            case StreamNetwork.Raw:
                {
                    var raw = stream.RawSettings ?? new();
                    var header = raw.Header;
                    if (header != null)
                    {
                        options.Type = EnumPropertyConverter.ToString(header.Type);
                    }

                    if (header != null && header is HttpSettingsHeaders)
                    {
                        var typedHeader = (HttpSettingsHeaders)header;

                        options.Path = typedHeader.Request.Path.First();
                        options.Host = SearchHost(typedHeader.Request.Headers);
                    }
                }
                break;

            case StreamNetwork.Kcp:
                {
                    var kcp = stream.KcpSettings ?? new();
                    var header = kcp.Header ?? new();

                    options.Path = kcp.Seed ?? "";
                    options.Type = EnumPropertyConverter.ToString(header.Type);
                }
                break;

            case StreamNetwork.Ws:
                {
                    var ws = stream.WSSettings ?? new();

                    options.Path = ws.Path ?? "";
                    options.Host = ws.Host == null ? SearchHost(ws.Headers) : ws.Host;
                }
                break;

            case StreamNetwork.Grpc:
                {
                    var grpc = stream.GRPCSettings ?? new();

                    options.Path = grpc.ServiceName;
                    options.Authority = grpc.Authority ?? "";

                    if (grpc.MultiMode)
                    {
                        options.Type = "multi";
                    }
                }
                break;

            case StreamNetwork.HttpUpgrade:
                {
                    var httpUpgrade = stream.HttpUpgradeSettings ?? new();

                    options.Path = httpUpgrade.Path ?? "";
                    options.Host = httpUpgrade.Host == null ? SearchHost(httpUpgrade.Headers) : httpUpgrade.Host;
                }
                break;

            case StreamNetwork.XHttp:
                {
                    var xhttp = stream.XHttpSettings ?? new();

                    options.Path = xhttp.Path ?? "";
                    options.Host = xhttp.Host == null ? SearchHost(xhttp.Headers) : xhttp.Host;
                    options.Mode = EnumPropertyConverter.ToString(xhttp.Mode);
                }
                break;
        }

        if (stream.Security == StreamSecurity.Tls)
        {
            var tls = stream.TlsSettings ?? new();
            if (tls.Alpn != null && tls.Alpn.Count > 0)
            {
                options.Alpn = string.Join(",", tls.Alpn);
            }

            if (tls.ServerName != null)
            {
                options.ServerName = tls.ServerName;
            }

            if (tls.Fingerprint != null)
            {
                options.Fingerprint = EnumPropertyConverter.ToString((Fingerprint)tls.Fingerprint);
            }

            if (tls.AllowInsecure)
            {
                options.AllowInsecure = true;
            }
        }


        var jsonOptions = JsonSerializer.Serialize(options, new JsonSerializerOptions()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        });

        return $"vmess://{Convert.ToBase64String(Encoding.UTF8.GetBytes(jsonOptions))}";
    }

    public override string FromInbound(VMessInbound inbound, string email)
    {
        var client = inbound.Settings.Clients.FirstOrDefault(x => x.Email == email);
        if (client == null)
        {
            throw new ArgumentException($"Client for Email = {email} not found in inbound {inbound.Tag}");
        }

        return FromInbound(inbound, client);
    }

    #endregion

    public override string FromInbound(ShadowSocksInbound inbound)
    {
        throw new NotImplementedException();
    }

    public override string FromInbound(SocksInbound inbound)
    {
        throw new NotImplementedException();
    }

    public override string FromInbound(TrojanInbound inbound)
    {
        throw new NotImplementedException();
    }



    public override HysteriaOutbound ToHysteriaOutbound(string config)
    {
        throw new NotImplementedException();
    }


    public override Outbound ToOutbound(string config)
    {
        throw new NotImplementedException();
    }

    public override ShadowSocksOutbound ToShadowSocksOutbound(string config)
    {
        throw new NotImplementedException();
    }

    public override SocksOutbound ToSocksOutbound(string config)
    {
        throw new NotImplementedException();
    }

    public override TrojanOutbound ToTrojanOutbound(string config)
    {
        throw new NotImplementedException();
    }

    public override VlessOutbound ToVlessOutbound(string config)
    {
        throw new NotImplementedException();
    }

    public override VMessOutbound ToVMessOutbound(string config)
    {
        throw new NotImplementedException();
    }


    // private utilities

    private string SearchHost(NameValueCollection headers)
    {
        var values = headers.GetValues("host");

        return values?.FirstOrDefault() ?? "";
    }

    private string GetAddressOrDefault(string? listen) => string.IsNullOrEmpty(listen) ? DEFAULT_ADDRESS : listen;
}

class QueryBuilder
{
    private Dictionary<string, string> _data = new(StringComparer.OrdinalIgnoreCase);

    public QueryBuilder() { }

    public QueryBuilder(Dictionary<string, string> initial)
    {
        foreach (var entity in initial)
        {
            _data.Add(entity.Key, entity.Value);
        }
    }

    public void Add(string key, string value)
    {
        _data.Add(key, value);
    }

    public void Remove(string key)
    {
        _data.Remove(key);
    }

    public override string ToString()
    {
        return string.Join("&", _data.Select(x => $"{x.Key}={x.Value}"));
    }
}

class VMessParams
{
    [JsonPropertyName("add")]
    public required string Address { get; set; }

    [JsonPropertyName("port")]
    public required int Port { get; set; }

    [JsonPropertyName("v")]
    public string Version { get; set; } = "2";

    [JsonPropertyName("allowInsecure")]
    public bool AllowInsecure { get; set; }

    [JsonPropertyName("fp")]
    public string Fingerprint { get; set; } = string.Empty;

    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("alpn")]
    public string? Alpn { get; set; }

    [JsonPropertyName("sni")]
    public string? ServerName { get; set; }

    [JsonPropertyName("scy")]
    public string? ClientSecurity { get; set; }

    [JsonPropertyName("ps")]
    public string? Remark { get; set; }

    [JsonPropertyName("path")]
    public string Path { get; set; } = string.Empty;

    [JsonPropertyName("tls")]
    public string Tls { get; set; } = "none";

    [JsonPropertyName("net")]
    public required string Network { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("host")]
    public string Host { get; set; } = string.Empty;

    [JsonPropertyName("aid")]
    public string Aid { get; set; } = "0";

    [JsonPropertyName("authority")]
    public string? Authority { get; set; }

    [JsonPropertyName("mode")]
    public string? Mode { get; set; }
}

public class VlessOptions
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("path")]
    public string? Path { get; set; }

    [JsonPropertyName("host")]
    public string? Host { get; set; }

    [JsonPropertyName("encryption")]
    public string? Encryption { get; set; }

    [JsonPropertyName("headerType")]
    public string? HeaderType { get; set; }

    [JsonPropertyName("seed")]
    public string? Seed { get; set; }

    [JsonPropertyName("serviceName")]
    public string? ServiceName { get; set; }

    [JsonPropertyName("authority")]
    public string? Authority { get; set; }

    [JsonPropertyName("mode")]
    public string? Mode { get; set; }

    [JsonPropertyName("security")]
    public string? Security { get; set; }

    [JsonPropertyName("alpn")]
    public string? Alpn { get; set; }

    [JsonPropertyName("sni")]
    public string? ServerName { get; set; }

    [JsonPropertyName("fp")]
    public string? Fingerprint { get; set; }

    [JsonPropertyName("allowInsecure")]
    public string? AllowInsecure { get; set; }

    [JsonPropertyName("sid")]
    public string? ShortId { get; set; }

    [JsonPropertyName("pqv")]
    public string? Mldsa65Verify { get; set; }

    [JsonPropertyName("pbk")]
    public string? PublicKey { get; set; }

    [JsonPropertyName("flow")]
    public string? Flow { get; set; }

    [JsonPropertyName("spx")]
    public string? SpiderX { get; set; }

    private static JsonSerializerOptions _options = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };

    public string ToQuery()
    {
        var json = JsonSerializer.Serialize(this, _options);
        var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(json)!;

        return $"?{string.Join("&", dict.Select(x => $"{x.Key}={x.Value}"))}";
    }
}