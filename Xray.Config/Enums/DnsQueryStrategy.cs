using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Xray.Config.Utilities;

namespace Xray.Config.Enums;

[JsonConverter(typeof(EnumMemberConverter<DnsQueryStrategy>))]
public enum DnsQueryStrategy
{
    [EnumMember(Value = "UseIP")]
    UseIP,

    [EnumMember(Value = "UseIPv4")]
    UseIPv4,

    [EnumMember(Value = "UseIPv6")]
    UseIPv6,

    [EnumMember(Value = "UseSystem")]
    UseSystem
}