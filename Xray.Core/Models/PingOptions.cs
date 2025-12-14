namespace Xray.Core.Models;

public class PingOptions
{
    public string Host { get; set; } = "127.0.0.1";

    public int Port { get; set; } = 80;

    public string TestingUrl { get; set; } = "https://www.gstatic.com/generate_204";

    public int Timeout { get; set; } = 10 * 1000;
}