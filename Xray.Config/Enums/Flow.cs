using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Xray.Config.Utilities;

namespace Xray.Config.Enums;

[JsonConverter(typeof(EnumMemberConverter<XtlsFlow>))]
public enum XtlsFlow
{   
    /// <summary>
    /// Default empty value
    /// </summary>
    [EnumMember(Value = "")]
    None,

    /// <summary>
    /// Uses XTLS, enables random padding of the internal handshake. Intercepts UDP traffic on port 443 (QUIC), forcing browsers to use regular HTTPS to increase the volume of traffic Splice can apply to.
    /// </summary>
    [EnumMember(Value = "xtls-rprx-vision")]
    XtlsRprxVision,

    /// <summary>
    /// Similar to xtls-rprx-vision, but does not intercept UDP 443. Used in cases where programs are forced to use QUIC, and interception causes them to stop working.
    /// </summary>
    [EnumMember(Value = "xtls-rprx-vision-udp443")]
    XtlsRprxVisionUdp443,
}