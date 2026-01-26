using Xray.Config.Utilities;

namespace Xray.Config.Enums;

/// <summary>
/// Balance strategy type
/// </summary>
public enum BalancerStrategyType
{
    /// <summary>
    /// Randomly selects the appropriate outbound proxy.
    /// </summary>
    [EnumProperty("random")]
    Random,

    /// <summary>
    /// Selects the corresponding outgoing proxies in turn.
    /// </summary>
    [EnumProperty("roundRobin")]
    RoundRobin,

    /// <summary>
    /// Selects the outbound proxy with the lowest latency based on connection observation results. Requires an observatory or burstObservatory configuration.
    /// </summary>
    [EnumProperty("leastPing")]
    LeastPing,

    /// <summary>
    /// Selects the most stable outbound proxy based on connection monitoring results. Requires an observatory or burstObservatory configuration.
    /// </summary>
    [EnumProperty("leastLoad")]
    LeastLoad,
}