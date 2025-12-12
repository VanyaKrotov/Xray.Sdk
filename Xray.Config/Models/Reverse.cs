using System.Text.Json.Serialization;

namespace Xray.Config.Models;

public class ReverseConfig
{
    [JsonPropertyName("bridges")]
    public List<ReverseBridge>? Bridges { get; set; }

    [JsonPropertyName("portals")]
    public List<ReversePortal>? Portals { get; set; }
}

public record ReverseBridge(string Tag, string Domain);

public record ReversePortal(string Tag, string Domain);
