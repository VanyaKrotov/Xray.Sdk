using Xray.Config.Utilities;

namespace Xray.Config.Enums;

/// <summary>
/// Mode for sending data from the client to the server.
/// </summary>
public enum XHttpMode
{
    /// <summary>
    /// Xray chooses the optimal mode by itself.
    /// </summary>
    [EnumProperty("auto")]
    Auto,

    /// <summary>
    /// The client sends data in batches via POST requests.
    /// </summary>
    [EnumProperty("packet-up")]
    PacketUp,

    /// <summary>
    /// The client keeps a constant stream up.
    /// </summary>
    [EnumProperty("stream-up")]
    StreamUp,
}