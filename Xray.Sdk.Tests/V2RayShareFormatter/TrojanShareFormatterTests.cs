using Xray.Config.Enums;
using Xray.Config.Models;
using Xray.Config.Share;

namespace Xray.Sdk.Tests;

public class TrojanShareFormatterTests
{
    private readonly V2RayShareFormatter _shareFormatter = new();

    [Fact(DisplayName = "Trojan Raw + None")]
    public void RAW_NONE_Test()
    {
        var client = new TrojanClient()
        {
            Email = "example@test.com",
            Password = "password",
        };

        var inbound = new TrojanInbound()
        {
            Port = new Port(10001),
            Tag = "Trojan in",
            Settings = new Inbound.TrojanSettings()
            {
                Clients = [client],
            },
            StreamSettings = new StreamSettings()
            {
                Network = StreamNetwork.Raw,
                Security = StreamSecurity.None,
            }
        };

        var link = _shareFormatter.CreateLink(inbound, client);

        Assert.Equal("trojan://password@0.0.0.0:10001/?type=raw&security=none#Trojan in", link);
    }

    [Fact(DisplayName = "Trojan Raw + Tls")]
    public void RAW_TLS_Test()
    {
        var client = new TrojanClient()
        {
            Email = "example@test.com",
            Password = "password",
        };

        var inbound = new TrojanInbound()
        {
            Port = new Port(10001),
            Tag = "Trojan in",
            Settings = new Inbound.TrojanSettings()
            {
                Clients = [client],
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

        var link = _shareFormatter.CreateLink(inbound, client);

        Assert.Equal("trojan://password@0.0.0.0:10001/?type=raw&path=/test&host=&headerType=http&security=tls&alpn=h2,h3&sni=www.google.com&fp=ios&allowInsecure=1#Trojan in", link);
    }

    [Fact(DisplayName = "Trojan Raw + Reality")]
    public void RAW_Reality_Test()
    {
        var client = new TrojanClient()
        {
            Email = "example@test.com",
            Password = "password",
        };

        var inbound = new TrojanInbound()
        {
            Port = new Port(10001),
            Tag = "Trojan in",
            Settings = new Inbound.TrojanSettings()
            {
                Clients = [client],
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
                    ServerNames = ["www.yandex.com"],
                    SpiderX = "/test",
                    Password = "342ojh42i3g4io23h4iu234jhc23fc432",
                    ShortIds = ["abcd"]
                }
            }
        };

        var link = _shareFormatter.CreateLink(inbound, client);

        Assert.Matches(@"^trojan:\/\/password@0\.0\.0\.0\:10001\/\?pbk=342ojh42i3g4io23h4iu234jhc23fc432&sid=abcd&spx=\/[A-Za-z0-9]+&type=raw&path=\/test&host=&headerType=http&security=reality&sni=www\.yandex\.com&fp=ios#Trojan in$", link);
    }
}
