using System.Text.Json.Serialization;

namespace Xray.Config.Models;

public class ObservatoryConfig
{
    [JsonPropertyName("subjectSelector")]
    public required List<string> SubjectSelector { get; set; }

    [JsonPropertyName("probeUrl")]
    public required string ProbeUrl { get; set; }

    [JsonPropertyName("probeInterval")]
    public required string ProbeInterval { get; set; }

    [JsonPropertyName("enableConcurrency")]
    public bool EnableConcurrency { get; set; }
}