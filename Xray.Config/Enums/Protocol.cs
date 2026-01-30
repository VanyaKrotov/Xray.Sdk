using Xray.Config.Utilities;

namespace Xray.Config.Enums;

public enum InboundProtocol
{
    [EnumProperty("dokodemo-door")]
    DokodemoDoor,

    [EnumProperty("http")]
    Http,

    [EnumProperty("shadowsocks")]
    ShadowSocks,

    [EnumProperty("socks")]
    Socks,

    [EnumProperty("vless")]
    Vless,

    [EnumProperty("vmess")]
    VMess,

    [EnumProperty("trojan")]
    Trojan,

    [EnumProperty("wireguard")]
    Wireguard,
   
    [EnumProperty("tun")]
    Tun
}

public enum OutboundProtocol
{
    [EnumProperty("blackhole")]
    BlackHole,

    [EnumProperty("dns")]
    Dns,

    [EnumProperty("freedom")]
    Freedom,

    [EnumProperty("loopback")]
    Loopback,

    [EnumProperty("http")]
    Http,

    [EnumProperty("shadowsocks")]
    ShadowSocks,

    [EnumProperty("socks")]
    Socks,

    [EnumProperty("vless")]
    Vless,

    [EnumProperty("vmess")]
    VMess,

    [EnumProperty("trojan")]
    Trojan,

    [EnumProperty("wireguard")]
    Wireguard,

    [EnumProperty("hysteria")]
    Hysteria
}