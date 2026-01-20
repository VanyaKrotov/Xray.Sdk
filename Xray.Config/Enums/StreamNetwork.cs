using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Xray.Config.Utilities;

namespace Xray.Config.Enums;

[JsonConverter(typeof(EnumMemberConverter<StreamNetwork>))]
public enum StreamNetwork
{
    [EnumMember(Value = "raw")]
    Raw,

    [EnumMember(Value = "xhttp")]
    XHttp,

    [EnumMember(Value = "kcp")]
    Kcp,

    [EnumMember(Value = "grpc")]
    Grpc,

    [EnumMember(Value = "ws")]
    Ws,

    [EnumMember(Value = "httpupgrade")]
    HttpUpgrade,
    
    [EnumMember(Value = "hysteria")]
    Hysteria
}