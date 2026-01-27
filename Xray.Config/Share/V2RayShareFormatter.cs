using System.Collections.Specialized;
using Xray.Config.Enums;
using Xray.Config.Models;
using Xray.Config.Utilities;

namespace Xray.Config.Share;

public class V2RayShareFormatter : ShareFormatter
{
    private static string DEFAULT_ADDRESS = "0.0.0.0";

    public V2RayShareFormatter() : base("v2ray") { }

    public override string FromInbound(VlessInbound inbound, VlessClient client)
    {
        var settings = inbound.Settings;
        var stream = inbound.StreamSettings;

        var query = new QueryBuilder(new()
        {
            { "encryption", EnumPropertyConverter.ToString(settings.Decryption) },
            { "type", EnumPropertyConverter.ToString(stream.Network)}
        });

        switch (stream.Network)
        {
            case StreamNetwork.Raw:
                {
                    var raw = stream.RawSettings ?? new();
                    var header = raw.Header;
                    if (header != null && header is HttpSettingsHeaders)
                    {
                        var typedHeader = (HttpSettingsHeaders)header;

                        query.Add("path", typedHeader.Request.Path.First());
                        query.Add("host", SearchHost(typedHeader.Request.Headers));
                        query.Add("headerType", "http");
                    }
                }

                break;

            case StreamNetwork.Kcp:
                {
                    var kcp = stream.KcpSettings ?? new();
                    var header = kcp.Header ?? new();

                    query.Add("headerType", EnumPropertyConverter.ToString(header.Type));
                    query.Add("seed", kcp.Seed ?? "");
                }

                break;

            case StreamNetwork.Ws:
                {
                    var ws = stream.WSSettings ?? new();

                    query.Add("path", ws.Path ?? "");

                    if (ws.Host == null)
                    {
                        query.Add("host", SearchHost(ws.Headers));
                    }
                    else
                    {
                        query.Add("host", ws.Host);
                    }
                }

                break;

            case StreamNetwork.Grpc:
                {
                    var grpc = stream.GRPCSettings ?? new();

                    query.Add("serviceName", grpc.ServiceName);
                    query.Add("authority", grpc.Authority ?? "");

                    if (grpc.MultiMode)
                    {
                        query.Add("mode", "multi");
                    }
                }

                break;

            case StreamNetwork.HttpUpgrade:
                {
                    var httpUpgrade = stream.HttpUpgradeSettings ?? new();

                    query.Add("path", httpUpgrade.Path ?? "");
                    if (httpUpgrade.Host == null)
                    {
                        query.Add("host", SearchHost(httpUpgrade.Headers));
                    }
                    else
                    {
                        query.Add("host", httpUpgrade.Host);
                    }
                }

                break;

            case StreamNetwork.XHttp:
                {
                    var xhttp = stream.XHttpSettings ?? new();

                    query.Add("path", xhttp.Path ?? "");

                    if (xhttp.Host == null)
                    {
                        query.Add("host", SearchHost(xhttp.Headers));
                    }
                    else
                    {
                        query.Add("host", xhttp.Host);
                    }

                    query.Add("mode", EnumPropertyConverter.ToString(xhttp.Mode));
                }

                break;
        }


        query.Add("security", EnumPropertyConverter.ToString(stream.Security));


        switch (stream.Security)
        {
            case StreamSecurity.Tls:
                {
                    var tlsSettings = stream.TlsSettings ?? new();
                    var alpns = tlsSettings.Alpn ?? new();
                    if (alpns.Count > 0)
                    {
                        query.Add("alpn", string.Join(",", alpns));
                    }

                    if (tlsSettings.ServerName != null)
                    {
                        query.Add("sni", tlsSettings.ServerName);
                    }

                    if (tlsSettings.Fingerprint != null)
                    {
                        query.Add("fp", EnumPropertyConverter.ToString((Fingerprint)tlsSettings.Fingerprint));
                    }

                    if (tlsSettings.AllowInsecure)
                    {
                        query.Add("allowInsecure", "1");
                    }
                }

                break;

            case StreamSecurity.Reality:
                {
                    var reality = stream.RealitySettings ?? new();
                    if (reality.ServerNames.Count > 0)
                    {
                        query.Add("sni", reality.ServerNames.ElementAt(RandomUtilities.GetInRange(0, reality.ServerNames.Count)));
                    }

                    if (reality.Password != null)
                    {
                        query.Add("pbk", reality.Password);
                    }

                    if (reality.ShortIds != null && reality.ShortIds.Count > 0)
                    {
                        query.Add("sid", reality.ShortIds.ElementAt(RandomUtilities.GetInRange(0, reality.ShortIds.Count)));
                    }

                    if (!string.IsNullOrEmpty(reality.Mldsa65Verify))
                    {
                        query.Add("pqv", reality.Mldsa65Verify);
                    }

                    query.Add("fp", EnumPropertyConverter.ToString(reality.Fingerprint));
                    query.Add("spx", $"/{RandomUtilities.Seq(15)}");

                    if (stream.Network == StreamNetwork.Raw)
                    {
                        query.Add("flow", EnumPropertyConverter.ToString(client.Flow));
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
            Host = inbound.Listen ?? DEFAULT_ADDRESS,
            Query = query.ToString(),
        };

        if (inbound.Port?.Single != null)
        {
            builder.Port = (int)inbound.Port.Single;
        }


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

    public override string FromInbound(VMessInbound inbound)
    {
        throw new NotImplementedException();
    }

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