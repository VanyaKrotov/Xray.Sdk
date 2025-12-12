using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Xray.Config.Utilities;

namespace Xray.Config.Enums;

[JsonConverter(typeof(EnumMemberConverter<VMessSecurity>))]
public enum VMessSecurity
{
    [EnumMember(Value = "none")]
    None,

    [EnumMember(Value = "aes-128-gcm")]
    Aes128Gcm,

    [EnumMember(Value = "chacha20-poly1305")]
    Chacha20Poly1305,

    [EnumMember(Value = "auto")]
    Auto,

    [EnumMember(Value = "zero")]
    Zero,
}