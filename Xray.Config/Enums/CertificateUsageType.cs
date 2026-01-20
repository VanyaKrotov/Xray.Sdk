using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Xray.Config.Utilities;

namespace Xray.Config.Enums;

[JsonConverter(typeof(EnumMemberConverter<CertificateUsageType>))]
public enum CertificateUsageType
{
    /// <summary>
    /// The certificate is used for authentication and TLS encryption.
    /// </summary>
    [EnumMember(Value = "encipherment")]
    Encipherment,

    /// <summary>
    /// The certificate used to verify the remote TLS certificate. When using this value, the current certificate must be a CA certificate.
    /// </summary>
    [EnumMember(Value = "verify")]
    Verify,

    /// <summary>
    /// The certificate is used to issue other certificates. When using this value, the current certificate must be a CA certificate.
    /// </summary>
    [EnumMember(Value = "issue")]
    Issue,
}