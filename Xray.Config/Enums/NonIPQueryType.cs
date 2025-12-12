using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Xray.Config.Utilities;

namespace Xray.Config.Enums;

[JsonConverter(typeof(EnumMemberConverter<NonIPQueryType>))]
public enum NonIPQueryType
{
    [EnumMember(Value = "drop")]
    Drop,

    [EnumMember(Value = "k")]
    Skip,

    [EnumMember(Value = "reject")]
    Reject
}