using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Xray.Config.Utilities;

namespace Xray.Config.Enums;

[JsonConverter(typeof(EnumMemberConverter<AddressPortStrategy>))]
public enum AddressPortStrategy
{
    [EnumMember(Value = "none")]
    None,

    [EnumMember(Value = "SrvPortOnly")]
    SrvPortOnly,

    [EnumMember(Value = "SrvAddressOnly")]
    SrvAddressOnly,

    [EnumMember(Value = "SrvPortAndAddress")]
    SrvPortAndAddress,

    [EnumMember(Value = "TxtPortOnly")]
    TxtPortOnly,

    [EnumMember(Value = "TxtAddressOnly")]
    TxtAddressOnly,

    [EnumMember(Value = "TxtPortAndAddress")]
    TxtPortAndAddress,
}