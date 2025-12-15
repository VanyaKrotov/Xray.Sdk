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
                    var ws = stream.WSSettings!;

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
                    var grpc = stream.GRPCSettings!;
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