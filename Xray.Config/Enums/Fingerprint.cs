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

    [EnumMember(Value = "random")]
    Random,

    [EnumMember(Value = "randomized")]
    Randomized,
}