using Xray.Config.Enums;
using Xray.Config.Models;
using Xray.Config.Share;

namespace Xray.Sdk.Tests;

public class VMessShareFormatterTests
{
    private readonly V2RayShareFormatter _shareFormatter = new();

    [Fact(DisplayName = "VMess Raw + Tls")]
    public void RAW_Tls_Test()
    {
        var client = new VMessClient()
        {
            Id = "26a28e07-8509-45b4-844a-6d881593d7de",
            Email = "example@test.com"
        };

        var inbound = new VMessInbound()
        {
            Port = new Port(10001),
            Tag = "VMess in",
            Settings = new Inbound.VMessSettings()
            {
                Clients = [client],
            },
            StreamSettings = new StreamSettings()
            {
                Network = StreamNetwork.Raw,
                Security = StreamSecurity.None,
                RawSettings = new RawSettings()
                {
                    Header = new HttpSettingsHeaders()
                    {
                        Request = new HttpRequest()
                        {
                            Path = ["/test"]
                        }
                    }
                },
            }
        };

        var link = _shareFormatter.CreateLink(inbound, client);

        Assert.Equal("vmess://eyJhZGQiOiIwLjAuMC4wIiwicG9ydCI6MTAwMDEsInYiOiIyIiwiYWxsb3dJbnNlY3VyZSI6ZmFsc2UsImZwIjoiIiwiaWQiOiIyNmEyOGUwNy04NTA5LTQ1YjQtODQ0YS02ZDg4MTU5M2Q3ZGUiLCJwcyI6IlZNZXNzIGluIiwicGF0aCI6Ii90ZXN0IiwidGxzIjoibm9uZSIsIm5ldCI6InJhdyIsInR5cGUiOiJodHRwIiwiaG9zdCI6IiIsImFpZCI6IjAifQ==", link);
    }
}