using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Xray.Config.Utilities;

namespace Xray.Config.Enums;

[JsonConverter(typeof(EnumMemberConverter<TransportProtocol>))]
public enum TransportProtocol
{
    [EnumMember(Value = "tcp")]
    Tcp,

    [EnumMember(Value = "udp")]
    Udp
}