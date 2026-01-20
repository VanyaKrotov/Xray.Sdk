using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Xray.Config.Utilities;

namespace Xray.Config.Enums;

/// <summary>
/// The log level for error logs
/// </summary>
[JsonConverter(typeof(EnumMemberConverter<LogLevel>))]
public enum LogLevel
{
    /// <summary>
    /// Disable all logs.
    /// </summary>
    [EnumMember(Value = "none")]
    None,

    /// <summary>
    /// Output information used for debugging the program. Includes all "info" content.
    /// </summary>
    [EnumMember(Value = "debug")]
    Debug,

    /// <summary>
    /// Runtime status information, etc., which does not affect normal use. Includes all "warning" content.
    /// </summary>
    [EnumMember(Value = "info")]
    Info,

    /// <summary>
    /// Information output when there are some problems that do not affect normal operation but may affect user experience. Includes all "error" content.
    /// </summary>
    [EnumMember(Value = "warning")]
    Warning,

    /// <summary>
    /// Xray encountered a problem that cannot be run normally and needs to be resolved immediately.
    /// </summary>
    [EnumMember(Value = "error")]
    Error,
}