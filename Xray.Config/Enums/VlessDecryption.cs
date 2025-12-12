using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Xray.Config.Utilities;

namespace Xray.Config.Enums;

[JsonConverter(typeof(EnumMemberConverter<VlessDecryption>))]
public enum VlessDecryption
{
    [EnumMember(Value = "none")]
    None
}