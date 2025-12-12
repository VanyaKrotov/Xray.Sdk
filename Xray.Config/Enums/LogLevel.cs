using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Xray.Config.Utilities;

namespace Xray.Config.Enums;

[JsonConverter(typeof(EnumMemberConverter<LogLevel>))]
public enum LogLevel
{
    [EnumMember(Value = "none")]
    None,

    [EnumMember(Value = "debug")]
    Debug,

    [EnumMember(Value = "info")]
    Info,

    [EnumMember(Value = "warning")]
    Warning,

    [EnumMember(Value = "error")]
    Error,
}