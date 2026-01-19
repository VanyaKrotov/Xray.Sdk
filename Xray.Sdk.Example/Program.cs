using Xray.Config.Enums;
using Xray.Config.Models;
using Xray.Core;

var config = new XrayConfig()
{
    Log = new LogConfig()
    {
        LogLevel = LogLevel.Error
    },
    Dns = new DnsConfig()
    {
        Servers = new List<DnsServer>()
        {
            new DnsServer()
            {
                Address = "8.8.8.8"
            }
        }
    },
    Inbounds = new List<Inbound>()
    {
        new HttpInbound()
        {
            Port = 8081,
            Tag = "Tag",
        },
        new DokodemoDoorInbound()
        {
            Port = 100,
            Tag = "Decodemo",
            Settings = new Inbound.DokodemoDoorSettings()
            {
                Network = new List<TransportProtocol>() {TransportProtocol.Tcp, TransportProtocol.Udp}
            }
        }
    },
    Outbounds = new List<Outbound>
    {
        new FreedomOutbound()
        {
            Tag = "freedom"
        }
    },
    Api = new ApiConfig()
    {
        Tag = "api",
        Listen = "localhost:10022",
        Services = new List<string>()
        {
            ApiServices.Handler,
            ApiServices.Logger,
        }
    },
    Stats = new StatsConfig(),
    Version = new VersionConfig()
    {
        Min = "25.9.12"
    }
};

// var processCore = new XrayProcessCore(new XrayProcessOptions()
// {
//     WorkingDirectory = "C:\\Users\\vanya\\Downloads\\Xray-windows-64"
// });

var libCore = new XrayLibCore(new XrayLibOptions()
{
    LibPath = "native/windows_amd64.dll"
});

// var processVersion = processCore.Version();

// processCore.Start(config);
// processCore.Stop();

var version = libCore.Version();

Console.WriteLine($"Version: {version}");

libCore.Start(config);
libCore.Stop();

Console.WriteLine("Press key to close this window");
Console.ReadKey();