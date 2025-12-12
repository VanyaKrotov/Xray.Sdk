using System.Text.Json.Serialization;

namespace Xray.Config.Models;

public class PolicyConfig
{
    [JsonPropertyName("levels")]
    public Dictionary<string, LevelPolicy>? Levels { get; set; }

    [JsonPropertyName("system")]
    public SystemPolicy? System { get; set; }
}

public class LevelPolicy
{
    [JsonPropertyName("handshake")]
    public int? Handshake { get; set; }

    [JsonPropertyName("connIdle")]
    public int? ConnIdle { get; set; }

    [JsonPropertyName("UplinkOnly")]
    public int? UplinkOnly { get; set; }

    [JsonPropertyName("DownlinkOnly")]
    public int? DownlinkOnly { get; set; }

    [JsonPropertyName("statsUserUplink")]
    public bool? StatsUserUplink { get; set; }

    [JsonPropertyName("statsUserDownlink")]
    public bool? StatsUserDownlink { get; set; }

    [JsonPropertyName("bufferSize")]
    public long? BufferSize { get; set; }
}

public class SystemPolicy
{
    [JsonPropertyName("statsInboundUplink")]
    public bool? StatsInboundUplink { get; set; }

    [JsonPropertyName("StatsInboundDownlink")]
    public bool? StatsInboundDownlink { get; set; }

    [JsonPropertyName("statsOutboundUplink")]
    public bool? StatsOutboundUplink { get; set; }

    [JsonPropertyName("statsOutboundDownlink")]
    public bool? StatsOutboundDownlink { get; set; }
}