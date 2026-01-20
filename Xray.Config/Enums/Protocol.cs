using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Xray.Config.Utilities;

namespace Xray.Config.Enums;

[JsonConverter(typeof(EnumMemberConverter<InboundProtocol>))]
public enum InboundProtocol
{
    [EnumMember(Value = "dokodemo-door")]
    DokodemoDoor,

    [EnumMember(Value = "http")]
    Http,

    [EnumMember(Value = "shadowsocks")]
    ShadowSocks,

    [EnumMember(Value = "socks")]
    Socks,

    [EnumMember(Value = "vless")]
    Vless,

    [EnumMember(Value = "vmess")]
    VMess,

    [EnumMember(Value = "trojan")]
    Trojan,

    [EnumMember(Value = "wireguard")]
    Wireguard
}

[JsonConverter(typeof(EnumMemberConverter<OutboundProtocol>))]
public enum OutboundProtocol
{
    [EnumMember(Value = "blackhole")]
    BlackHole,

    [EnumMember(Value = "dns")]
    Dns,

    [EnumMember(Value = "freedom")]
    Freedom,

    [EnumMember(Value = "loopback")]
    Loopback,

    [EnumMember(Value = "http")]
    Http,

    [EnumMember(Value = "shadowsocks")]
    ShadowSocks,

    [EnumMember(Value = "socks")]
    Socks,

    [EnumMember(Value = "vless")]
    Vless,

    [EnumMember(Value = "vmess")]
    VMess,

    [EnumMember(Value = "trojan")]
    Trojan,

    [EnumMember(Value = "wireguard")]
    Wireguard,
    
    [EnumMember(Value = "hysteria")]
    Hysteria
}