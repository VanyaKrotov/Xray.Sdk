using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Xray.Config.Utilities;

namespace Xray.Config.Enums;

/// <summary>
/// Whether to enable transparent proxying (Linux only).
/// </summary>
[JsonConverter(typeof(EnumMemberConverter<TProxy>))]
public enum TProxy
{
    /// <summary>
    /// Use transparent proxying in forwarding mode. All TCP connections over IPv4/6 are supported.
    /// </summary>
    [EnumMember(Value = "redirect")]
    Redirect,

    /// <summary>
    /// Use transparent proxying in TProxy mode. All TCP and UDP connections based on IPv4/6 are supported.
    /// </summary>
    [EnumMember(Value = "tproxy")]
    On,

    /// <summary>
    /// Disable transparent proxying.
    /// </summary>
    [EnumMember(Value = "off")]
    Off,
}