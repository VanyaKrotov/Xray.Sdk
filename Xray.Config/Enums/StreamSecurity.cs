using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Xray.Config.Utilities;

namespace Xray.Config.Enums;

/// <summary>
/// Whether transport layer encryption type.
/// </summary>
[JsonConverter(typeof(EnumMemberConverter<StreamSecurity>))]
public enum StreamSecurity
{
    /// <summary>
    /// Means no encryption.
    /// </summary>
    [EnumMember(Value = "none")]
    None,

    /// <summary>
    /// Means using <see href="https://en.wikipedia.org/wiki/Transport_Layer_Security">TLS</see>
    /// </summary>
    [EnumMember(Value = "tls")]
    Tls,

    /// <summary>
    /// Means using REALITY.
    /// </summary>
    [EnumMember(Value = "reality")]
    Reality
}
