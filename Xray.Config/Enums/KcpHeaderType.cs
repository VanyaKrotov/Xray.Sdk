using Xray.Config.Utilities;

namespace Xray.Config.Enums;

/// <summary>
/// Camouflage type
/// </summary>
public enum KcpHeaderType
{
    /// <summary>
    /// Default value, no masking is applied, the data sent does not have any distinguishing features.
    /// </summary>
    [EnumProperty("none")]
    None,

    /// <summary>
    /// Disguised as SRTP packets, will be identified as video call data (e.g. FaceTime).
    /// </summary>
    [EnumProperty("srtp")]
    Srtp,

    /// <summary>
    /// Disguised as UTP packets, will be identified as BT download data.
    /// </summary>
    [EnumProperty("utp")]
    Utp,

    /// <summary>
    /// Disguised as WeChat video call packets.
    /// </summary>
    [EnumProperty("wechat-video")]
    WechatVideo,

    /// <summary>
    /// Disguised as DTLS 1.2 packets.
    /// </summary>
    [EnumProperty("dtls")]
    Dtls,

    /// <summary>
    /// Disguised as WireGuard packets. (This is not the real WireGuard protocol.)
    /// </summary>
    [EnumProperty("wireguard")]
    Wireguard,

    /// <summary>
    /// Some corporate networks allow DNS queries without authorization, adding a DNS header to KCP packets allows bypassing some corporate networks.
    /// </summary>
    [EnumProperty("dns")]
    Dns,
}