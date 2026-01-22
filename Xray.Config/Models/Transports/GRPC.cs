using System.Text.Json.Serialization;

namespace Xray.Config.Models;

/// <summary>
/// gRPC configuration for the current connection, valid only if this connection uses gRPC.
/// </summary>
public class GRPCSettings
{
    /// <summary>
    /// A string that can be used as Host for some other purpose.
    /// </summary>
    [JsonPropertyName("authority")]
    public string? Authority { get; set; }

    /// <summary>
    /// A string specifying the service name, similar to a path in HTTP/2. The client will use this name to communicate, and the server will check whether the service name matches.
    /// </summary>
    [JsonPropertyName("serviceName")]
    public string? ServiceName { get; set; }

    /// <summary>
    /// [BETA] true includes multiMode, default value: false.
    /// </summary>
    [JsonPropertyName("multiMode")]
    public bool MultiMode { get; set; }

    /// <summary>
    /// Setting a gRPC user agent can prevent gRPC traffic from being blocked by some CDNs.
    /// </summary>
    [JsonPropertyName("user_agent")]
    public string? UserAgent { get; set; }

    /// <summary>
    /// A health check is performed if no data is transmitted for a specified period of time, measured in seconds. If this value is less than 10, then will be used as the minimum value 10.
    /// </summary>
    [JsonPropertyName("idle_timeout")]
    public int? IdleTimeout { get; set; }

    /// <summary>
    /// The health check response timeout in seconds. If the health check is not completed within this time and there is still no data transfer, the health check will be considered a failure. Default value: 20.
    /// </summary>
    [JsonPropertyName("health_check_timeout")]
    public int? HealthCheckTimeout { get; set; }

    /// <summary>
    /// true enables health checking if there are no child connections. Default value: false.
    /// </summary>
    [JsonPropertyName("permit_without_stream")]
    public bool PermitWithoutStream { get; set; }

    /// <summary>
    /// The initial h2 Stream window size. If the value is less than or equal to 0, this feature has no effect. If the value is greater than 65535, the dynamic window mechanism is disabled. The default value is 0, meaning it has no effect.
    /// </summary>
    [JsonPropertyName("initial_windows_size")]
    public int? InitialWindowsSize { get; set; }
}