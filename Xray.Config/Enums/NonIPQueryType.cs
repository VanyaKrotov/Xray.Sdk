using Xray.Config.Utilities;

namespace Xray.Config.Enums;

/// <summary>
/// Controls requests that are not for IP addresses (not A or AAAA).
/// </summary>
public enum NonIPQueryType
{
    /// <summary>
    /// Drop
    /// </summary>
    [EnumProperty("drop")]
    Drop,

    /// <summary>
    /// Not processed by the built-in DNS server, but forwarded to the target server. Unlike [Reject "drop"]
    /// </summary>
    [EnumProperty("k")]
    Skip,

    /// <summary>
    /// 
    /// </summary>
    [EnumProperty("reject")]
    Reject
}