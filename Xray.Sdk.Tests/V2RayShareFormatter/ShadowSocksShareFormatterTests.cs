using Xray.Config.Enums;
using Xray.Config.Models;
using Xray.Config.Share;

namespace Xray.Sdk.Tests;

public class ShadowSocksShareFormatterTests
{
    private readonly V2RayShareFormatter _shareFormatter = new();

    [Fact(DisplayName = "ShadowSocks Raw + None")]
    public void RAW_NONE_Test()
    {
        var client = new ShadowSocksClient()
        {
            Email = "example@test.com",
            Password = "password",
            Method = EncryptionMethod.Chacha20Poly1305
        };

        var inbound = new ShadowSocksInbound()
        {
            Port = new Port(10001),
            Tag = "ShadowSocks in",
            Settings = new Inbound.ShadowSocksSettings()
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

        Assert.Equal("ss://OnBhc3N3b3Jk@0.0.0.0:10001/?type=raw#ShadowSocks in", link);
    }

    [Fact(DisplayName = "ShadowSocks Raw + Tls")]
    public void RAW_TLS_Test()
    {
        var client = new ShadowSocksClient()
        {
            Email = "example@test.com",
            Password = "password",
            Method = EncryptionMethod.Chacha20Poly1305
        };

        var inbound = new ShadowSocksInbound()
        {
            Port = new Port(10001),
            Tag = "ShadowSocks in",
            Settings = new Inbound.ShadowSocksSettings()
            {
                Clients = [client],
            },
            StreamSettings = new StreamSettings()
            {
                Network = StreamNetwork.Raw,
                Security = StreamSecurity.Tls,
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

        Assert.Equal("ss://OnBhc3N3b3Jk@0.0.0.0:10001/?type=raw&alpn=h2,h3&sni=www.google.com&fp=ios&allowInsecure=1#ShadowSocks in", link);
    }
}
