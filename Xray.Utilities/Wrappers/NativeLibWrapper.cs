using System.Runtime.InteropServices;

namespace Xray.Utilities.Wrappers;

public class NativeLibWrapper : IDisposable
{
    private IntPtr _libHandle;

    private delegate IntPtr Curve25519GenkeyDelegate(string privateKey);
    private delegate IntPtr Curve25519GenkeyWGDelegate(string privateKey);
    private delegate IntPtr ExecuteUUIDDelegate(string input);
    private delegate IntPtr ExecuteMLDSA65Delegate(string input);
    private delegate IntPtr ExecuteMLKEM768Delegate(string input);
    private delegate IntPtr ExecuteVlessEnc68Delegate();

    private Curve25519GenkeyDelegate _25519Genkey;
    private Curve25519GenkeyWGDelegate _25519GenkeyWG;
    private ExecuteUUIDDelegate _generateUUID;
    private ExecuteMLDSA65Delegate _executeMLDSA65;
    private ExecuteMLKEM768Delegate _executeMLKEM768;
    private ExecuteVlessEnc68Delegate _executeVlessEnc;

    private readonly Dictionary<OSPlatform, string> _platformExtensions = new()
    {
        { OSPlatform.OSX, ".dylib" },
        { OSPlatform.Linux, ".so" },
        { OSPlatform.Windows, ".dll" },
    };

    private readonly string LIB_NAME = "NativeLib";

    public NativeLibWrapper()
    {
        var platformExt = _platformExtensions.SingleOrDefault(x => RuntimeInformation.IsOSPlatform(x.Key)).Value;
        if (string.IsNullOrEmpty(platformExt))
        {
            throw new PlatformNotSupportedException();
        }

        _libHandle = NativeLibrary.Load(LIB_NAME + platformExt);

        _25519Genkey = Marshal.GetDelegateForFunctionPointer<Curve25519GenkeyDelegate>(NativeLibrary.GetExport(_libHandle, "Curve25519Genkey"));
        _25519GenkeyWG = Marshal.GetDelegateForFunctionPointer<Curve25519GenkeyWGDelegate>(NativeLibrary.GetExport(_libHandle, "Curve25519GenkeyWG"));
        _generateUUID = Marshal.GetDelegateForFunctionPointer<ExecuteUUIDDelegate>(NativeLibrary.GetExport(_libHandle, "ExecuteUUID"));
        _executeMLDSA65 = Marshal.GetDelegateForFunctionPointer<ExecuteMLDSA65Delegate>(NativeLibrary.GetExport(_libHandle, "ExecuteMLDSA65"));
        _executeMLKEM768 = Marshal.GetDelegateForFunctionPointer<ExecuteMLKEM768Delegate>(NativeLibrary.GetExport(_libHandle, "ExecuteMLKEM768"));
        _executeVlessEnc = Marshal.GetDelegateForFunctionPointer<ExecuteVlessEnc68Delegate>(NativeLibrary.GetExport(_libHandle, "ExecuteVLESSEnc"));
    }

    public X25519Data X25519Genkey(string privateKey)
    {
        var response = ParseTypedResponse(_25519Genkey(privateKey));

        return X25519Data.FromResponse(response.Message);
    }

    public X25519Data X25519GenkeyWG(string privateKey)
    {
        var response = ParseTypedResponse(_25519GenkeyWG(privateKey));

        return X25519Data.FromResponse(response.Message);
    }

    public string GenerateUuidV4()
    {
        var response = ParseTypedResponse(_generateUUID(""));

        return response.Message;
    }

    public string GenerateUuidV5(string input)
    {
        var response = ParseTypedResponse(_generateUUID(input));

        return response.Message;
    }

    public MLDSA65Data ExecuteMLDSA65(string input)
    {
        var response = ParseTypedResponse(_executeMLDSA65(input));
        var splitted = response.Message.Split("|");

        return new MLDSA65Data()
        {
            Seed = splitted[0],
            Pub = splitted[1]
        };
    }

    public MLKEM768Data ExecuteMLKEM768(string input)
    {
        var response = ParseTypedResponse(_executeMLKEM768(input));
        var splitted = response.Message.Split("|");

        return new MLKEM768Data()
        {
            Seed = splitted[0],
            Client = splitted[1],
            Hash = splitted[2]
        };
    }

    public VlessAuthentication ExecuteVlessAuthentication()
    {
        var response = ParseTypedResponse(_executeVlessEnc());
        var splitted = response.Message.Split("|");

        return new VlessAuthentication()
        {
            X25519 = new VlessAuthPair()
            {
                Decryption = splitted[0],
                Encryption = splitted[1]
            },
            MLKEM768 = new VlessAuthPair()
            {
                Decryption = splitted[2],
                Encryption = splitted[3]
            }
        };
    }

    public void Dispose()
    {
        if (_libHandle != IntPtr.Zero)
        {
            NativeLibrary.Free(_libHandle);
            _libHandle = IntPtr.Zero;
        }
    }

    private static TypedResponse ParseTypedResponse(IntPtr ptr)
    {
        string raw = Marshal.PtrToStringAnsi(ptr) ?? "";
        string[]? parts = raw.Split('|', 2);

        var response = new TypedResponse()
        {
            Code = int.TryParse(parts[0], out var c) ? c : -1,
            Message = parts.Length > 1 ? parts[1] : ""
        };

        if (!response.IsSuccess)
        {
            throw new Exception(response.Message);
        }

        return response;
    }
}

public class X25519Data
{
    public required string PrivateKey { get; set; }
    public required string Password { get; set; }
    public required string Hash { get; set; }

    public static X25519Data FromResponse(string message)
    {
        var splitted = message.Split("|");

        return new X25519Data
        {
            PrivateKey = splitted[0],
            Password = splitted[1],
            Hash = splitted[2]
        };
    }
}

public class MLDSA65Data
{
    public required string Seed { get; set; }

    public required string Pub { get; set; }
}

public class MLKEM768Data
{
    public required string Seed { get; set; }

    public required string Client { get; set; }

    public required string Hash { get; set; }
}

public class VlessAuthentication
{
    public required VlessAuthPair X25519 { get; set; }

    public required VlessAuthPair MLKEM768 { get; set; }
}

public class VlessAuthPair
{
    public required string Decryption { get; set; }

    public required string Encryption { get; set; }
}

public class TypedResponse
{
    public int Code { get; set; }

    public string Message { get; set; } = string.Empty;

    public bool IsSuccess => Code == 0;
}
