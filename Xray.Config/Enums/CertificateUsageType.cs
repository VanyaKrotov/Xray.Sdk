using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Xray.Config.Utilities;

namespace Xray.Config.Enums;

[JsonConverter(typeof(EnumMemberConverter<CertificateUsageType>))]
public enum CertificateUsageType
{
    [EnumMember(Value = "encipherment")]
    Encipherment,

    [EnumMember(Value = "verify")]
    Verify,

    [EnumMember(Value = "issue")]
    Issue,
}