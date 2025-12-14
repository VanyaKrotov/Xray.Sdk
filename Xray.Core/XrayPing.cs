using System.Net.NetworkInformation;
using Xray.Config.Models;
using Xray.Core.Exceptions;
using Xray.Core.Utilities;
using Xray.Core.Wrappers;

namespace Xray.Core;

public static class XrayPing
{
    private static ICMPPingService _ping = new ICMPPingService();

    public static async Task<long> PingICMP(Models.PingOptions options)
    {
        var reply = await _ping.PingAsync(options.Host, options.Timeout);
        if (reply.Status != IPStatus.Success)
        {
            throw new Exception($"Ping end with status: {reply.Status}");
        }

        return reply.RoundtripTime;
    }

    public static async Task<long> Ping(Models.PingOptions options)
    {
        var response = await Task.Run(() => NativeWrapper.Ping(options.Port, options.TestingUrl));
        if (!response.IsSuccess)
        {
            throw new CoreException(response.Message, response.Code);
        }

        return long.Parse(response.Message);
    }

    public static async Task<long> Ping(XrayConfig config, Models.PingOptions options)
    {
        var response = await Task.Run(() => NativeWrapper.PingConfig(config.ToJson(), options.Port, options.TestingUrl));
        if (!response.IsSuccess)
        {
            throw new CoreException(response.Message, response.Code);
        }

        return long.Parse(response.Message);
    }

    public static Task<long> Ping(Outbound outbound, Models.PingOptions options)
    {
        var config = new XrayConfig()
        {
            Inbounds = new List<Inbound>()
            {
                new HttpInbound()
                {
                    Listen = "127.0.0.1",
                    Port = options.Port,
                    Tag = "in",
                }
            },
            Outbounds = new List<Outbound>()
            {
                outbound
            }
        };

        return Ping(config, options);
    }
}