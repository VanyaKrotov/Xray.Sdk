using Xray.Config.Utilities;

namespace Xray.Config.Enums;

public enum XtlsFlow
{
    /// <summary>
    /// Default empty value
    /// </summary>
    [EnumProperty("")]
    None,

    /// <summary>
    /// Uses XTLS, enables random padding of the internal handshake. Intercepts UDP traffic on port 443 (QUIC), forcing browsers to use regular HTTPS to increase the volume of traffic Splice can apply to.
    /// </summary>
    [EnumProperty("xtls-rprx-vision")]
    XtlsRprxVision,

    /// <summary>
    /// Similar to xtls-rprx-vision, but does not intercept UDP 443. Used in cases where programs are forced to use QUIC, and interception causes them to stop working.
    /// </summary>
    [EnumProperty("xtls-rprx-vision-udp443")]
    XtlsRprxVisionUdp443,
}