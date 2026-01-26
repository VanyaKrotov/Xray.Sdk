using Xray.Config.Utilities;

namespace Xray.Config.Enums;

public enum OperationSystem
{
    [EnumProperty("linux")]
    Linux,

    [EnumProperty("windows")]
    Windows,

    [EnumProperty("darwin")]
    Darwin,
}