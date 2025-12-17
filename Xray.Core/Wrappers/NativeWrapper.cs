using System.Runtime.InteropServices;

namespace Xray.Core.Wrappers;

public class NativeWrapper : IDisposable
{
    private IntPtr _libHandle;

    private delegate IntPtr StartDelegate(string uuid, string jsonConfig);
    private delegate IntPtr StopDelegate(string uuid);
    private delegate int IsStartedDelegate(string uuid);
    private delegate IntPtr GetVersionDelegate();
    private StartDelegate _start;
    private StopDelegate _stop;
    private IsStartedDelegate _isStarted;
    private GetVersionDelegate _getVersion;

    private readonly Dictionary<OSPlatform, string> _platformExtensions = new()
    {
        { OSPlatform.OSX, ".dylib" },
        { OSPlatform.Linux, ".so" },
        { OSPlatform.Windows, ".dll" },
    };

    private readonly string LIB_NAME = "NativeWrapper";

    public NativeWrapper()
    {
        var platformExt = _platformExtensions.SingleOrDefault(x => RuntimeInformation.IsOSPlatform(x.Key)).Value;
        if (string.IsNullOrEmpty(platformExt))
        {
            throw new PlatformNotSupportedException();
        }

        _libHandle = NativeLibrary.Load(LIB_NAME + platformExt);

        _start = Marshal.GetDelegateForFunctionPointer<StartDelegate>(NativeLibrary.GetExport(_libHandle, "Start"));
        _stop = Marshal.GetDelegateForFunctionPointer<StopDelegate>(NativeLibrary.GetExport(_libHandle, "Stop"));
        _isStarted = Marshal.GetDelegateForFunctionPointer<IsStartedDelegate>(NativeLibrary.GetExport(_libHandle, "IsStarted"));
        _getVersion = Marshal.GetDelegateForFunctionPointer<GetVersionDelegate>(NativeLibrary.GetExport(_libHandle, "GetXrayCoreVersion"));
    }

    public TypedResponse Start(Guid guid, string jsonConfig)
    {
        return ParseTypedResponse(_start(guid.ToString(), jsonConfig));
    }

    public TypedResponse Stop(Guid guid)
    {
        return ParseTypedResponse(_stop(guid.ToString()));
    }

    public bool IsStarted(Guid guid)
    {
        return _isStarted(guid.ToString()) == 1;
    }

    public string GetVersion()
    {
        return Marshal.PtrToStringAnsi(_getVersion())!;
    }

    public void Dispose()
    {
        if (_libHandle != IntPtr.Zero)
        {
            NativeLibrary.Free(_libHandle);
            _libHandle = IntPtr.Zero;
        }
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
        string[]? parts = raw.Split('|', 2);

        return new TypedResponse()
        {
            Code = int.TryParse(parts[0], out var c) ? c : -1,
            Message = parts.Length > 1 ? parts[1] : ""
        };
    }
}