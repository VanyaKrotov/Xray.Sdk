using System.Runtime.InteropServices;
using Xray.Core.Exceptions;

namespace Xray.Core.Wrappers;

public static class NativeWrapper
{

#if WINDOWS
    private const string LIB_NAME = "NativeWrapper.dll";
#elif OSX
    private const string LIB_NAME = "NativeWrapper.dylib";
#elif LINUX
    private const string LIB_NAME = "NativeWrapper.so";
#else
    private const string LIB_NAME = "NativeWrapper.dylib";
#endif

    public static TypedResponse StartServer(string jsonConfig)
    {
        return ParseTypedResponse(StartServer(jsonConfig));

        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr StartServer(string jsonConfig);
    }

    public static TypedResponse StopServer()
    {
        return ParseTypedResponse(StopServer());

        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr StopServer();
    }

    public static bool IsServerStarted()
    {
        return IsStarted() == 1;

        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        static extern int IsStarted();
    }

    public static TypedResponse PingConfig(string jsonConfig, int port, string testingUrl)
    {
        return ParseTypedResponse(PingConfig(jsonConfig, port, testingUrl));

        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr PingConfig(string jsonConfig, int port, string testingUrl);
    }

    public static TypedResponse Ping(int port, string testingUrl)
    {
        return ParseTypedResponse(Ping(port, testingUrl));

        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr Ping(int port, string testingUrl);
    }

    public static X25519Keys Gen25519Keys(int port, string testingUrl)
    {
        var result = ParseTypedResponse(Curve25519Genkey(port, testingUrl));
        if (!result.IsSuccess)
        {
            throw new CoreException(result.Message, result.Code);
        }

        var data = result.Message.Split("|");

        return new X25519Keys()
        {
            PrivateKey = data[0],
            Password = data[1],
            Hash = data[2],
        };

        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr Curve25519Genkey(int port, string testingUrl);
    }

    public class X25519Keys
    {
        public required string PrivateKey { get; set; }

        public required string Password { get; set; }

        public required string Hash { get; set; }
    }

    public static string GetVersion()
    {
        return Marshal.PtrToStringAnsi(GetXrayCoreVersion())!;

        [DllImport(LIB_NAME, CallingConvention = CallingConvention.Cdecl)]
        static extern IntPtr GetXrayCoreVersion();
    }

    public class TypedResponse
    {
        public int Code { get; set; }

        public string Message { get; set; } = string.Empty;

        public bool IsSuccess => Code == 0;
    }

    private static TypedResponse ParseTypedResponse(IntPtr ptr)
    {
        string raw = Marshal.PtrToStringAnsi(ptr) ?? "";
        var parts = raw.Split('|', 2);

        return new TypedResponse() { Code = int.TryParse(parts[0], out var c) ? c : -1, Message = parts.Length > 1 ? parts[1] : "" };
    }
}