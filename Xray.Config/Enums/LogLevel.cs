using Xray.Config.Utilities;

namespace Xray.Config.Enums;

/// <summary>
/// The log level for error logs
/// </summary>
public enum LogLevel
{
    /// <summary>
    /// Disable all logs.
    /// </summary>
    [EnumProperty("none")]
    None,

    /// <summary>
    /// Output information used for debugging the program. Includes all "info" content.
    /// </summary>
    [EnumProperty("debug")]
    Debug,

    /// <summary>
    /// Runtime status information, etc., which does not affect normal use. Includes all "warning" content.
    /// </summary>
    [EnumProperty("info")]
    Info,

    /// <summary>
    /// Information output when there are some problems that do not affect normal operation but may affect user experience. Includes all "error" content.
    /// </summary>
    [EnumProperty("warning")]
    Warning,

    /// <summary>
    /// Xray encountered a problem that cannot be run normally and needs to be resolved immediately.
    /// </summary>
    [EnumProperty("error")]
    Error,
}