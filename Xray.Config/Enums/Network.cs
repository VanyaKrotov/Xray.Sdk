using Xray.Config.Utilities;

namespace Xray.Config.Enums;

public enum Network
{
    [EnumProperty("tcp")]
    Tcp,

    [EnumProperty("udp")]
    Udp
}