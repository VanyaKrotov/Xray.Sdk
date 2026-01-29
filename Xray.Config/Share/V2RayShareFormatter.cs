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
        var stream = inbound.StreamSettings;
        var streamOptions = GetStreamOptions(stream);
        var security = stream.Security;
        var options = new VlessOptions()
        {
            Type = streamOptions.Type,
            Path = streamOptions.Path,
            Host = streamOptions.Host,
            Mode = streamOptions.Mode,
            Seed = streamOptions.Seed,
            Authority = streamOptions.Authority,
            ServiceName = streamOptions.ServiceName,
            HeaderType = streamOptions.HeaderType,
            Security = EnumPropertyConverter.ToString(security),
            Encryption = EnumPropertyConverter.ToString(inbound.Settings.Decryption),
        };

        if (security == StreamSecurity.Tls && stream.TlsSettings != null)
        {
            GetTlsOptions(stream.TlsSettings, options);
        }
        else if (security == StreamSecurity.Reality && stream.RealitySettings != null)
        {
            GetRealityOptions(stream.RealitySettings, options);
        }

        if (stream.Network == StreamNetwork.Raw)
        {
            options.Flow = EnumPropertyConverter.ToString(client.Flow);
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
            Query = QueryUtilities.ToQuery(options),
        };

        return builder.ToString();
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
            Type = "none",
            Version = "2",
            Id = client.Id,
            Remark = GetRemark(inbound),
            Port = (int)inbound.Port.Single!,
            Address = GetAddressOrDefault(inbound.Listen),
            Network = EnumPropertyConverter.ToString(stream.Network),
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

        if (stream.Security == StreamSecurity.Tls && stream.TlsSettings != null)
        {
            var tls = stream.TlsSettings;
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

        return $"vmess://{Convert.ToBase64String(Encoding.UTF8.GetBytes(options.ToJson()))}";
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

    #region ShadowSocks

    public override string FromInbound(ShadowSocksInbound inbound, ShadowSocksClient client)
    {
        var stream = inbound.StreamSettings;
        var streamOptions = GetStreamOptions(stream);
        var options = new ShadowSocksOptions()
        {
            Type = streamOptions.Type,
            Path = streamOptions.Path,
            Host = streamOptions.Host,
            Mode = streamOptions.Mode,
            Seed = streamOptions.Seed,
            Authority = streamOptions.Authority,
            ServiceName = streamOptions.ServiceName,
            HeaderType = streamOptions.HeaderType,
        };

        if (stream.Security == StreamSecurity.Tls && stream.TlsSettings != null)
        {
            GetTlsOptions(stream.TlsSettings, options);
        }

        var methodString = inbound.Settings?.Method != null ? EnumPropertyConverter.ToString((EncryptionMethod)inbound.Settings.Method) : "";
        var part = $"{methodString}:{client.Password}";
        if (methodString.StartsWith("2022"))
        {
            part = $"{methodString}:{inbound.Settings?.Password ?? ""}:{client.Password}";
        }

        var builder = new UriBuilder()
        {
            Scheme = "ss",
            Path = "",
            UserName = Convert.ToBase64String(Encoding.UTF8.GetBytes(part)),
            Password = "",
            Port = (int)inbound.Port.Single!,
            Host = GetAddressOrDefault(inbound.Listen),
            Query = QueryUtilities.ToQuery(options),
            Fragment = GetRemark(inbound)
        };

        return builder.ToString();
    }

    public override string FromInbound(ShadowSocksInbound inbound, string email)
    {
        var client = inbound.Settings?.Clients.FirstOrDefault(x => x.Email == email);
        if (client == null)
        {
            throw new ArgumentException($"Client for Email = {email} not found in inbound {inbound.Tag}");
        }

        return FromInbound(inbound, client);
    }

    #endregion

    #region  Trojan

    public override string FromInbound(TrojanInbound inbound, TrojanClient client)
    {
        var stream = inbound.StreamSettings;
        var streamOptions = GetStreamOptions(stream);
        var security = stream.Security;
        var options = new TrojanOptions()
        {
            Type = streamOptions.Type,
            Path = streamOptions.Path,
            Host = streamOptions.Host,
            Mode = streamOptions.Mode,
            Seed = streamOptions.Seed,
            Authority = streamOptions.Authority,
            ServiceName = streamOptions.ServiceName,
            HeaderType = streamOptions.HeaderType,
            Security = EnumPropertyConverter.ToString(security),
        };

        if (security == StreamSecurity.Tls && stream.TlsSettings != null)
        {
            GetTlsOptions(stream.TlsSettings, options);
        }
        else if (security == StreamSecurity.Reality && stream.RealitySettings != null)
        {
            GetRealityOptions(stream.RealitySettings, options);
        }

        var builder = new UriBuilder()
        {
            Path = "",
            Password = "",
            Scheme = "trojan",
            UserName = client.Password,
            Fragment = GetRemark(inbound),
            Port = (int)inbound.Port.Single!,
            Host = GetAddressOrDefault(inbound.Listen),
            Query = QueryUtilities.ToQuery(options),
        };

        return builder.ToString();
    }

    public override string FromInbound(TrojanInbound inbound, string email)
    {
        var client = inbound.Settings?.Clients.FirstOrDefault(x => x.Email == email);
        if (client == null)
        {
            throw new ArgumentException($"Client for Email = {email} not found in inbound {inbound.Tag}");
        }

        return FromInbound(inbound, client);
    }

    #endregion

    #region Socks

    public override string FromInbound(SocksInbound inbound, SocksAccount account)
    {
        var uriBuilder = new UriBuilder()
        {
            Path = "",
            Scheme = "socks",
            UserName = account.User,
            Password = account.Password,
            Fragment = GetRemark(inbound),
            Port = (int)inbound.Port.Single!,
            Host = GetAddressOrDefault(inbound.Listen),
        };

        return uriBuilder.Uri.ToString();
    }

    public override string FromInbound(SocksInbound inbound, string username)
    {
        var client = inbound.Settings?.Accounts.FirstOrDefault(x => x.User == username);
        if (client == null)
        {
            throw new ArgumentException($"Client for Username = {username} not found in inbound {inbound.Tag}");
        }

        return FromInbound(inbound, client);
    }

    #endregion

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

    private void GetTlsOptions(TlsSettings tlsSettings, TransferOptions options)
    {
        var alpns = tlsSettings.Alpn ?? new();
        if (alpns.Count > 0)
        {
            options.Alpn = string.Join(",", alpns);
        }

        if (tlsSettings.ServerName != null)
        {
            options.ServerName = tlsSettings.ServerName;
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

    private void GetRealityOptions(RealitySettings reality, RealityTransferOptions options)
    {
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
    }

    private StreamOptions GetStreamOptions(StreamSettings stream)
    {
        var options = new StreamOptions()
        {
            Type = EnumPropertyConverter.ToString(stream.Network),
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


        return options;
    }

    private string SearchHost(NameValueCollection headers)
    {
        var values = headers.GetValues("host");

        return values?.FirstOrDefault() ?? "";
    }

    private string GetAddressOrDefault(string? listen) => string.IsNullOrEmpty(listen) ? DEFAULT_ADDRESS : listen;
}

class StreamOptions
{
    public string? Path { get; set; }
    public string? Host { get; set; }
    public string? HeaderType { get; set; }
    public string? Seed { get; set; }
    public string? ServiceName { get; set; }
    public string? Authority { get; set; }
    public string? Mode { get; set; }
    public string? Type { get; set; }
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

    //
    private static readonly JsonSerializerOptions _serializeOptions = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };

    public string ToJson() => JsonSerializer.Serialize(this, _serializeOptions);
}

static class QueryUtilities
{
    private static JsonSerializerOptions _options = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };

    public static string ToQuery(object model)
    {
        var json = JsonSerializer.Serialize(model, _options);
        var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(json)!;

        return $"?{string.Join("&", dict.Select(x => $"{x.Key}={x.Value}"))}";
    }
}

class TransferOptions
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("path")]
    public string? Path { get; set; }

    [JsonPropertyName("host")]
    public string? Host { get; set; }

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
}

class RealityTransferOptions : TransferOptions
{

    [JsonPropertyName("flow")]
    public string? Flow { get; set; }

    [JsonPropertyName("pbk")]
    public string? PublicKey { get; set; }

    [JsonPropertyName("pqv")]
    public string? Mldsa65Verify { get; set; }

    [JsonPropertyName("sid")]
    public string? ShortId { get; set; }

    [JsonPropertyName("spx")]
    public string? SpiderX { get; set; }
}

class VlessOptions : RealityTransferOptions
{

    [JsonPropertyName("encryption")]
    public string? Encryption { get; set; }
}

class TrojanOptions : RealityTransferOptions { }

class ShadowSocksOptions : TransferOptions { }