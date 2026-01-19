namespace Xray.Core.Models;

public class X25519Data
{
    public required string PrivateKey { get; set; }
    public required string Password { get; set; }
    public required string Hash { get; set; }
}