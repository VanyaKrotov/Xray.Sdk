using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Xray.Config.Utilities;

namespace Xray.Config.Enums;

[JsonConverter(typeof(EnumMemberConverter<NetProtocol>))]
public enum NetProtocol
{
    [EnumMember(Value = "http")]
    Http,

    [EnumMember(Value = "tls")]
    Tls,

    [EnumMember(Value = "bittorrent")]
    Bittorrent
}
