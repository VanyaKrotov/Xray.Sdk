using Xray.Config.Utilities;

namespace Xray.Config.Enums;

public enum CertificateUsageType
{
    /// <summary>
    /// The certificate is used for authentication and TLS encryption.
    /// </summary>
    [EnumProperty("encipherment")]
    Encipherment,

    /// <summary>
    /// The certificate used to verify the remote TLS certificate. When using this value, the current certificate must be a CA certificate.
    /// </summary>
    [EnumProperty("verify")]
    Verify,

    /// <summary>
    /// The certificate is used to issue other certificates. When using this value, the current certificate must be a CA certificate.
    /// </summary>
    [EnumProperty("issue")]
    Issue,
}