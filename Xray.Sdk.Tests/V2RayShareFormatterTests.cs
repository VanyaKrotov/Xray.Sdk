using Xray.Config.Enums;
using Xray.Config.Models;
using Xray.Config.Share;

namespace XTLS_SDK.Tests;

public class V2RayShareFormatterTests
{
    private readonly V2RayShareFormatter _shareFormatter = new();

    [Fact]
    public void Vless_RAW_NONE_Test()
    {
        var client = new VlessClient()
        {
            Email = "example@test.com",
            Id = "222232-32-3-2-3-23--23-2-3",
            Flow = XtlsFlow.XtlsRprxVision,
        };

        var inbound = new VlessInbound()
        {
            Port = new Port(10001),
            Tag = "Vless in",
            Settings = new Inbound.VlessSettings()
            {
                Clients = [client]
            },
            StreamSettings = new StreamSettings()
            {
                Network = StreamNetwork.Raw,
                Security = StreamSecurity.None
            }
        };

        var link = _shareFormatter.FromInbound(inbound, client);

        Assert.Equal("vless://222232-32-3-2-3-23--23-2-3@0.0.0.0:10001/?encryption=none&flow=xtls-rprx-vision&type=raw&security=none#Vless in", link);
    }

    [Fact]
    public void Vless_RAW_TLS_Test()
    {
        var client = new VlessClient()
        {
            Email = "example@test.com",
            Id = "222232-32-3-2-3-23--23-2-3",
            Flow = XtlsFlow.XtlsRprxVision,
        };

        var inbound = new VlessInbound()
        {
            Port = new Port(10001),
            Tag = "Vless in",
            Settings = new Inbound.VlessSettings()
            {
                Clients = [client],
                Decryption = VlessDecryption.None
            },
            StreamSettings = new StreamSettings()
            {
                Network = StreamNetwork.Raw,
                Security = StreamSecurity.Tls,
                RawSettings = new RawSettings()
                {
                    AcceptProxyProtocol = false,
                    Header = new HttpSettingsHeaders()
                    {
                        Request = new HttpRequest()
                        {
                            Path = ["/test"]
                        }
                    }
                },
                TlsSettings = new TlsSettings()
                {
                    AllowInsecure = true,
                    Alpn = ["h2", "h3"],
                    ServerName = "www.google.com",
                    Fingerprint = Fingerprint.iOS,
                    Certificates = new List<TlsCertificate>()
                    {
                        new TlsCertificate()
                        {
                        KeyFile = "test.key",
                        CertificateFile = "test.pem"
                        }
                    },
                }
            }
        };

        var link = _shareFormatter.FromInbound(inbound, client);

        Assert.Equal("vless://222232-32-3-2-3-23--23-2-3@0.0.0.0:10001/?encryption=none&flow=xtls-rprx-vision&type=raw&path=/test&host=&headerType=http&security=tls&alpn=h2,h3&sni=www.google.com&fp=ios&allowInsecure=1#Vless in", link);
    }

    [Fact]
    public void Vless_RAW_Reality_Test()
    {
        var client = new VlessClient()
        {
            Email = "example@test.com",
            Id = "222232-32-3-2-3-23--23-2-3",
            Flow = XtlsFlow.XtlsRprxVision,
        };

        var inbound = new VlessInbound()
        {
            Port = new Port(10001),
            Tag = "Vless in",
            Settings = new Inbound.VlessSettings()
            {
                Clients = [client],
                Decryption = VlessDecryption.None
            },
            StreamSettings = new StreamSettings()
            {
                Network = StreamNetwork.Raw,
                Security = StreamSecurity.Reality,
                RawSettings = new RawSettings()
                {
                    AcceptProxyProtocol = false,
                    Header = new HttpSettingsHeaders()
                    {
                        Request = new HttpRequest()
                        {
                            Path = ["/test"]
                        }
                    }
                },
                RealitySettings = new RealitySettings()
                {
                    Target = "www.google.com",
                    Fingerprint = Fingerprint.iOS,
                    Show = false,
                    ServerNames = ["ya.com", "www.yandex.com"],
                    SpiderX = "/test",
                    Password = "342ojh42i3g4io23h4iu234jhc23fc432",
                    ShortIds = ["abcd"]
                }
            }
        };

        var link = _shareFormatter.FromInbound(inbound, client);

        Assert.Matches(@"^vless:\/\/[A-Za-z0-9\-]+@0\.0\.0\.0:10001\/\?encryption=none&flow=xtls-rprx-vision&pbk=[A-Za-z0-9]+&sid=abcd&spx=\/[A-Za-z0-9]+&type=raw&path=\/test&host=&headerType=http&security=reality&sni=www\.yandex\.com&fp=ios#Vless in$", link);
    }
}
