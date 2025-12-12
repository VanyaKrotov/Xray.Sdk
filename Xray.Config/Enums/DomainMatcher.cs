using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Xray.Config.Utilities;

namespace Xray.Config.Enums;

[JsonConverter(typeof(EnumMemberConverter<DomainMatcher>))]
public enum DomainMatcher
{
    [EnumMember(Value = "hybrid")]
    Hybrid,

    [EnumMember(Value = "linear")]
    Linear
}