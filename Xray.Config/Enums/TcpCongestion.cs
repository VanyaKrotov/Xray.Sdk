using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Xray.Config.Utilities;

namespace Xray.Config.Enums;

[JsonConverter(typeof(EnumMemberConverter<TcpCongestion>))]
public enum TcpCongestion
{
    [EnumMember(Value = "bbr")]
    Bbr,

    [EnumMember(Value = "cubic")]
    Cubic,

    [EnumMember(Value = "reno")]
    Reno,
}