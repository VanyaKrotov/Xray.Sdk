using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Xray.Config.Utilities;

namespace Xray.Config.Enums;

/// <summary>
/// Controls requests that are not for IP addresses (not A or AAAA).
/// </summary>
[JsonConverter(typeof(EnumMemberConverter<NonIPQueryType>))]
public enum NonIPQueryType
{
    /// <summary>
    /// Drop
    /// </summary>
    [EnumMember(Value = "drop")]
    Drop,

    /// <summary>
    /// Not processed by the built-in DNS server, but forwarded to the target server. Unlike [Reject "drop"]
    /// </summary>
    [EnumMember(Value = "k")]
    Skip,

    /// <summary>
    /// 
    /// </summary>
    [EnumMember(Value = "reject")]
    Reject
}