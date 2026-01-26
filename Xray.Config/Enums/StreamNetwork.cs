using Xray.Config.Utilities;

namespace Xray.Config.Enums;

public enum StreamNetwork
{
    [EnumProperty("raw")]
    [EnumProperty("tcp")]
    Raw,

    [EnumProperty("xhttp")]
    XHttp,

    [EnumProperty("kcp")]
    Kcp,

    [EnumProperty("grpc")]
    Grpc,

    [EnumProperty("ws")]
    Ws,

    [EnumProperty("httpupgrade")]
    HttpUpgrade,

    [EnumProperty("hysteria")]
    Hysteria
}