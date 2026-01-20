using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Xray.Config.Utilities;

namespace Xray.Config.Enums;

/// <summary>
/// Domain name resolution strategy.
/// </summary>
[JsonConverter(typeof(EnumMemberConverter<RoutingDomainStrategy>))]
public enum RoutingDomainStrategy
{
    /// <summary>
    /// Only domain names are used for route selection.
    /// </summary>
    [EnumMember(Value = "AsIs")]
    AsIs,

    /// <summary>
    /// If the domain name does not match any rule, the domain name is resolved to an IP address (A record or AAAA record) for re-mapping;
    /// <list type="bullet">
    /// <item>If a domain name has multiple A records, an attempt is made to match all A records until one matches some rule;</item>
    /// <item>The resolved IP address is only used when choosing a route, and the original domain name is still used in the forwarded data packets;</item>
    /// </list>
    /// </summary>
    [EnumMember(Value = "IPIfNonMatch")]
    IPIfNonMatch,

    /// <summary>
    /// If any IP address-based rule is encountered during matching, the domain name is immediately resolved to an IP address for matching;
    /// </summary>
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
