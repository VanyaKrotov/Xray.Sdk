using Xray.Config.Utilities;

namespace Xray.Config.Enums;

/// <summary>
/// Whether transport layer encryption type.
/// </summary>
public enum StreamSecurity
{
    /// <summary>
    /// Means no encryption.
    /// </summary>
    [EnumProperty("none")]
    None,

    /// <summary>
    /// Means using <see href="https://en.wikipedia.org/wiki/Transport_Layer_Security">TLS</see>
    /// </summary>
    [EnumProperty("tls")]
    Tls,

    /// <summary>
    /// Means using REALITY.
    /// </summary>
    [EnumProperty("reality")]
    Reality
}
