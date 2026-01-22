using Xray.Api.Enums;
using Xray.Config.Enums;

namespace Xray.Api.Models;

public class BaseAddUser
{
    public required string Email { get; set; }

    public uint Level { get; set; }

    public required string Tag { get; set; }
}

public class AddVlessUser : BaseAddUser
{   
    /// <summary>
    /// 
    /// </summary>
    public required string Id { get; set; }

    /// <summary>
    /// Flow control mode, used to select the XTLS algorithm.
    /// </summary>
    public XtlsFlow Flow { get; set; } = XtlsFlow.XtlsRprxVision;

}

public class AddTrojanUser : BaseAddUser
{   
    /// <summary>
    /// Password of user
    /// </summary>
    public string? Password { get; set; }
}

public class AddShadowsocksUser : BaseAddUser
{   
    /// <summary>
    /// Required for Shadowsocks 2022. A pre-shared key similar to WireGuard is used as the password.
    /// </summary>
    public string? Password { get; set; }
    
    public CipherType CipherType { get; set; }

    public bool IvCheck { get; set; }
}

public class AddShadowsocks2022User : BaseAddUser
{
    public string? Key { get; set; }
}

public class AddSocksUser : BaseAddUser
{   
    public string? Username { get; set; }

    public string? Password { get; set; }
}

public class AddHttpUser : BaseAddUser
{
    public string? Username { get; set; }

    public string? Password { get; set; }
}