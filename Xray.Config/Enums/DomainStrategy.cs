using Xray.Config.Utilities;

namespace Xray.Config.Enums;

/// <summary>
/// Domain name resolution strategy.
/// </summary>
public enum RoutingDomainStrategy
{
    /// <summary>
    /// Only domain names are used for route selection.
    /// </summary>
    [EnumProperty("AsIs")]
    AsIs,

    /// <summary>
    /// If the domain name does not match any rule, the domain name is resolved to an IP address (A record or AAAA record) for re-mapping;
    /// <list type="bullet">
    /// <item>If a domain name has multiple A records, an attempt is made to match all A records until one matches some rule;</item>
    /// <item>The resolved IP address is only used when choosing a route, and the original domain name is still used in the forwarded data packets;</item>
    /// </list>
    /// </summary>
    [EnumProperty("IPIfNonMatch")]
    IPIfNonMatch,

    /// <summary>
    /// If any IP address-based rule is encountered during matching, the domain name is immediately resolved to an IP address for matching;
    /// </summary>
    [EnumProperty("IPOnDemand")]
    IPOnDemand
}

public enum DomainStrategy
{
    [EnumProperty("AsIs")]
    AsIs,

    [EnumProperty("UseIP")]
    UseIP,

    [EnumProperty("UseIPv6v4")]
    UseIPv6v4,

    [EnumProperty("UseIPv6")]
    UseIPv6,

    [EnumProperty("UseIPv4v6")]
    UseIPv4v6,

    [EnumProperty("UseIPv4")]
    UseIPv4,

    [EnumProperty("ForceIP")]
    ForceIP,

    [EnumProperty("ForceIPv6v4")]
    ForceIPv6v4,

    [EnumProperty("ForceIPv6")]
    ForceIPv6,

    [EnumProperty("ForceIPv4v6")]
    ForceIPv4v6,

    [EnumProperty("ForceIPv4")]
    ForceIPv4,
}
