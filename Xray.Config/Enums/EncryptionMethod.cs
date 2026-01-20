using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Xray.Config.Utilities;

namespace Xray.Config.Enums;

[JsonConverter(typeof(EnumMemberConverter<EncryptionMethod>))]
public enum EncryptionMethod
{
    /// <summary>
    /// Password length 16
    /// </summary>
    [EnumMember(Value = "2022-blake3-aes-128-gcm")]
    Blake3Aes128Gcm,

    /// <summary>
    /// Password length 32
    /// </summary>
    [EnumMember(Value = "2022-blake3-aes-256-gcm")]
    Blake3Aes256Gcm,

    /// <summary>
    /// Password length 32
    /// </summary>
    [EnumMember(Value = "2022-blake3-chacha20-poly1305")]
    Blake3Chacha20Poly1305,

    [EnumMember(Value = "aes-256-gcm")]
    Aes256Gcm,

    [EnumMember(Value = "aes-128-gcm")]
    Aes128Gcm,

    [EnumMember(Value = "chacha20-poly1305")]
    Chacha20Poly1305,

    [EnumMember(Value = "xchacha20-poly1305")]
    XChacha20Poly1305,

    [EnumMember(Value = "none")]
    None,
}

