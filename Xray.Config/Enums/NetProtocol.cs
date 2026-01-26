using Xray.Config.Utilities;

namespace Xray.Config.Enums;

public enum NetProtocol
{
    [EnumProperty("http")]
    Http,

    [EnumProperty("tls")]
    Tls,

    [EnumProperty("bittorrent")]
    Bittorrent
}
