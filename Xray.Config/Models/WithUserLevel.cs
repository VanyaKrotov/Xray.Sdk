using System.Text.Json.Serialization;

namespace Xray.Config.Models;

public abstract class WithUserLevel
{
    /// <summary>
    /// User level, <see href="https://xtls.github.io/config/policy.html#levelpolicyobject">the local policy</see> corresponding to this user level will be used for the connection.
    /// <para>The userLevel value corresponds to the value level in the policy section . If not specified, the default value of 0 is used.</para>
    /// </summary>
    [JsonPropertyName("userLevel")]
    public int UserLevel { get; set; }
}
