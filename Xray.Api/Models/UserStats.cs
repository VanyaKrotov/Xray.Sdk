namespace Xray.Api.Models;

public class UserStats
{
    public required string Email { get; set; }

    public long Uplink { get; set; }

    public long Downlink { get; set; }
}