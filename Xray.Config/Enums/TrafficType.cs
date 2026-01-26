
using Xray.Config.Utilities;

namespace Xray.Config.Enums;

public enum TrafficType
{
    [EnumProperty("http")]
    Http,

    [EnumProperty("tls")]
    Tls,

    [EnumProperty("quic")]
    Quic,

    [EnumProperty("fakedns")]
    Fakedns
}