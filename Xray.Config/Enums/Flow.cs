using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Xray.Config.Utilities;

namespace Xray.Config.Enums;

[JsonConverter(typeof(EnumMemberConverter<Flow>))]
public enum Flow
{
    [EnumMember(Value = "")]
    None,

    [EnumMember(Value = "xtls-rprx-vision")]
    XtlsRprxVision,
}