
using Xray.Utilities.Wrappers;

namespace Xray.Utilities.Crypto;

public class XrayCrypto : IXrayCrypto
{
    private readonly NativeLibWrapper _lib = new NativeLibWrapper();

    public X25519Data X25519Genkey(string privateKey) => _lib.X25519Genkey(privateKey);

    public X25519Data X25519GenkeyWG(string privateKey) => _lib.X25519GenkeyWG(privateKey);

    public string GenerateUuidV4() => _lib.GenerateUuidV4();

    public string GenerateUuidV5(string input) => _lib.GenerateUuidV5(input);

    public MLDSA65Data GenMLDSA65(string input) => _lib.ExecuteMLDSA65(input);

    public MLKEM768Data GenMLKEM768(string input) => _lib.ExecuteMLKEM768(input);

    public VlessAuthentication GenerateVlessAuthentication() => _lib.ExecuteVlessAuthentication();
}