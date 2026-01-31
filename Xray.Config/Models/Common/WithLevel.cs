using System.Text.Json.Serialization;

namespace Xray.Config.Models;

public abstract class WithLevel
{
    /// <summary>
    /// User level. <see href="https://xtls.github.io/config/policy.html#levelpolicyobject">The local policy</see> corresponding to this user level will be used for the connection. The value level corresponds to the value level in the policy section. If not specified, the default value of 0 is used.
    /// </summary>
    [JsonPropertyName("level")]
    public int? Level { get; set; }
}
