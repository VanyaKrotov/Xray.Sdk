using Xray.Config.Utilities;

namespace Xray.Config.Enums;

/// <summary>
/// Whether to enable transparent proxying (Linux only).
/// </summary>
public enum TProxy
{
    /// <summary>
    /// Use transparent proxying in forwarding mode. All TCP connections over IPv4/6 are supported.
    /// </summary>
    [EnumProperty("redirect")]
    Redirect,

    /// <summary>
    /// Use transparent proxying in TProxy mode. All TCP and UDP connections based on IPv4/6 are supported.
    /// </summary>
    [EnumProperty("tproxy")]
    On,

    /// <summary>
    /// Disable transparent proxying.
    /// </summary>
    [EnumProperty("off")]
    Off,
}