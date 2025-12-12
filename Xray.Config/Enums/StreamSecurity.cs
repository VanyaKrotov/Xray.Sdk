using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Xray.Config.Utilities;

namespace Xray.Config.Enums;

[JsonConverter(typeof(EnumMemberConverter<StreamSecurity>))]
public enum StreamSecurity
{
    [EnumMember(Value = "none")]
    None,

    [EnumMember(Value = "tls")]
    Tls,

    [EnumMember(Value = "reality")]
    Reality
}
