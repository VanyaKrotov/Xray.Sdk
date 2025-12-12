using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Xray.Config.Utilities;

namespace Xray.Config.Enums;

[JsonConverter(typeof(EnumMemberConverter<AllocateStrategy>))]
public enum AllocateStrategy
{
    [EnumMember(Value = "always")]
    Always,

    [EnumMember(Value = "random")]
    Random
}