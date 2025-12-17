using System.Collections.Specialized;
using System.Net.Http.Headers;
using Xray.Config.Models;
using Xray.Config.Utilities;

namespace Xray.Config.Formatters;

public static class V2RayFormatter
{
    public static string GenVlessLink(VlessInbound inbound, string email)
    {
        var client = inbound.Settings.Clients.SingleOrDefault(x => x.Email == email);
        if (client == null)
        {
            throw new ArgumentException("Client not found");
        }

        return GenVlessLink(inbound, client);
    }

    public static string GenVlessLink(VlessInbound inbound, VlessClient client)
    {
        var query = new NameValueCollection()
        {
            { "type", EnumMemberConvert.ToString(inbound.StreamSettings!.Network) }
        };

        var stream = inbound.StreamSettings!;

        switch (inbound.StreamSettings.Network)
        {
            case Enums.StreamNetwork.Raw:
                {
                    var raw = stream.RawSettings!;
                    if (raw.Header!.Type == Enums.RawHeadersType.Http)
                    {
                        var request = raw.Header.Request;

                        query.Add("path", request.Path.First());
                        query.Add("headerType", "http");

                        if (GetHost(request.Headers, out var host))
                        {
                            query.Add("host", host);
                        }
                    }

                    break;
                }

            case Enums.StreamNetwork.Kcp:
                {
                    var kcp = stream.KcpSettings!;

                    query.Add("headerType", EnumMemberConvert.ToString(kcp.Header.Type));
                    query.Add("seed", kcp.Seed);

                    break;
                }

            case Enums.StreamNetwork.Ws:
                {
                    var ws = stream.WsSettings!;

                    query.Add("path", ws.Path);

                    if (!string.IsNullOrEmpty(ws.Host))
                    {
                        query.Add("host", ws.Host);
                    }
                    else if (GetHost(ws.Headers, out var host))
                    {
                        query.Add("host", host);
                    }

                    break;
                }

            case Enums.StreamNetwork.Grpc:
                {
                    var grpc = stream.GrpcSettings!;
                    if (!string.IsNullOrEmpty(grpc?.ServiceName))
                    {
                        query.Add("serviceName", grpc.ServiceName);
                    }

                    if (!string.IsNullOrEmpty(grpc?.Authority))
                    {
                        query.Add("authority", grpc.Authority);
                    }

                    if (grpc!.MultiMode)
                    {
                        query.Add("mode", "multi");
                    }

                    break;
                }

            case Enums.StreamNetwork.HttpUpgrade:
                {
                    query.Add("path", stream.HttpUpgradeSettings!.Path);
                    if (!string.IsNullOrEmpty(stream.HttpUpgradeSettings.Host))
                    {
                        query.Add("host", stream.HttpUpgradeSettings.Host);
                    }
                    else if (GetHost(stream.HttpUpgradeSettings!.Headers!, out var host))
                    {
                        query.Add("host", host);
                    }

                    break;
                }

            case Enums.StreamNetwork.XHttp:
                {
                    query.Add("path", stream.XHttpSettings.Path);

                    if (!string.IsNullOrEmpty(stream.XHttpSettings.Host))
                    {
                        query.Add("host", stream.XHttpSettings.Host);
                    }
                    else if (GetHost(stream.XHttpSettings!.Headers!, out var host))
                    {
                        query.Add("host", host);
                    }

                    query.Add("mode", stream.XHttpSettings.Mode ?? "auto");

                    break;
                }
        }

        query.Add("security", EnumMemberConvert.ToString(stream.Security ?? Enums.StreamSecurity.None));

        switch (stream.Security)
        {
            case Enums.StreamSecurity.Tls:
                {
                    var tls = stream.TlsSettings!;
                    var alpn = string.Join(",", tls.Alpn ?? new List<string>());
                    if (!string.IsNullOrEmpty(alpn))
                    {
                        query.Add("alpn", alpn);
                    }

                    if (!string.IsNullOrEmpty(tls.ServerName))
                    {
                        query.Add("sni", tls.ServerName);
                    }

                    if (tls.Fingerprint != null)
                    {
                        query.Add("fp", EnumMemberConvert.ToString((Enums.Fingerprint)tls.Fingerprint));
                    }

                    if (tls.AllowInsecure)
                    {
                        query.Add("allowInsecure", "1");
                    }

                    break;
                }

            case Enums.StreamSecurity.Reality:
                {
                    var reality = stream.RealitySettings!;
                    if (reality.ServerNames != null)
                    {
                        query.Add("sni", reality.ServerNames[Random.Shared.Next(reality.ServerNames.Count)]);
                    }

                    if (!string.IsNullOrEmpty(reality.PublicKey))
                    {
                        query.Add("pbk", reality.PublicKey);
                    }

                    if (reality.ShortIds != null)
                    {
                        query.Add("sid", reality.ShortIds[Random.Shared.Next(reality.ShortIds.Count)]);
                    }

                    if (reality.Fingerprint != null)
                    {
                        query.Add("fp", EnumMemberConvert.ToString((Enums.Fingerprint)reality.Fingerprint));
                    }

                    if (!string.IsNullOrEmpty(reality.Mldsa65Verify))
                    {
                        query.Add("pqv", reality.Mldsa65Verify);
                    }

                    query.Add("spx", $"/{string.Join("", RandomUtilities.Seq(new Random(), 15))}");

                    break;
                }
        }

        if ((stream.Security == Enums.StreamSecurity.Tls || stream.Security == Enums.StreamSecurity.Reality) && stream.Network == Enums.StreamNetwork.Raw && client.Flow != Enums.Flow.None)
        {
            query.Add("flow", EnumMemberConvert.ToString(client.Flow));
        }

        return $"vless://{client.Id}@{inbound.Listen}:{inbound.Port}?{query}#{Uri.EscapeDataString(client.Email)}";
    }

    public class VlessLinkOptions
    {
        public required string Uuid { get; set; }

        public required int Port { get; set; }

        public required int Address { get; set; }

        public string Remark { get; set; } = string.Empty;

        public required string Security { get; set; }

        public string? Type { get; set; }

        public string? HeaderType { get; set; }

        public string? ServerName { get; set; }

        public string? Host { get; set; }

        public string? Path { get; set; }

        public string? Alpn { get; set; }

        public string? Fingerprint { get; set; }

        public string? Flow { get; set; }

        public string? PublicKey { get; set; }

        public string? ShortId { get; set; }

        public string? Spx { get; set; }

        public string? Pqv { get; set; }
    }

    private static bool GetHost(HttpHeaders headers, out string? host)
    {
        if (headers.TryGetValues("host", out var hosts) && hosts.Any())
        {
            host = hosts.First();

            return true;
        }

        host = null;

        return false;
    }
}

public abstract class ProtocolSharedFabric
{
    public abstract string SharedLink { get; }

    protected Dictionary<string, string> GetStreamSettingsParams(StreamSettings settings)
    {
        var dict = new Dictionary<string, string>()
        {
            { "type", EnumMemberConvert.ToString(settings.Network) },
            { "security", EnumMemberConvert.ToString(settings.Security ?? Enums.StreamSecurity.None) },
        };

        switch (settings.Network)
        {
            case Enums.StreamNetwork.Raw:
                ConvertRawParams(settings.RawSettings!, dict);

                break;

            case Enums.StreamNetwork.Grpc:
                ConvertGrpcParams(settings.GrpcSettings!, dict);

                break;

            case Enums.StreamNetwork.HttpUpgrade:
                ConvertHttpUpdateParams(settings.HttpUpgradeSettings!, dict);

                break;

            case Enums.StreamNetwork.Kcp:
                ConvertKcpParams(settings.KcpSettings!, dict);

                break;

            case Enums.StreamNetwork.Ws:
                ConvertWsParams(settings.WsSettings!, dict);

                break;

            case Enums.StreamNetwork.XHttp:
                ConvertXHttpParams(settings.XHttpSettings!, dict);

                break;
        }

        switch (settings.Security)
        {
            case Enums.StreamSecurity.Tls:
                ConvertTlsParams(settings.TlsSettings!, dict);

                break;

            case Enums.StreamSecurity.Reality:
                ConvertRealityParams(settings.RealitySettings!, dict);

                break;
        }

        return dict;
    }

    protected virtual void ConvertParams(StreamSettings settings, Dictionary<string, string> dict) { }

    protected abstract void ConvertRawParams(RawSettings settings, Dictionary<string, string> dict);

    protected abstract void ConvertTlsParams(TlsSettings settings, Dictionary<string, string> dict);

    protected abstract void ConvertRealityParams(RealitySettings settings, Dictionary<string, string> dict);

    protected abstract void ConvertXHttpParams(XHttpSettings settings, Dictionary<string, string> dict);

    protected abstract void ConvertKcpParams(KcpSettings settings, Dictionary<string, string> dict);

    protected abstract void ConvertGrpcParams(GrpcSettings settings, Dictionary<string, string> dict);

    protected abstract void ConvertWsParams(WsSettings settings, Dictionary<string, string> dict);

    protected abstract void ConvertHttpUpdateParams(HttpUpgradeSettings settings, Dictionary<string, string> dict);


    private string ParseHttpHeadersHost(HttpHeaders? headers)
    {
        if (headers != null && headers.TryGetValues("host", out var hosts) && hosts.Any())
        {
            return hosts.First();
        }

        return "";
    }

    protected Dictionary<string, string> ConvertHostedParams(HostedSettings settings)
    {
        return new Dictionary<string, string>()
        {
            {"path", settings.Path ?? "/"},
            {"host", string.IsNullOrEmpty(settings.Host) ? ParseHttpHeadersHost(settings.Headers) : settings.Host}
        };
    }

    protected Dictionary<string, string> GetSettingsHeadersParams(SettingsHeaders settings)
    {
        if (settings.Type != Enums.RawHeadersType.Http || settings.Request == null)
        {
            return new();
        }

        return new Dictionary<string, string>()
        {
            {"path", settings.Request.Path?.FirstOrDefault() ?? ""},
            {"host", ParseHttpHeadersHost(settings.Request.Headers)}
        };
    }
}


public class VlessSharedFabric : ProtocolSharedFabric
{
    protected readonly VlessInbound _inbound;

    protected readonly VlessClient _client;

    protected readonly VlessSharedOptions _options;

    public VlessSharedFabric(VlessInbound inbound, VlessClient client, VlessSharedOptions option)
    {
        _inbound = inbound;
        _client = client;
        _options = option;
    }

    public VlessSharedFabric(VlessInbound inbound, string email, VlessSharedOptions option)
    {
        var client = inbound.Settings.Clients.SingleOrDefault(c => c.Email == email);
        if (client == null)
        {
            throw new ArgumentException($"Client by email = {email} not found");
        }

        _inbound = inbound;
        _options = option;
        _client = client;
    }

    public virtual string GetRemark()
    {
        return "";
    }

    public override string SharedLink => throw new NotImplementedException();

    protected override void ConvertParams(StreamSettings settings, Dictionary<string, string> dict)
    {
        base.ConvertParams(settings, dict);

        if (settings.Network == Enums.StreamNetwork.Raw)
        {

        }
    }

    protected override void ConvertGrpcParams(GrpcSettings settings, Dictionary<string, string> dict)
    {
        dict["serviceName"] = settings.ServiceName ?? "";
        dict["authority"] = settings.Authority ?? "";

        if (settings.MultiMode)
        {
            dict["mode"] = "multi";
        }
    }

    protected override void ConvertHttpUpdateParams(HttpUpgradeSettings settings, Dictionary<string, string> dict)
    {
        ConvertHostedParams(settings, dict);
    }

    protected override void ConvertKcpParams(KcpSettings settings, Dictionary<string, string> dict)
    {
        dict["headerType"] = EnumMemberConvert.ToString(settings.Header.Type);
        dict["seed"] = settings.Seed ?? "";
    }

    protected override void ConvertRawParams(RawSettings settings, Dictionary<string, string> options)
    {
        GetSettingsHeadersParams(settings.Header, options);
    }

    protected override void ConvertRealityParams(RealitySettings settings, Dictionary<string, string> dict)
    {

    }

    protected override void ConvertTlsParams(TlsSettings settings, Dictionary<string, string> dict)
    {
        if (settings.Alpn != null)
        {
            dict["alpn"] = string.Join(",", settings.Alpn);
        }

        if (!string.IsNullOrEmpty(settings.ServerName))
        {
            dict["sni"] = settings.ServerName;
        }

        if (settings.Fingerprint != null)
        {
            dict["fp"] = EnumMemberConvert.ToString((Enums.Fingerprint)settings.Fingerprint);
        }

        if (settings.AllowInsecure)
        {
            dict["allowInsecure"] = "1";
        }
    }

    protected override void ConvertWsParams(WsSettings settings, Dictionary<string, string> dict)
    {
        ConvertHostedParams(settings, dict);
    }

    protected override void ConvertXHttpParams(XHttpSettings settings, Dictionary<string, string> dict)
    {
        ConvertHostedParams(settings, dict);

        dict["mode"] = settings.Mode ?? "";
    }
}

public class VlessSharedOptions
{
    public required string Remark { get; set; }

    public required string Address { get; set; }

    public int Port { get; set; }

    public string? ServerName { get; set; }

    public string? Host { get; set; }

    public string? Path { get; set; }

    public Enums.StreamSecurity? Security { get; set; }

    public Enums.Fingerprint? Fingerprint { get; set; }

    public string? FragmentSettings { get; set; }

    public string? NoiseSettings { get; set; }

    public bool AllowInsecure { get; set; }

    public bool MuxEnabled { get; set; }

    public bool RandomUseragent { get; set; }

    public bool UseSNIAsHost { get; set; }
}