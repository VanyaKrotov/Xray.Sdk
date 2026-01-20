using System.Text.Json.Serialization;

namespace Xray.Config.Models;

/// <summary>
/// API interface configuration provides a set of APIs based on gRPC for remote invocation.
/// <para><see href="https://xtls.github.io/config/api.html">Docs</see></para>
/// </summary>
public class ApiConfig
{
    /// <summary>
    /// Outbound proxy identifier.
    /// </summary>
    [JsonPropertyName("tag")]
    public required string Tag { get; set; }

    /// <summary>
    /// The IP and port that the API service listens on. This is an optional configuration item.
    /// <para>When you omit this item, you need to add inbounds and routing configurations according to the examples in the <see href="https://xtls.github.io/config/api.html#related-configuration">relevant configurations below</see>.</para>
    /// </summary>
    [JsonPropertyName("listen")]
    public string? Listen { get; set; }

    /// <summary>
    /// List of enabled APIs, optional values can be found in <see href="https://xtls.github.io/config/api.html#supported-api-list">Supported API List</see>.
    /// </summary>
    [JsonPropertyName("services")]
    public List<string>? Services { get; set; }
}

/// <summary>
/// Supported API List
/// </summary>
public static class ApiServices
{
    /// <summary>
    /// APIs that modify the inbound and outbound proxies, with the following available functions:
    /// <list type="bullet">
    ///     <item>Add a new inbound proxy;</item>
    ///     <item>Add a new outbound proxy;</item>
    ///     <item>Delete an existing inbound proxy;</item>
    ///     <item>Delete an existing outbound proxy;</item>
    ///     <item>List inbound proxies;</item>
    ///     <item>List outbound proxies;</item>
    ///     <item>Add a user to an inbound proxy (VMess, VLESS, Trojan, and Shadowsocks(v1.3.0+) only);</item>
    ///     <item>Delete a user from an inbound proxy (VMess, VLESS, Trojan, and Shadowsocks(v1.3.0+) only);</item>
    /// </list>
    /// </summary>
    public const string Handler = "HandlerService";

    /// <summary>
    /// Supports restarting the built-in logger, which can be used in conjunction with logrotate to perform operations on log files.
    /// </summary>
    public const string Logger = "LoggerService";

    /// <summary>
    /// Built-in data statistics service, see <see href="https://xtls.github.io/config/stats.html">Statistics Information</see> for details
    /// </summary>
    public const string Stats = "StatsService";

    /// <summary>
    /// API for adding, deleting, and replacing routing rules and querying equalizer statistics.
    /// <para>The available functions are as follows:</para>
    /// <list type="bullet">
    ///     <item><b>adrules</b> - adds and replaces routing configuration</item>
    ///     <item><b>rmrules</b> - delete routing rules</item>
    ///     <item><b>sib</b> - disconnect source IP</item>
    ///     <item><b>bi</b> - query equalizer statistics</item>
    ///     <item><b>bo</b> - forces the equalizer to select the specified outboundTag</item>
    /// </list>
    /// </summary>
    public const string Routing = "RoutingService";

    /// <summary>
    /// Supports gRPC clients to obtain the list of APIs from the server.
    /// </summary>
    public const string Reflection = "ReflectionService";
}