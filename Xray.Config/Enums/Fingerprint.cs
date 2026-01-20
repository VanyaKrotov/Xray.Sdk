using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Xray.Config.Utilities;

namespace Xray.Config.Enums;

[JsonConverter(typeof(EnumMemberConverter<Fingerprint>))]
public enum Fingerprint
{
    [EnumMember(Value = "")]
    None,

    [EnumMember(Value = "chrome")]
    Chrome,

    [EnumMember(Value = "firefox")]
    Firefox,

    [EnumMember(Value = "safari")]
    Safari,

    [EnumMember(Value = "ios")]
    iOS,

    [EnumMember(Value = "android")]
    Android,

    [EnumMember(Value = "edge")]
    Edge,

    [EnumMember(Value = "360")]
    e360,

    [EnumMember(Value = "qq")]
    Qq,

    /// <summary>
    /// [DANGER] For security reasons, this parameter should not be use.
    /// </summary>
    [EnumMember(Value = "unsafe")]
    Unsafe,

    /// <summary>
    /// Random selection of new browser versions.
    /// </summary>
    [EnumMember(Value = "random")]
    Random,

    /// <summary>
    /// Full random generation of a unique fingerprint (100% support for TLS 1.3 using X25519)
    /// </summary>
    [EnumMember(Value = "randomized")]
    Randomized,
}