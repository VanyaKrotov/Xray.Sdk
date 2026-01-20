using System.Text.Json.Serialization;

namespace Xray.Config.Models;

/// <summary>
/// Setting up FakeDNS. Can be used in conjunction with transparent proxying to obtain real domain names.
/// <para><see href="https://xtls.github.io/config/fakedns.html">Docs</see></para>
/// </summary>
public class FaceDnsConfig
{
    /// <summary>
    /// FakeDNS will use the IP block specified by this option to allocate addresses.
    /// </summary>
    [JsonPropertyName("ipPool")]
    public string? IpPool { get; set; }

    /// <summary>
    /// Specifies the maximum number of domain name-IP mappings stored by FakeDNS. When the number of mappings exceeds this value, mappings will be eliminated according to the LRU rule. The default is 65535.
    /// </summary>
    [JsonPropertyName("poolSize")]
    public int? PoolSize { get; set; }
}