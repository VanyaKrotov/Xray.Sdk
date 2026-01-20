using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Xray.Config.Utilities;

namespace Xray.Config.Enums;

[JsonConverter(typeof(EnumMemberConverter<RawHeadersType>))]
public enum RawHeadersType
{   
    /// <summary>
    /// Specifies that no masking is performed.
    /// </summary>
    [EnumMember(Value = "none")]
    None,

    /// <summary>
    /// Specifies that HTTP cloaking is being performed.
    /// </summary>
    [EnumMember(Value = "http")]
    Http,
}