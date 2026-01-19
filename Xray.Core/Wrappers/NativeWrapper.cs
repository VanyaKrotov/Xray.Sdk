using System.Runtime.InteropServices;
using Xray.Core.Exceptions;
using Xray.Core.Models;

namespace Xray.Core.Wrappers;

public class NativeWrapper : IDisposable
{
    private IntPtr _libHandle;

    private delegate IntPtr StartDelegate(string uuid, string jsonConfig);
    private delegate IntPtr StopDelegate(string uuid);
    private delegate int IsStartedDelegate(string uuid);
    private delegate IntPtr GetVersionDelegate();
    private delegate IntPtr Curve25519GenkeyDelegate(string privateKey);
    private delegate IntPtr Curve25519GenkeyWGDelegate(string privateKey);
    private delegate IntPtr ExecuteUUIDDelegate(string input);
    private delegate IntPtr ExecuteMLDSA65Delegate(string input);
    private delegate IntPtr ExecuteMLKEM768Delegate(string input);
    private delegate IntPtr ExecuteVlessEnc68Delegate();
    private delegate IntPtr GenerateCertDelegate(string domains, string commonName, string organization, bool isCA, string expire);
    private delegate IntPtr ExecuteCertChainHashDelegate(string certPem);

    private StartDelegate _start;
    private StopDelegate _stop;
    private IsStartedDelegate _isStarted;
    private GetVersionDelegate _getVersion;
    private Curve25519GenkeyDelegate _25519Genkey;
    private Curve25519GenkeyWGDelegate _25519GenkeyWG;
    private ExecuteUUIDDelegate _generateUUID;
    private ExecuteMLDSA65Delegate _executeMLDSA65;
    private ExecuteMLKEM768Delegate _executeMLKEM768;
    private ExecuteVlessEnc68Delegate _executeVlessEnc;
    private GenerateCertDelegate _generateCert;
    private ExecuteCertChainHashDelegate _executeCertChainHash;

    public NativeWrapper(string libPath)
    {
        _libHandle = NativeLibrary.Load(libPath);

        // Core
        _start = Marshal.GetDelegateForFunctionPointer<StartDelegate>(NativeLibrary.GetExport(_libHandle, "Start"));
        _stop = Marshal.GetDelegateForFunctionPointer<StopDelegate>(NativeLibrary.GetExport(_libHandle, "Stop"));
        _isStarted = Marshal.GetDelegateForFunctionPointer<IsStartedDelegate>(NativeLibrary.GetExport(_libHandle, "IsStarted"));
        _getVersion = Marshal.GetDelegateForFunctionPointer<GetVersionDelegate>(NativeLibrary.GetExport(_libHandle, "GetXrayCoreVersion"));

        // Crypto utilities
        _25519Genkey = Marshal.GetDelegateForFunctionPointer<Curve25519GenkeyDelegate>(NativeLibrary.GetExport(_libHandle, "Curve25519Genkey"));
        _25519GenkeyWG = Marshal.GetDelegateForFunctionPointer<Curve25519GenkeyWGDelegate>(NativeLibrary.GetExport(_libHandle, "Curve25519GenkeyWG"));
        _generateUUID = Marshal.GetDelegateForFunctionPointer<ExecuteUUIDDelegate>(NativeLibrary.GetExport(_libHandle, "ExecuteUUID"));
        _executeMLDSA65 = Marshal.GetDelegateForFunctionPointer<ExecuteMLDSA65Delegate>(NativeLibrary.GetExport(_libHandle, "ExecuteMLDSA65"));
        _executeMLKEM768 = Marshal.GetDelegateForFunctionPointer<ExecuteMLKEM768Delegate>(NativeLibrary.GetExport(_libHandle, "ExecuteMLKEM768"));
        _executeVlessEnc = Marshal.GetDelegateForFunctionPointer<ExecuteVlessEnc68Delegate>(NativeLibrary.GetExport(_libHandle, "ExecuteVLESSEnc"));
        _generateCert = Marshal.GetDelegateForFunctionPointer<GenerateCertDelegate>(NativeLibrary.GetExport(_libHandle, "GenerateCert"));
        _executeCertChainHash = Marshal.GetDelegateForFunctionPointer<ExecuteCertChainHashDelegate>(NativeLibrary.GetExport(_libHandle, "ExecuteCertChainHash"));
    }

    /// <summary>
    /// Start the xray-core server by uuid
    /// </summary>
    /// <param name="guid">The uniq identity for server</param>
    public void Start(Guid guid, string jsonConfig)
    {
        GetTransferResponseData(_start(guid.ToString(), jsonConfig));
    }

    /// <summary>
    /// Stop the xray-core server by uuid
    /// </summary>
    /// <param name="guid">The uniq identity for server</param>
    public void Stop(Guid guid)
    {
        GetTransferResponseData(_stop(guid.ToString()));
    }

    /// <summary>
    /// Check if the xray-core server instance is running by uuid
    /// </summary>
    /// <param name="guid">The uniq identity for server</param>
    /// <returns>True if the server is running</returns>
    public bool IsStarted(Guid guid)
    {
        return _isStarted(guid.ToString()) == 1;
    }

    /// <summary>
    /// Get the xray core version
    /// </summary>
    /// <returns>Xray core version</returns>
    public string GetVersion()
    {
        return Marshal.PtrToStringAnsi(_getVersion())!;
    }

    /// <summary>
    /// Generate key pair for X25519 key exchange (REALITY, VLESS Encryption)
    /// </summary>
    /// <param name="privateKey">Private key (base64.RawURLEncoding)</param>
    /// <returns>X25519 keys</returns>
    public X25519Data X25519Genkey(string privateKey)
    {
        var splitted = GetTransferResponseData(_25519Genkey(privateKey)).Split("|");

        return new X25519Data
        {
            PrivateKey = splitted[0],
            Password = splitted[1],
            Hash = splitted[2]
        };
    }

    /// <summary>
    /// Generate key pair for X25519 key exchange for Wireguard
    /// </summary>
    /// <param name="privateKey">Private key (base64.RawURLEncoding)</param>
    /// <returns>X25519 keys</returns>
    public X25519Data X25519GenkeyWG(string privateKey)
    {
        var splitted = GetTransferResponseData(_25519GenkeyWG(privateKey)).Split("|");

        return new X25519Data
        {
            PrivateKey = splitted[0],
            Password = splitted[1],
            Hash = splitted[2]
        };
    }

    /// <summary>
    /// Generate UUIDv4 (VLESS)
    /// </summary>
    /// <returns>UUIDv4 (random)</returns>
    public string GenerateUuidV4()
    {
        return GetTransferResponseData(_generateUUID(""));
    }

    /// <summary>
    /// Generate UUIDv5 (VLESS)
    /// </summary>
    /// <param name="input">Initial word (30 bytes)</param>
    /// <returns>UUIDv5 (random)</returns>
    public string GenerateUuidV5(string input)
    {
        return GetTransferResponseData(_generateUUID(input));
    }

    /// <summary>
    /// Generate key pair for ML-DSA-65 post-quantum signature (REALITY)
    /// </summary>
    /// <param name="input">Seed in base64 encoding</param>
    /// <returns>MLDSA65 keys</returns>
    public MLDSA65Data ExecuteMLDSA65(string input)
    {
        var splitted = GetTransferResponseData(_executeMLDSA65(input)).Split("|");

        return new MLDSA65Data()
        {
            Seed = splitted[0],
            Pub = splitted[1]
        };
    }

    /// <summary>
    /// Generate key pair for ML-KEM-768 post-quantum key exchange (VLESS Encryption)
    /// </summary>
    /// <param name="input">Seed in base64 encoding</param>
    /// <returns>MLKEM768 keys</returns>
    public MLKEM768Data ExecuteMLKEM768(string input)
    {
        var splitted = GetTransferResponseData(_executeMLKEM768(input)).Split("|");

        return new MLKEM768Data()
        {
            Seed = splitted[0],
            Client = splitted[1],
            Hash = splitted[2]
        };
    }

    /// <summary>
    /// Generate decryption/encryption json pair (VLESS Encryption)
    /// </summary>
    /// <returns>X25519 not Post-Quantum and ML-KEM-768 Post-Quantum</returns>
    public VlessAuthentication ExecuteVlessAuthentication()
    {
        var splitted = GetTransferResponseData(_executeVlessEnc()).Split("|");

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

    /// <summary>
    /// Generating TLS certificates
    /// </summary>
    /// <param name="options">Options for generation</param>
    /// <returns>Generated TLS certs</returns>
    public TlsCerts GenerateCert(GenerateCertOptions options)
    {
        var data = GetTransferResponseData(_generateCert(string.Join(",", options.Domains), options.CommonName, options.Organization, options.IsCA, options.Expire)).Split("|");

        return new TlsCerts()
        {
            Certificate = data[0].Split(",").ToList(),
            Key = data[1].Split(",").ToList()
        };
    }

    /// <summary>
    /// Calculate TLS certificates hash
    /// </summary>
    /// <param name="certPem">file or cert content</param>
    /// <returns>Hash for input TLS cert</returns>
    public string ExecuteCertChainHash(string certPem)
    {
        return GetTransferResponseData(_executeCertChainHash(certPem));
    }

    /// <summary>
    /// Dispose lib instance from memory
    /// </summary>
    public void Dispose()
    {
        if (_libHandle != IntPtr.Zero)
        {
            NativeLibrary.Free(_libHandle);
            _libHandle = IntPtr.Zero;
        }
    }

    private static string HandleResponse(TransferResponse response)
    {
        if (!response.IsSuccess)
        {
            throw new CoreException(response.Message, response.Code);
        }

        return response.Message;
    }

    private static TransferResponse ParseTransferResponse(IntPtr ptr)
    {
        string raw = Marshal.PtrToStringAnsi(ptr) ?? "";
        string[]? parts = raw.Split('|', 2);

        return new TransferResponse()
        {
            Code = int.TryParse(parts[0], out var c) ? c : -1,
            Message = parts.Length > 1 ? parts[1] : ""
        };
    }

    private static string GetTransferResponseData(IntPtr ptr)
    {
        var response = ParseTransferResponse(ptr);

        HandleResponse(response);

        return response.Message;
    }
}

class TransferResponse
{
    public int Code { get; set; }

    public string Message { get; set; } = string.Empty;

    public bool IsSuccess => Code == 0;
}