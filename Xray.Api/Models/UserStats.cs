namespace Xray.Api.Models;

/// <summary>
/// Represents per-user traffic statistics returned by the Xray-core QueryStats API.
/// </summary>
public class UserStats
{
    /// <summary>
    /// The email identifier of the user whose statistics are being reported.
    /// This corresponds to the "email" field in the user's inbound configuration.
    /// </summary>
    public required string Email { get; set; }

    /// <summary>
    /// Total amount of uploaded traffic (in bytes) sent by the user.
    /// Represents outbound data from the user's perspective.
    /// </summary>
    public long Uplink { get; set; }

    /// <summary>
    /// Total amount of downloaded traffic (in bytes) received by the user.
    /// Represents inbound data from the user's perspective.
    /// </summary>
    public long Downlink { get; set; }
}
