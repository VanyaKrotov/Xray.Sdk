using Xray.Config.Utilities;

namespace Xray.Config.Enums;

public enum Fingerprint
{
    [EnumProperty("")]
    None,

    [EnumProperty("chrome")]
    Chrome,

    [EnumProperty("firefox")]
    Firefox,

    [EnumProperty("safari")]
    Safari,

    [EnumProperty("ios")]
    iOS,

    [EnumProperty("android")]
    Android,

    [EnumProperty("edge")]
    Edge,

    [EnumProperty("360")]
    e360,

    [EnumProperty("qq")]
    Qq,

    /// <summary>
    /// [DANGER] For security reasons, this parameter should not be use.
    /// </summary>
    [EnumProperty("unsafe")]
    Unsafe,

    /// <summary>
    /// Random selection of new browser versions.
    /// </summary>
    [EnumProperty("random")]
    Random,

    /// <summary>
    /// Full random generation of a unique fingerprint (100% support for TLS 1.3 using X25519)
    /// </summary>
    [EnumProperty("randomized")]
    Randomized,
}