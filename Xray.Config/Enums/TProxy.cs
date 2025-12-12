using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Xray.Config.Utilities;

namespace Xray.Config.Enums;

[JsonConverter(typeof(EnumMemberConverter<TProxy>))]
public enum TProxy
{
    [EnumMember(Value = "redirect")]
    Redirect,

    [EnumMember(Value = "tproxy")]
    On,

    [EnumMember(Value = "off")]
    Off,
}