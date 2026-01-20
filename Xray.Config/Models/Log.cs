using System.Text.Json.Serialization;
using Xray.Config.Enums;

namespace Xray.Config.Models;

/// <summary>
/// Log configuration controls how Xray outputs logs.
/// <para><see href="https://xtls.github.io/en/config/log.html">Docs</see></para>
/// </summary>
public class LogConfig
{
    /// <summary>
    /// The file path for the access log. The value is a valid file path, such as "/var/log/Xray/access.log" (Linux) or "C:\\Temp\\Xray\\_access.log" (Windows). When this item is not specified or is an empty value, the log is output to stdout.
    /// </summary>
    [JsonPropertyName("access")]
    public string? Access { get; set; }

    /// <summary>
    /// The file path for the error log. The value is a valid file path, such as "/var/log/Xray/error.log" (Linux) or "C:\\Temp\\Xray\\_error.log" (Windows). When this item is not specified or is an empty value, the log is output to stdout.
    /// </summary>
    [JsonPropertyName("error")]
    public string? Error { get; set; }

    /// <summary>
    /// The log level for error logs, indicating the information that needs to be recorded. The default value is "warning". Note that this setting applies to the error log only. It doesn't affect the access log (except for "none" value). The access log doesn't have log levels.
    /// </summary>
    [JsonPropertyName("loglevel")]
    public LogLevel? LogLevel { get; set; }

    /// <summary>
    /// Log DNS queries made by built-in DNS clients to the access log. Example log record: DOH//doh.server got answer: domain.com -> [ip1, ip2] 2.333ms.
    /// </summary>
    [JsonPropertyName("dnsLog")]
    public bool DnsLog { get; set; }

    /// <summary>
    /// IP address masking, when enabled, will automatically replace the IP address appearing in the log. It is used to protect privacy when sharing logs.
    /// <para>The default is empty and is not enabled. Currently available levels are quarter, half, full.</para>
    /// The mask form corresponds to the following:
    /// <list type="bullet">
    ///     <item>ipv4 1.2.*.* 1.*.*.* [Masked IPv4];</item>
    ///     <item>ipv6 1234:5678::/32 1234::/16 [Masked IPv6];</item>
    /// </list> 
    /// </summary>
    [JsonPropertyName("maskAddress")]
    public string? MaskAddress { get; set; }
}