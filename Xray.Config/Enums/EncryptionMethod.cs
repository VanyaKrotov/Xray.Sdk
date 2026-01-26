
using Xray.Config.Utilities;

namespace Xray.Config.Enums;

public enum EncryptionMethod
{
    /// <summary>
    /// Password length 16
    /// </summary>
    [EnumProperty("2022-blake3-aes-128-gcm")]
    Blake3Aes128Gcm,

    /// <summary>
    /// Password length 32
    /// </summary>
    [EnumProperty("2022-blake3-aes-256-gcm")]
    Blake3Aes256Gcm,

    /// <summary>
    /// Password length 32
    /// </summary>
    [EnumProperty("2022-blake3-chacha20-poly1305")]
    Blake3Chacha20Poly1305,

    [EnumProperty("aes-256-gcm")]
    Aes256Gcm,

    [EnumProperty("aes-128-gcm")]
    Aes128Gcm,

    [EnumProperty("chacha20-poly1305")]
    Chacha20Poly1305,

    [EnumProperty("xchacha20-poly1305")]
    XChacha20Poly1305,

    [EnumProperty("none")]
    None,
}

