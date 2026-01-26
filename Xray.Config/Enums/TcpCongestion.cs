using Xray.Config.Utilities;

namespace Xray.Config.Enums;

public enum TcpCongestion
{
    [EnumProperty("bbr")]
    Bbr,

    [EnumProperty("cubic")]
    Cubic,

    [EnumProperty("reno")]
    Reno,
}