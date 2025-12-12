using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Xray.Config.Utilities;

namespace Xray.Config.Enums;

[JsonConverter(typeof(EnumMemberConverter<RawHeadersType>))]
public enum RawHeadersType
{
    [EnumMember(Value = "none")]
    None,

    [EnumMember(Value = "http")]
    Http,
}