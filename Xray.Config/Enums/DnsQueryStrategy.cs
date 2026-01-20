using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Xray.Config.Utilities;

namespace Xray.Config.Enums;

[JsonConverter(typeof(EnumMemberConverter<DnsQueryStrategy>))]
public enum DnsQueryStrategy
{
    /// <summary>
    /// Queries both A and AAAA records.
    /// </summary>
    [EnumMember(Value = "UseIP")]
    UseIP,

    /// <summary>
    /// Only queries A records.
    /// </summary>
    [EnumMember(Value = "UseIPv4")]
    UseIPv4,

    /// <summary>
    /// Only queries AAAA records.
    /// </summary>
    [EnumMember(Value = "UseIPv6")]
    UseIPv6,

    /// <summary>
    /// Every time dns-Query call, it check the system-network to see if it supports IPv6(and IPv4) or not, if it support IPv6(or IPv4), the IPv6(or IPv4) is also returned, otherwise not returned.
    /// </summary>
    [EnumMember(Value = "UseSystem")]
    UseSystem
}