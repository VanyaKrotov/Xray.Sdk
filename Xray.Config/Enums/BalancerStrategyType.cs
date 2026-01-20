using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Xray.Config.Utilities;

namespace Xray.Config.Enums;

/// <summary>
/// Balance strategy type
/// </summary>
[JsonConverter(typeof(EnumMemberConverter<BalancerStrategyType>))]
public enum BalancerStrategyType
{
    /// <summary>
    /// Randomly selects the appropriate outbound proxy.
    /// </summary>
    [EnumMember(Value = "random")]
    Random,

    /// <summary>
    /// Selects the corresponding outgoing proxies in turn.
    /// </summary>
    [EnumMember(Value = "roundRobin")]
    RoundRobin,

    /// <summary>
    /// Selects the outbound proxy with the lowest latency based on connection observation results. Requires an observatory or burstObservatory configuration.
    /// </summary>
    [EnumMember(Value = "leastPing")]
    LeastPing,

    /// <summary>
    /// Selects the most stable outbound proxy based on connection monitoring results. Requires an observatory or burstObservatory configuration.
    /// </summary>
    [EnumMember(Value = "leastLoad")]
    LeastLoad,
}