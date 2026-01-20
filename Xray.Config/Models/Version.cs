using System.Text.Json.Serialization;

namespace Xray.Config.Models;

/// <summary>
/// It controls the version on which this config can run. When sharing config files, this prevents accidental execution on unwanted client versions. During execution, the client will check if its current version meets this requirement. Support of v25.8.3+.
/// <para><see href="https://xtls.github.io/config/#%E5%9F%BA%E7%A1%80%E9%85%8D%E7%BD%AE%E6%A8%A1%E5%9D%97">Docs</see></para>
/// </summary>
public class VersionConfig
{
    /// <summary>
    /// Min config version. Format: x.y.z
    /// </summary>
    [JsonPropertyName("min")]
    public string? Min { get; set; }

    /// <summary>
    /// Max config version. Format: x.y.z
    /// </summary>
    [JsonPropertyName("max")]
    public string? Max { get; set; }
}