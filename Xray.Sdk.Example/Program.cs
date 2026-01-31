using Xray.Config.Enums;
using Xray.Config.Models;
using Xray.Config.Share;
using Xray.Core;

var libCore = new XrayLibCore(new XrayLibOptions()
{
  LibPath = "native/windows_amd64.dll",
  // LibPath = "native/darwin_arm64.dylib",
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
// var client = new VMessClient()
// {
//   Email = "example@test.com",
//   Id = "26a28e07-8509-45b4-844a-6d881593d7de",
// };

// var inbound = new VMessInbound()
// {
//   Port = new Port(10001),
//   Tag = "VMess in",
//   Settings = new Inbound.VMessSettings()
//   {
//     Clients = [client],
//   },
//   StreamSettings = new StreamSettings()
//   {
//     Network = StreamNetwork.Raw,
//     Security = StreamSecurity.None,
//     RawSettings = new RawSettings()
//     {
//       AcceptProxyProtocol = false,
//       Header = new HttpSettingsHeaders()
//       {
//         Request = new HttpRequest()
//         {
//           Path = ["/test"]
//         }
//       }
//     },
//     RealitySettings = new RealitySettings()
//     {
//       Target = "www.google.com",
//       Fingerprint = Fingerprint.iOS,
//       Show = false,
//       ServerNames = ["ya.com", "www.yandex.com"],
//       SpiderX = "/test",
//       Password = "342ojh42i3g4io23h4iu234jhc23fc432",
//       ShortIds = ["abcd"]
//     }
//   }
// };

var _shareFormatter = new V2RayShareFormatter();

// var tcpTlsLink = _shareFormatter.CreateLink(inbound, client);

// var client = new ShadowSocksClient()
// {
//   Email = "example@test.com",
//   Password = "password",
//   Method = EncryptionMethod.Chacha20Poly1305
// };

// var inbound = new ShadowSocksInbound()
// {
//   Port = new Port(10001),
//   Tag = "ShadowSocks in",
//   Settings = new Inbound.ShadowSocksSettings()
//   {
//     Clients = [client],
//   },
//   StreamSettings = new StreamSettings()
//   {
//     Network = StreamNetwork.Raw,
//     Security = StreamSecurity.None,
//   }
// };

// var link = _shareFormatter.CreateLink(inbound, client);

Console.WriteLine("Enter configuration:");
string? input;

do
{
  input = Console.ReadLine();
  if (string.IsNullOrEmpty(input))
  {
    Console.WriteLine("Failed to load config");
  }

} while (string.IsNullOrEmpty(input));

var config = new XrayConfig()
{
  Log = new LogConfig()
  {
    LogLevel = LogLevel.Warning,
  },
  Inbounds =
  [
    new HttpInbound()
    {
      Port = new Port(10023),
      Tag = "http-in",
      Listen = "127.0.0.1",
    }
  ],
  Outbounds = [
    _shareFormatter.Parse(input)
  ],
};


// var res = XrayConfig.FromJson(json);

var version = libCore.Version();

libCore.Start(config);

Console.WriteLine("Server started");
Console.ReadKey();

Console.WriteLine($"Version: {version}");

libCore.Stop();

// Console.WriteLine(tcpTlsLink);

Console.WriteLine("Press key to close this window");
Console.ReadKey();