using System.Text.Json.Serialization;

namespace Xray.Config.Models;

public class Fallback
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("alpn")]
    public string? Alpn { get; set; }

    [JsonPropertyName("path")]
    public string? Path { get; set; }

    [JsonPropertyName("dest")]
    public int? Dest { get; set; }

    [JsonPropertyName("xver")]
    public int? XVer { get; set; }
}