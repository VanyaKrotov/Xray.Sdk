using System.Text.Json.Serialization;

namespace Xray.Config.Models;

/// <summary>
/// A reverse proxy can redirect traffic from a server to a client, that is, perform reverse traffic forwarding.
/// <para><see href="https://xtls.github.io/config/reverse.html">Docs</see></para>
/// </summary>
public class ReverseConfig
{
    /// <summary>
    /// An array where each element is a bridge.
    /// </summary>
    [JsonPropertyName("bridges")]
    public List<ReverseBridge>? Bridges { get; set; }

    /// <summary>
    /// An array where each element is a portal.
    /// </summary>
    [JsonPropertyName("portals")]
    public List<ReversePortal>? Portals { get; set; }
}

/// <summary>
/// Bridge configuration object
/// </summary>
/// <param name="Tag">All connections originating from bridge will have this label. It can be used for identification in <see href="https://xtls.github.io/config/routing.html">the routing configuration</see> using inboundTag</param>
/// <param name="Domain">Specifies the domain to bridge use to establish a connection with portal. This domain is used only for communication between bridge and portal and does not need to exist.</param>
public record ReverseBridge(string Tag, string Domain);

/// <summary>
/// Portal configuration object
/// </summary>
/// <param name="Tag">Label portal. Used in <see href="https://xtls.github.io/config/routing.html">routing configuration</see> to outboundTag redirect traffic to this portal</param>
/// <param name="Domain">When the portal receives traffic, if the target domain of the traffic matches this domain, the portal assumes that the current connection is a communication connection established by the bridge. Otherwise, the traffic is considered to be traffic that requires forwarding. The portal identifies these two types of connections and performs the appropriate forwarding.</param>
public record ReversePortal(string Tag, string Domain);
