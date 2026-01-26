using Xray.Config.Utilities;

namespace Xray.Config.Enums;

public enum SocksAuth
{
    [EnumProperty("noauth")]
    NoAuth,

    [EnumProperty("password")]
    Password,
}