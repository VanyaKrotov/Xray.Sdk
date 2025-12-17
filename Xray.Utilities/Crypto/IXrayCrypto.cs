using Xray.Utilities.Wrappers;

namespace Xray.Utilities.Crypto;

public interface IXrayCrypto
{
    X25519Data X25519Genkey(string privateKey);

    X25519Data X25519GenkeyWG(string privateKey);

    string GenerateUuidV4();

    string GenerateUuidV5(string input);

    MLDSA65Data GenMLDSA65(string input);

    MLKEM768Data GenMLKEM768(string input);

    VlessAuthentication GenerateVlessAuthentication();
}