using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Xray.Config.Utilities;

namespace Xray.Config.Enums;

[JsonConverter(typeof(EnumMemberConverter<SocksAuth>))]
public enum SocksAuth
{
    [EnumMember(Value = "noauth")]
    NoAuth,

    [EnumMember(Value = "password")]
    Password,
}