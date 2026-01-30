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
var client = new VMessClient()
{
  Email = "example@test.com",
  Id = "26a28e07-8509-45b4-844a-6d881593d7de",
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

var _shareFormatter = new V2RayShareFormatter();

var tcpTlsLink = _shareFormatter.CreateLink(inbound, client);

var _client = new VlessClient()
{
  Email = "example@test.com",
  Id = "222232-32-3-2-3-23--23-2-3",
  Flow = XtlsFlow.XtlsRprxVision,
};

var _inbound = new VlessInbound()
{
  Port = new Port(10001),
  Tag = "Vless in",
  Settings = new Inbound.VlessSettings()
  {
    Clients = [_client],
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

var link = _shareFormatter.CreateLink(_inbound, _client);

var inb = _shareFormatter.Parse(tcpTlsLink);

// var res = XrayConfig.FromJson(json);

// var processVersion = processCore.Version();

// processCore.Start(config);
// processCore.Stop();

var version = libCore.Version();

Console.WriteLine(tcpTlsLink);
Console.WriteLine($"Version: {version}");

Console.WriteLine("Press key to close this window");
Console.ReadKey();