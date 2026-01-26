using Xray.Config.Utilities;

namespace Xray.Config.Enums;

public enum AllocateStrategy
{
    [EnumProperty("always")]
    Always,

    [EnumProperty("random")]
    Random
}