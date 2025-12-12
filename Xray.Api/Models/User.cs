using Xray.Proxy.Shadowsocks;

namespace Xray.Api.Models;

public class BaseUser
{
    public required string Email { get; set; }

    public uint Level { get; set; }
}

public class VlessUser : BaseUser
{
    public required string Id { get; set; }

    public string? Flow { get; set; }

    public string? Encryption { get; set; }

    public uint XorMode { get; set; }

    public uint Seconds { get; set; }

    public string? Padding { get; set; }

    public Reverse? Reverse { get; set; }
}

public class TrojanUser : BaseUser
{
    public required string Password { get; set; }
}

public class ShadowSocksUser : BaseUser
{
    public required string Password { get; set; }

    public CipherType CipherType { get; set; }

    public bool IvCheck { get; set; }
}

public class ShadowSocks2022User : BaseUser
{
    public required string Key { get; set; }
}

public class SocksUser : BaseUser
{
    public string? Username { get; set; }

    public string? Password { get; set; }
}

public class HttpUser : BaseUser
{
    public string? Username { get; set; }

    public string? Password { get; set; }
}

public class Reverse
{
    public required string Tag { get; set; }
}