using Xray.Config.Models;
using Xray.Core.Exceptions;
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

    private static void HandleResponse(NativeWrapper.TypedResponse response)
    {
        if (response.IsSuccess)
        {
            return;
        }

        throw new CoreException(response.Message, response.Code);
    }
}
