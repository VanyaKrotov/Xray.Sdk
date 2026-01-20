using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Xray.Config.Utilities;

namespace Xray.Config.Enums;

[JsonConverter(typeof(EnumMemberConverter<OperationSystem>))]
public enum OperationSystem
{
    [EnumMember(Value = "linux")]
    Linux,

    [EnumMember(Value = "windows")]
    Windows,

    [EnumMember(Value = "darwin")]
    Darwin,
}