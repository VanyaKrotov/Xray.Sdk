using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Xray.Config.Utilities;

namespace Xray.Config.Enums;

[JsonConverter(typeof(EnumMemberConverter<RoutingDomainStrategy>))]
public enum RoutingDomainStrategy
{
    [EnumMember(Value = "AsIs")]
    AsIs,

    [EnumMember(Value = "IPIfNonMatch")]
    IPIfNonMatch,

    [EnumMember(Value = "IPOnDemand")]
    IPOnDemand
}

[JsonConverter(typeof(EnumMemberConverter<DomainStrategy>))]
public enum DomainStrategy
{
    [EnumMember(Value = "AsIs")]
    AsIs,

    [EnumMember(Value = "UseIP")]
    UseIP,

    [EnumMember(Value = "UseIPv6v4")]
    UseIPv6v4,

    [EnumMember(Value = "UseIPv6")]
    UseIPv6,

    [EnumMember(Value = "UseIPv4v6")]
    UseIPv4v6,

    [EnumMember(Value = "UseIPv4")]
    UseIPv4,

    [EnumMember(Value = "ForceIP")]
    ForceIP,

    [EnumMember(Value = "ForceIPv6v4")]
    ForceIPv6v4,

    [EnumMember(Value = "ForceIPv6")]
    ForceIPv6,

    [EnumMember(Value = "ForceIPv4v6")]
    ForceIPv4v6,

    [EnumMember(Value = "ForceIPv4")]
    ForceIPv4,
}
