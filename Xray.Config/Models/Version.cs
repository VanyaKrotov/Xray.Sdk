using System.Text.Json.Serialization;

namespace Xray.Config.Models;

public class VersionConfig
{
    [JsonPropertyName("min")]
    public string? Min { get; set; }

    [JsonPropertyName("max")]
    public string? Max { get; set; }
}