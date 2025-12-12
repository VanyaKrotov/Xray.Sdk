using Xray.Config.Enums;
using Xray.Config.Models;
using Xray.Config.Utilities;

var config = new XRayConfig()
{
    Log = new LogConfig()
    {
        LogLevel = LogLevel.Error
    },
    Inbounds = new List<Inbound>()
    {
        new HttpInbound()
        {
            Port = 8080,
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
        new VlessOutbound() { },
        new WireguardOutbound() {}
    },
    Api = new ApiConfig()
    {
        Listen = "http://localhost:8080",
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

var export = XrayConfigJsonSerializer.Serialize(config);
var import = XrayConfigJsonSerializer.Deserialize(export);

Console.WriteLine(export);

// var xtlsClient = new XtlsApi("http://127.0.0.1:8080");

// var res = await xtlsClient.GetInboundUsers(new GetUsersOptions() { Tag = "default" });

// Console.WriteLine(res);

// // await xtlsClient.RemoveUser("default", "test@test.com");

// var count = await xtlsClient.GetInboundUsersCount("default");

// Console.WriteLine(res);

// // await xtlsClient.AddVlessUser(new AddVlessUser()
// // {
// //     Email = "test@test.com",
// //     Tag = "default",
// //     Uuid = "test-test-test-test-test-test",
// // });

// res = await xtlsClient.GetInboundUsers(new GetUsersOptions() { Tag = "default" });


// Console.WriteLine(res);

// var stats = await xtlsClient.GetSysStats();

// var online = await xtlsClient.GetUserOnlineStatus("love@example.com");

// Console.WriteLine(online);
