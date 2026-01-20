using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Xray.Config.Utilities;

namespace Xray.Config.Enums;

/// <summary>
/// Mode for sending data from the client to the server.
/// </summary>
[JsonConverter(typeof(EnumMemberConverter<XHttpMode>))]
public enum XHttpMode
{
    /// <summary>
    /// Xray chooses the optimal mode by itself.
    /// </summary>
    [EnumMember(Value = "auto")]
    Auto,

    /// <summary>
    /// The client sends data in batches via POST requests.
    /// </summary>
    [EnumMember(Value = "packet-up")]
    PacketUp,

    /// <summary>
    /// The client keeps a constant stream up.
    /// </summary>
    [EnumMember(Value = "stream-up")]
    StreamUp,
}