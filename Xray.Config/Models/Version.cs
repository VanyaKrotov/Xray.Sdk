using System.Text.Json.Serialization;

namespace Xray.Config.Models;

/// <summary>
/// It controls the version on which this config can run. When sharing config files, this prevents accidental execution on unwanted client versions. During execution, the client will check if its current version meets this requirement. Support of v25.8.3+.
/// <para><see href="https://xtls.github.io/ru/config/#%D0%BE%D1%81%D0%BD%D0%BE%D0%B2%D0%BD%D1%8B%D0%B5-%D0%BC%D0%BE%D0%B4%D1%83%D0%BB%D0%B8-%D0%BA%D0%BE%D0%BD%D1%84%D0%B8%D0%B3%D1%83%D1%80%D0%B0%D1%86%D0%B8%D0%B8">Docs</see></para>
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