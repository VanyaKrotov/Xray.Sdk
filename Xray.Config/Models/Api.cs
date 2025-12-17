using System.Text.Json.Serialization;

namespace Xray.Config.Models;

public class ApiConfig
{
    [JsonPropertyName("tag")]
    public required string Tag { get; set; }

    [JsonPropertyName("listen")]
    public string? Listen { get; set; }

    [JsonPropertyName("services")]
    public List<string>? Services { get; set; }
}

public static class ApiServices
{
    public const string Handler = "HandlerService";
    public const string Logger = "LoggerService";
    public const string Stats = "StatsService";
    public const string Routing = "RoutingService";
    public const string Reflection = "ReflectionService";
}