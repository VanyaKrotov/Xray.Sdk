using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Xray.Config.Utilities;

namespace Xray.Config.Enums;

/// <summary>
/// Camouflage type
/// </summary>
[JsonConverter(typeof(EnumMemberConverter<KcpHeaderType>))]
public enum KcpHeaderType
{
    /// <summary>
    /// Default value, no masking is applied, the data sent does not have any distinguishing features.
    /// </summary>
    [EnumMember(Value = "none")]
    None,

    /// <summary>
    /// Disguised as SRTP packets, will be identified as video call data (e.g. FaceTime).
    /// </summary>
    [EnumMember(Value = "srtp")]
    Srtp,

    /// <summary>
    /// Disguised as UTP packets, will be identified as BT download data.
    /// </summary>
    [EnumMember(Value = "utp")]
    Utp,

    /// <summary>
    /// Disguised as WeChat video call packets.
    /// </summary>
    [EnumMember(Value = "wechat-video")]
    WechatVideo,

    /// <summary>
    /// Disguised as DTLS 1.2 packets.
    /// </summary>
    [EnumMember(Value = "dtls")]
    Dtls,

    /// <summary>
    /// Disguised as WireGuard packets. (This is not the real WireGuard protocol.)
    /// </summary>
    [EnumMember(Value = "wireguard")]
    Wireguard,

    /// <summary>
    /// Some corporate networks allow DNS queries without authorization, adding a DNS header to KCP packets allows bypassing some corporate networks.
    /// </summary>
    [EnumMember(Value = "dns")]
    Dns,
}