
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Xray.Config.Utilities;

namespace Xray.Config.Enums;

[JsonConverter(typeof(EnumMemberConverter<TrafficType>))]
public enum TrafficType
{
    [EnumMember(Value = "http")]
    Http,

    [EnumMember(Value = "tls")]
    Tls,

    [EnumMember(Value = "quic")]
    Quic,

    [EnumMember(Value = "fakedns")]
    Fakedns
}