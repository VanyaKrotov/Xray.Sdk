using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Xray.Config.Utilities;

namespace Xray.Config.Enums;

[JsonConverter(typeof(EnumMemberConverter<VMessSecurity>))]
public enum VMessSecurity
{
    /// <summary>
    /// Without encryption, the message structure is preserved VMess.
    /// </summary>
    [EnumMember(Value = "none")]
    None,

    /// <summary>
    /// Use algorithm AES-128-GCM
    /// </summary>
    [EnumMember(Value = "aes-128-gcm")]
    Aes128Gcm,

    /// <summary>
    /// Use algorithm Chacha20-Poly1305
    /// </summary>
    [EnumMember(Value = "chacha20-poly1305")]
    Chacha20Poly1305,

    /// <summary>
    /// Default value. Automatic selection (for architectures AMD64, ARM64or , s390xthe encryption method will be selected aes-128-gcm; otherwise, Chacha20-Poly1305)
    /// </summary>
    [EnumMember(Value = "auto")]
    Auto,

    /// <summary>
    /// Without encryption, the data stream is copied directly (similar to VLESS).
    /// </summary>
    [EnumMember(Value = "zero")]
    Zero,
}