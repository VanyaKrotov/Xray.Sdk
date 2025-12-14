using Xray.Config.Models;
using Xray.Core.Exceptions;
using Xray.Core.Models;
using Xray.Core.Wrappers;

namespace Xray.Core;

public static class XrayCore
{
    public static string Version => NativeWrapper.GetVersion();

    public static bool TryStartServer(XrayConfig config)
    {
        return NativeWrapper.StartServer(config.ToJson()).IsSuccess;
    }

    public static void StartServer(XrayConfig config)
    {
        HandleResponse(NativeWrapper.StartServer(config.ToJson()));
    }

    public static bool IsStartedServer() => NativeWrapper.IsServerStarted();

    public static int PingConfig(XrayConfig config, PingOptions options)
    {
        ValidatePingOptions(options);

        var result = NativeWrapper.PingConfig(config.ToJson(), options.Port, options.TestingUrl);

        HandleResponse(result);

        return int.Parse(result.Message);
    }

    public static int Ping(PingOptions options)
    {
        ValidatePingOptions(options);

        var result = NativeWrapper.Ping(options.Port, options.TestingUrl);

        HandleResponse(result);

        return int.Parse(result.Message);
    }

    private static void ValidatePingOptions(PingOptions options)
    {
        if (string.IsNullOrEmpty(options.TestingUrl))
        {
            throw new ArgumentException("TestingUrl must be filled");
        }
    }

    private static void HandleResponse(NativeWrapper.TypedResponse response)
    {
        if (response.IsSuccess)
        {
            return;
        }

        throw new CoreException(response.Message, response.Code);
    }
}
