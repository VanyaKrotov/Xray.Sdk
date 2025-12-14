namespace Xray.Core.Models;

public class PingOptions
{
    public required int Port { get; set; }

    public string TestingUrl { get; set; } = "https://www.gstatic.com/generate_204";
}