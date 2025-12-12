using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Xray.Config.Utilities;

namespace Xray.Config.Enums;

[JsonConverter(typeof(EnumMemberConverter<RoutingRuleType>))]
public enum RoutingRuleType
{
    [EnumMember(Value = "field")]

    Field,
}
