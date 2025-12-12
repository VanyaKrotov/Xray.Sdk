using System.Text.Json.Serialization;

namespace Xray.Config.Models;

public class FaceDnsConfig
{
    [JsonPropertyName("ipPool")]
    public string? IpPool { get; set; }

    [JsonPropertyName("poolSize")]
    public int? PoolSize { get; set; }
}