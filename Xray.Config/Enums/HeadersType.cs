using Xray.Config.Utilities;

namespace Xray.Config.Enums;

public enum HeadersType
{
    /// <summary>
    /// Specifies that no masking is performed.
    /// </summary>
    [EnumProperty("none")]
    None,

    /// <summary>
    /// Specifies that HTTP cloaking is being performed.
    /// </summary>
    [EnumProperty("http")]
    Http,
}