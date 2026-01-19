using Xray.Config.Models;
using Xray.Core.Models;
using Xray.Core.Wrappers;

namespace Xray.Core;

public class XrayLibCore : IXrayLibCore
{
    private readonly Guid _guid = Guid.NewGuid();

    private readonly NativeWrapper _lib;

    public XrayLibCore(XrayLibOptions options)
    {
        _lib = new NativeWrapper(options.LibPath);
    }

    public void Start(XrayConfig config)
    {
        _lib.Start(_guid, config.ToJson());
    }

    public void Stop()
    {
        _lib.Stop(_guid);
    }

    public string Version() => _lib.GetVersion();

    public bool IsStarted() => _lib.IsStarted(_guid);

    public X25519Data GenerateX25519Keys(string privateKey) => _lib.X25519Genkey(privateKey);

    public X25519Data GenerateX25519WGKeys(string privateKey) => _lib.X25519GenkeyWG(privateKey);

    public string GenerateUuidV4() => _lib.GenerateUuidV4();

    public string GenerateUuidV5(string input) => _lib.GenerateUuidV5(input);

    public MLDSA65Data GenerateMLDSA65(string input) => _lib.ExecuteMLDSA65(input);

    public MLKEM768Data GenerateMLKEM768(string input) => _lib.ExecuteMLKEM768(input);

    public VlessAuthentication GenerateVlessAuthentication() => _lib.ExecuteVlessAuthentication();

    public TlsCerts GenerateCert(GenerateCertOptions options) => _lib.GenerateCert(options);

    public string GetCertChainHash(string certPem) => _lib.ExecuteCertChainHash(certPem);

    public void Dispose()
    {
        _lib.Dispose();
    }
}

public class XrayLibOptions
{
    public required string LibPath { get; set; }
}