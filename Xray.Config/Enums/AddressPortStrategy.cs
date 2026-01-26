using Xray.Config.Utilities;

namespace Xray.Config.Enums;

public enum AddressPortStrategy
{
    [EnumProperty("none")]
    None,

    [EnumProperty("SrvPortOnly")]
    SrvPortOnly,

    [EnumProperty("SrvAddressOnly")]
    SrvAddressOnly,

    [EnumProperty("SrvPortAndAddress")]
    SrvPortAndAddress,

    [EnumProperty("TxtPortOnly")]
    TxtPortOnly,

    [EnumProperty("TxtAddressOnly")]
    TxtAddressOnly,

    [EnumProperty("TxtPortAndAddress")]
    TxtPortAndAddress,
}