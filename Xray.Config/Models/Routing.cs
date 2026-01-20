using System.Text.Json.Serialization;
using Xray.Config.Enums;

namespace Xray.Config.Models;

/// <summary>
/// Configures routing. Specify rules to route connections through different outbounds.
/// <para><see href="https://xtls.github.io/config/routing.html">Docs</see></para>
/// </summary>
public class RoutingConfig
{
    /// <summary>
    /// Domain name resolution strategy. Different strategies are used depending on the configuration. Default value is "AsIs".
    /// </summary>
    [JsonPropertyName("domainStrategy")]
    public RoutingDomainStrategy? DomainStrategy { get; set; }

    /// <summary>
    /// Matches an array, each element of which is a rule.
    /// </summary>
    [JsonPropertyName("rules")]
    public List<RoutingRule> Rules { get; set; } = new();

    /// <summary>
    /// An array where each element is a load balancer configuration.
    /// </summary>
    [JsonPropertyName("balancers")]
    public List<RoutingBalancer> Balancers { get; set; } = new();
}

/// <summary>
/// Routing rule object
/// </summary>
public class RoutingRule
{
    /// <summary>
    /// Routing rule type
    /// </summary>
    [JsonPropertyName("type")]
    public RoutingRuleType Type { get; set; } = RoutingRuleType.Field;

    /// <summary>
    /// An array where each element represents a domain name mapping. The following formats are possible:
    /// <list type="bullet">
    ///     <item>Simple string: The rule takes effect if this string matches any part of the target domain name. For example, "sina.com" can match "sina.com," "sina.com.cn," and "www.sina.com," but it cannot match "sina.cn."</item>
    ///     <item>Regular expression: Starts with "regexp:", the rest is a regular expression. The rule takes effect if this regular expression matches the target domain name. For example, "regexp:\\\\.goo.\*\\\\.com\$"it matches "www.google.com" or "fonts.googleapis.com," but does not match "google.com." (Note that in JSON, the backslash, often used in regular expressions, is used as an escape character, so the backslash \in the regular expression should be replaced with \\.)</item>
    ///     <item>Subdomain (recommended): Starts with "domain:", the rest is the domain name. The rule takes effect if this domain name is the target domain name or a subdomain of it. For example, "domain:xray.com" matches "www.xray.com" and "xray.com," but does not match "wxray.com."</item>
    ///     <item>Exact match: starts with "full:", the rest is the domain name. The rule takes effect if this domain name exactly matches the target domain name. For example, "full:xray.com" matches "xray.com" but does not match "www.xray.com".</item>
    ///     <item>Predefined domain list: Starts with "geosite:", the rest is a name, such as geosite:googleor geosite:cn. For domain names and lists, see <see href="https://xtls.github.io/config/routing.html#%E8%B7%AF%E7%94%B1">Predefined Domain Lists</see></item>
    ///     <item>Loading domain names from file: has the form "ext:file:tag", must start with ext:(in lowercase) followed by the file name and tag, the file is stored in the resources directory , the file format is the same as geosite.dat, the tag must exist in the file.</item>
    /// </list>
    /// </summary>
    [JsonPropertyName("domain")]
    public List<string>? Domain { get; set; }

    /// <summary>
    /// An array where each element represents a range of IP addresses. The rule is triggered if any element matches the target IP address.
    /// <para>The following formats are possible:</para>
    /// <list type="bullet">
    ///     <item>IP address: eg "127.0.0.1".</item>
    ///     <item><see href="https://en.wikipedia.org/wiki/Classless_Inter-Domain_Routing">CIDR</see>: For example, "10.0.0.0/8", can also be used "0.0.0.0/0" "::/0"to specify all IPv4 or IPv6 addresses.</item>
    ///     <item>Predefined IP address list: This list is built into every Xray installation package, the filename is geoip.dat. The format is "geoip:код_страны", must begin with geoip:(in lowercase), followed by a two-letter country code. Almost all countries with internet access are supported.</item>
    ///     <item>Special meaning: "geoip:private", includes all private addresses, such as 127.0.0.1.</item>
    ///     <item>The inversion function !: "geoip:!cn"denotes results not included in geoip:cn. Multiple negated conditions are combined with the logical AND , while positive conditions and the set of all negative conditions are combined with the logical OR . For example, ip: ["geoip:!cn", "geoip:!us", "geoip:telegram"]matches IP addresses that are not in the US AND not in China, OR are Telegram IP addresses.</item>
    ///     <item>Loading IP addresses from a file: has the form "ext:file:tag", must start with ext:(in lowercase) followed by the file name and tag, the file is stored in <see href="https://xtls.github.io/config/features/env.html#%E8%B5%84%E6%BA%90%E6%96%87%E4%BB%B6%E8%B7%AF%E5%BE%84">the resource directory</see>, the file format is the same as geoip.dat, the tag must exist in the file.</item>
    /// </list>
    /// </summary>
    [JsonPropertyName("ip")]
    public List<string>? Ip { get; set; }

    /// <summary>
    /// Destination port range, three formats possible:
    /// <list type="bullet">
    ///     <item>"a-b": a and b are positive integers less than 65536. This range is a closed interval, the rule takes effect if the destination port falls within this range.</item>
    ///     <item>a: a is a positive integer less than 65536. The rule takes effect if the destination port is equal to a.</item>
    ///     <item>A mixture of the two formats above, separated by a comma ",". For example: "53,443,1000-2000".</item>
    /// </list>
    /// </summary>
    [JsonPropertyName("port")]
    public string? Port { get; set; }

    /// <summary>
    /// Source port, three formats are possible:
    /// <list type="bullet">
    ///     <item>"a-b": a and b are positive integers less than 65536. This range is a closed interval, the rule takes effect if the destination port falls within this range.</item>
    ///     <item>a: a is a positive integer less than 65536. The rule takes effect if the destination port is equal to a.</item>
    ///     <item>A mixture of the two formats above, separated by a comma ",". For example: "53,443,1000-2000".</item>
    /// </list>
    /// </summary>
    [JsonPropertyName("sourcePort")]
    public string? SourcePort { get; set; }

    // TODO: make as enum array
    /// <summary>
    /// Valid values: "tcp", "udp", or "tcp,udp". The rule takes effect if the connection type matches the specified one.
    /// </summary>
    [JsonPropertyName("network")]
    public string? Network { get; set; }

    // TODO: add handling alias source
    /// <summary>
    /// An array where each element represents a range of IP addresses. Possible formats include IP address, CIDR, GeoIP, and loading IP addresses from a file. The rule is applied if any element matches the source IP address.
    /// </summary>
    [JsonPropertyName("sourceIP")]
    public List<string>? SourceIP { get; set; }

    /// <summary>
    /// The format is the same as for other IPs. It is used to specify the IP address used by the local host inbound(when listening on all IP addresses, 0.0.0.0 different actual incoming IPs will result in different values localIP).
    /// <para>Doesn't work for UDP (tracking is impossible due to its datagram nature), the IP address that is being listened on will always be visible (listen).</para>
    /// </summary>
    [JsonPropertyName("localIP")]
    public List<string>? LocalIP { get; set; }

    /// <summary>
    /// An array where each element is an email address. The rule is triggered if any element matches the source user.
    /// <para>Similar to the domain name, matching using regular expressions starting with is also supported regexp: (This also needs to be replaced \with \\, see the explanation in the section domain.)</para>
    /// </summary>
    [JsonPropertyName("user")]
    public List<string>? User { get; set; }

    /// <summary>
    /// The incoming VLESS connection allows the client to change the seventh and eighth bytes of the UUID to any values ​​and use them as data vlessRoute. This allows the user to customize some of the server's routing without changing any external fields.
    /// </summary>
    [JsonPropertyName("vlessRoute")]
    public string? VlessRoute { get; set; }

    /// <summary>
    /// An array where each element is a tag. The rule is applied if any element matches the incoming protocol tag.
    /// </summary>
    [JsonPropertyName("inboundTag")]
    public List<string>? InboundTag { get; set; }

    /// <summary>
    /// An array where each element represents a protocol. The rule takes effect if any protocol matches the current connection's protocol type.
    /// </summary>
    [JsonPropertyName("protocol")]
    public List<NetProtocol>? Protocol { get; set; }

    /// <summary>
    /// A JSON object where keys and values ​​are strings. Used to validate HTTP traffic attribute values ​​(for obvious reasons, only 1.0 and 1.1 are supported). The rule is triggered if the HTTP headers contain all the specified keys and the values ​​contain the specified substring. Keys are case-insensitive. Values ​​support regular expressions.
    /// </summary>
    [JsonPropertyName("attrs")]
    public Dictionary<string, string>? Attrs { get; set; }

    /// <summary>
    /// Matches the outgoing channel tag.
    /// </summary>
    [JsonPropertyName("outboundTag")]
    public string? OutboundTag { get; set; }

    /// <summary>
    /// Matches the load balancer tag.
    /// </summary>
    [JsonPropertyName("balancerTag")]
    public string? BalancerTag { get; set; }

    /// <summary>
    /// If the connection comes from the local machine, a process-based match is made. If the connection is not from the local machine, the match is immediately considered unsuccessful. Only Windows and Linux are supported.
    /// </summary>
    [JsonPropertyName("process")]
    public List<string>? Process { get; set; }

    /// <summary>
    /// Optional, has no actual effect, used only to identify the name of this rule.
    /// </summary>
    [JsonPropertyName("ruleTag")]
    public string? RuleTag { get; set; }
}

public class RoutingBalancer
{
    /// <summary>
    /// This load balancer's tag, used to match balancerTag against RuleObject.
    /// </summary>
    [JsonPropertyName("tag")]
    public required string Tag { get; set; }

    /// <summary>
    /// An array of strings, each of which will be used to match an outgoing channel tag prefix. For example, for the following outgoing channel tags: [ "a", "ab", "c", "ba" ], "selector": ["a"] will match [ "a", "ab" ].
    /// </summary>
    [JsonPropertyName("selector")]
    public List<string>? Selector { get; set; }

    /// <summary>
    /// If connection monitoring reveals that all outbound connections are unavailable, the outbound connection specified in this setting is used.
    /// <para>Note: <see href="https://xtls.github.io/config/observatory.html#%E8%BF%9E%E6%8E%A5%E8%A7%82%E6%B5%8B">An observatory</see> or <see href="https://xtls.github.io/config/observatory.html#%E8%BF%9E%E6%8E%A5%E8%A7%82%E6%B5%8B">burstObservatory</see> configuration is required.</para>
    /// </summary>
    [JsonPropertyName("fallbackTag")]
    public string? FallbackTag { get; set; }

    /// <summary>
    /// Balance strategy object
    /// </summary>
    [JsonPropertyName("strategy")]
    public BalancerStrategy? Strategy { get; set; }
}

/// <summary>
/// Balance strategy object
/// </summary>
public class BalancerStrategy
{
    /// <summary>
    /// Default is "random"
    /// </summary>
    [JsonPropertyName("type")]
    public BalancerStrategyType Type { get; set; } = BalancerStrategyType.Random;

    /// <summary>
    /// This is an optional configuration parameter whose format varies for different load balancing strategies.
    /// <para>Currently, this configuration parameter can only be added for the load balancing strategy leastLoad.</para>
    /// </summary>
    [JsonPropertyName("settings")]
    public BalancerStrategySettings? Settings { get; set; }
}

/// <summary>
/// This is an optional configuration parameter whose format varies for different load balancing strategies.
/// </summary>
public class BalancerStrategySettings
{
    /// <summary>
    /// The number of optimal nodes selected by the load balancer. Traffic will be randomly distributed among these nodes.
    /// </summary>
    [JsonPropertyName("expected")]
    public int? Expected { get; set; }

    /// <summary>
    /// Maximum allowed RTT (latency) time when measuring speed.
    /// </summary>
    [JsonPropertyName("maxRTT")]
    public string? MaxRTT { get; set; }

    /// <summary>
    /// Maximum allowed percentage of failed speed measurements, eg 0.01 means that 1% of failed measurements are allowed (apparently not implemented).
    /// </summary>
    [JsonPropertyName("tolerance")]
    public float? Tolerance { get; set; }

    /// <summary>
    /// Maximum acceptable standard deviation of RTT time when measuring speed.
    /// </summary>
    [JsonPropertyName("baselines")]
    public List<string>? Baselines { get; set; }

    /// <summary>
    /// An optional configuration parameter, an array, that allows you to specify weights for all outgoing connections.
    /// </summary>
    [JsonPropertyName("costs")]
    public List<Coast>? Costs { get; set; }

    /// <summary>
    /// The object allows you to specify weights for all outgoing connections.
    /// </summary>
    public class Coast
    {
        /// <summary>
        /// Whether to use regular expressions to select Tag outgoing connections.
        /// </summary>
        [JsonPropertyName("regexp")]
        public bool? Regexp { get; set; }

        /// <summary>
        /// Tag Outgoing connection matching.
        /// </summary>
        [JsonPropertyName("match")]
        public string? Match { get; set; }

        /// <summary>
        /// Weight value. The higher the value, the less likely the corresponding node is to be selected.
        /// </summary>
        [JsonPropertyName("value")]
        public float? Value { get; set; }
    }
}