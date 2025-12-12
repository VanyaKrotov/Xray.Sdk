using Xray.Api.Enums;

namespace Xray.Api.Models;

public class BaseAddUser
{
    public required string Email { get; set; }

    public uint Level { get; set; }

    public required string Tag { get; set; }
}

public class AddVlessUser : BaseAddUser
{
    public required string Uuid { get; set; }

    public string Flow { get; set; } = "xtls-rprx-vision";

}

public class AddTrojanUser : BaseAddUser
{
    public string? Password { get; set; }
}

public class AddShadowsocksUser : BaseAddUser
{
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