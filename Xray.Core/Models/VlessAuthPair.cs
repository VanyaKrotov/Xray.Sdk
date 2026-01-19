namespace Xray.Core.Models;

public class VlessAuthentication
{
    public required VlessAuthPair X25519 { get; set; }

    public required VlessAuthPair MLKEM768 { get; set; }
}

public class VlessAuthPair
{
    public required string Decryption { get; set; }

    public required string Encryption { get; set; }
}