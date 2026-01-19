namespace Xray.Core.Models;

public class MLKEM768Data
{
    public required string Seed { get; set; }

    public required string Client { get; set; }

    public required string Hash { get; set; }
}