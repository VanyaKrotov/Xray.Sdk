using Xray.Core.Models;

namespace Xray.Core;

public interface IXrayLibCore : IXrayCore
{
    /// <summary>
    /// Generate key pair for X25519 key exchange (REALITY, VLESS Encryption)
    /// </summary>
    /// <param name="privateKey">Private key (base64.RawURLEncoding)</param>
    /// <returns>X25519 keys</returns>
    public X25519Data GenerateX25519Keys(string privateKey);

    /// <summary>
    /// Generate key pair for X25519 key exchange for Wireguard
    /// </summary>
    /// <param name="privateKey">Private key (base64.RawURLEncoding)</param>
    /// <returns>X25519 keys</returns>
    public X25519Data GenerateX25519WGKeys(string privateKey);

    /// <summary>
    /// Generate UUIDv4 (VLESS)
    /// </summary>
    /// <returns>UUIDv4 (random)</returns>
    public string GenerateUuidV4();

    /// <summary>
    /// Generate UUIDv5 (VLESS)
    /// </summary>
    /// <param name="input">Initial word (30 bytes)</param>
    /// <returns>UUIDv5 (random)</returns>
    public string GenerateUuidV5(string input);

    /// <summary>
    /// Generate key pair for ML-DSA-65 post-quantum signature (REALITY)
    /// </summary>
    /// <param name="input">Seed in base64 encoding</param>
    /// <returns>MLDSA65 data</returns>
    public MLDSA65Data GenerateMLDSA65(string input);

    /// <summary>
    /// Generate key pair for ML-KEM-768 post-quantum key exchange (VLESS Encryption)
    /// </summary>
    /// <param name="input">Seed in base64 encoding</param>
    /// <returns>MLKEM768 keys</returns>
    public MLKEM768Data GenerateMLKEM768(string input);

    /// <summary>
    /// Generate decryption/encryption json pair (VLESS Encryption)
    /// </summary>
    /// <returns>X25519 not Post-Quantum and ML-KEM-768 Post-Quantum</returns>
    public VlessAuthentication GenerateVlessAuthentication();

    /// <summary>
    /// Generating TLS certificates
    /// </summary>
    /// <param name="options">Options for generation</param>
    /// <returns>Generated TLS certs</returns>
    public TlsCerts GenerateCert(GenerateCertOptions options);

    /// <summary>
    /// Calculate TLS certificates hash
    /// </summary>
    /// <param name="certPem">file or cert content</param>
    /// <returns>Hash for input TLS cert</returns>
    public string GetCertChainHash(string certPem);
}