using Xray.Config.Enums;
using Xray.Config.Models;
using Xray.Config.Share;
using Xray.Core;

// var config = new XrayConfig()
// {
//     Log = new LogConfig()
//     {
//         LogLevel = LogLevel.Error
//     },
//     Dns = new DnsConfig()
//     {
//         Servers = new List<DnsServer>()
//         {
//             new DnsServer()
//             {
//                 Address = "8.8.8.8"
//             }
//         }
//     },
//     Inbounds = new List<Inbound>()
//     {
//         new HttpInbound()
//         {
//             Port = new Port(8081),
//             Tag = "Tag",
//         },
//         new DokodemoDoorInbound()
//         {
//             Port = new Port(10000),
//             Tag = "Decodemo",
//             Settings = new Inbound.DokodemoDoorSettings()
//             {
//                 Network = new List<TransportProtocol>() {TransportProtocol.Tcp, TransportProtocol.Udp}
//             }
//         }
//     },
//     Outbounds = new List<Outbound>
//     {
//         new FreedomOutbound()
//         {
//             Tag = "freedom"
//         }
//     },
//     Api = new ApiConfig()
//     {
//         Tag = "api",
//         Listen = "localhost:10022",
//         Services = new List<string>()
//         {
//             ApiServices.Handler,
//             ApiServices.Logger,
//         }
//     },
//     Stats = new StatsConfig(),
//     Version = new VersionConfig()
//     {
//         Min = "25.9.12"
//     }
// };

// var processCore = new XrayProcessCore(new XrayProcessOptions()
// {
//     WorkingDirectory = "C:\\Users\\vanya\\Downloads\\Xray-windows-64"
// });

var libCore = new XrayLibCore(new XrayLibOptions()
{
  LibPath = "native/windows_amd64.dll"
});

// var json = @"
// {
//   ""log"": {
//     ""access"": ""access.log"",
//     ""error"": ""error.log"",
//     ""loglevel"": ""debug""
//   },

//   ""dns"": {
//     ""servers"": [
//       ""8.8.8.8"",
//       ""1.1.1.1"",
//       {
//         ""address"": ""localhost"",
//         ""port"": 53
//       }
//     ]
//   },

//   ""inbounds"": [
//     {
//       ""tag"": ""vmess-in"",
//       ""port"": 1080,
//       ""protocol"": ""vmess"",
//       ""settings"": {
//         ""clients"": [
//           {
//             ""id"": ""11111111-1111-1111-1111-111111111111"",
//             ""alterId"": 0
//           }
//         ]
//       },
//       ""streamSettings"": {
//         ""network"": ""tcp""
//       }
//     },
//     {
//       ""tag"": ""socks-in"",
//       ""port"": 1081,
//       ""protocol"": ""socks"",
//       ""settings"": {
//         ""auth"": ""noauth"",
//         ""udp"": true
//       }
//     },
//     {
//       ""tag"": ""http-in"",
//       ""port"": 1082,
//       ""protocol"": ""http"",
//       ""settings"": {}
//     }
//   ],

//   ""outbounds"": [
//     {
//       ""tag"": ""direct"",
//       ""protocol"": ""freedom"",
//       ""settings"": {}
//     },
//     {
//       ""tag"": ""block"",
//       ""protocol"": ""blackhole"",
//       ""settings"": {}
//     }
//   ],

//   ""routing"": {
//     ""domainStrategy"": ""AsIs"",
//     ""rules"": [
//       {
//         ""type"": ""field"",
//         ""domain"": [""geosite:category-ads""],
//         ""outboundTag"": ""block""
//       },
//       {
//         ""type"": ""field"",
//         ""ip"": [""geoip:private""],
//         ""outboundTag"": ""direct""
//       },
//       {
//         ""type"": ""field"",
//         ""inboundTag"": [""vmess-in"", ""socks-in"", ""http-in""],
//         ""outboundTag"": ""direct""
//       }
//     ]
//   }
// }
// ";


// var userUuid = libCore.GenerateUuidV4();
// var keys = libCore.GenerateX25519Keys("example");
var vlessInbound = new VlessInbound()
{
  Port = new Port(443),
  Tag = "vless-in",
  Settings = new()
  {
    Clients = [
      new () {
          Email = "test@example.com",
          Id = "0123456789abcdef-0123456789abcde",
          Flow = XtlsFlow.XtlsRprxVision,
        }
      ],
  },
  StreamSettings = new()
  {
    Network = StreamNetwork.Raw,
    Security = StreamSecurity.Reality,
    RealitySettings = new()
    {
      Target = "google.com:443",
      ServerNames = ["", "www.vk.com"],
      PrivateKey = "example key",
      ShortIds = ["", "0123456789abcdef"]
    },
  },
  Sniffing = new()
  {
    Enabled = true,
    DestOverride = [
      TrafficType.Http,
      TrafficType.Tls,
      TrafficType.Quic
    ],
    RouteOnly = true
  }
};

var v2rayShare = new V2RayShareFormatter();

var v2RayVlessLink = v2rayShare.FromInbound(vlessInbound, "test@example.com");

// var res = XrayConfig.FromJson(json);

// var processVersion = processCore.Version();

// processCore.Start(config);
// processCore.Stop();

var version = libCore.Version();

Console.WriteLine($"Version: {version}");

libCore.Stop();

Console.WriteLine("Press key to close this window");
Console.ReadKey();