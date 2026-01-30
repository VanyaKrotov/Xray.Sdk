using System.Collections.Specialized;
using System.Text.Json.Serialization;
using Xray.Config.Enums;

namespace Xray.Config.Models;

/// <summary>
/// XHTTP configuration for the current connection, valid only if this connection uses XHTTP.
/// </summary>
public class XHttpSettings
{
    /// <summary>
    /// Host header: in the HTTP request.
    /// </summary>
    [JsonPropertyName("host")]
    public string? Host { get; set; }

    /// <summary>
    /// The HTTP path that the client uses to send requests.
    /// </summary>
    [JsonPropertyName("path")]
    public string? Path { get; set; }

    /// <summary>
    /// Mode for sending data from the client to the server. Use client-only.
    /// </summary>
    [JsonPropertyName("mode")]
    public XHttpMode Mode { get; set; } = XHttpMode.Auto;

    /// <summary>
    /// The sharing scheme for the original JSON for all parameters except host, path, mode. When extra is present, only these four parameters are valid.
    /// </summary>
    [JsonPropertyName("extra")]
    public XHttpExtraSettings Extra { get; set; } = new();

    /// <summary>
    /// Additional HTTP headers.
    /// </summary>
    [JsonPropertyName("headers")]
    public NameValueCollection Headers { get; set; } = new();
}

public class XHttpExtraSettings
{
    [JsonPropertyName("header")]
    public Dictionary<string, string> Headers { get; set; } = new();

    [JsonPropertyName("xPaddingBytes")]
    public string? XPaddingBytes { get; set; }

    [JsonPropertyName("noGRPCHeader")]
    public bool? NoGRPCHeader { get; set; }

    [JsonPropertyName("noSSEHeader")]
    public bool? NoSSEHeader { get; set; }

    [JsonPropertyName("scMaxEachPostBytes")]
    public long? ScMaxEachPostBytes { get; set; }

    [JsonPropertyName("scMinPostsIntervalMs")]
    public int? ScMinPostsIntervalMs { get; set; }

    [JsonPropertyName("scMaxBufferedPosts")]
    public int? ScMaxBufferedPosts { get; set; }

    [JsonPropertyName("scStreamUpServerSecs")]
    public string? ScStreamUpServerSecs { get; set; }

    [JsonPropertyName("xmux")]
    public XMux XMux { get; set; } = new()
    {
        MaxConcurrency = "16-32",
        MaxConnections = 0,
        CMaxReuseTimes = 0,
        HMaxRequestTimes = "600-900",
        HMaxReusableSecs = "1800-3000",
        HKeepAlivePeriod = 0
    };

    [JsonPropertyName("downloadSettings")]
    public XHttpDownloadSettings? DownloadSettings { get; set; }
}

public class XMux
{
    [JsonPropertyName("maxConcurrency")]
    public string? MaxConcurrency { get; set; }

    [JsonPropertyName("maxConnections")]
    public int? MaxConnections { get; set; }

    [JsonPropertyName("cMaxReuseTimes")]
    public int? CMaxReuseTimes { get; set; }

    [JsonPropertyName("hMaxRequestTimes")]
    public string? HMaxRequestTimes { get; set; }

    [JsonPropertyName("hMaxReusableSecs")]
    public string? HMaxReusableSecs { get; set; }

    [JsonPropertyName("hKeepAlivePeriod")]
    public int? HKeepAlivePeriod { get; set; }
}

public class XHttpDownloadSettings : StreamSettings
{
    [JsonPropertyName("address")]
    public string? Address { get; set; }

    [JsonPropertyName("port")]
    public int? Port { get; set; }
}