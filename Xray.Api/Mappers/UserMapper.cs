using Xray.Api.Models;

namespace Xray.Api.Mappers;

public static class UserMapper
{
    private readonly static Dictionary<string, Func<Common.Protocol.User, BaseUser>> _userDecoders = new Dictionary<string, Func<Common.Protocol.User, BaseUser>>()
    {
        {Proxy.Trojan.Account.Descriptor.FullName, (user) =>
            {
                var account = Proxy.Trojan.Account.Parser.ParseFrom(user.Account.Value);

                return new TrojanUser()
                {
                    Email = user.Email,
                    Level = user.Level,
                    Password = account.Password
                };
            }
        },
        {Proxy.Vless.Account.Descriptor.FullName, (user) =>
            {
                var account = Proxy.Vless.Account.Parser.ParseFrom(user.Account.Value);

                return new VlessUser()
                {
                    Email = user.Email,
                    Level = user.Level,
                    Id = account.Id,
                    Encryption = account.Encryption,
                    Flow = account.Flow,
                    Padding = account.Padding,
                    Reverse = account.Reverse == null ? null : new Reverse() { Tag = account.Reverse.Tag },
                    Seconds = account.Seconds,
                    XorMode = account.XorMode,
                };
            }
        },
        {Proxy.Shadowsocks.Account.Descriptor.FullName, (user) =>
            {
                var account = Proxy.Shadowsocks.Account.Parser.ParseFrom(user.Account.Value);

                return new ShadowSocksUser()
                {
                    Email = user.Email,
                    Level = user.Level,
                    Password = account.Password,
                    CipherType = account.CipherType,
                    IvCheck = account.IvCheck,
                };
            }
        },
        {Proxy.Shadowsocks2022.Account.Descriptor.FullName, (user) =>
            {
                var account = Proxy.Shadowsocks2022.Account.Parser.ParseFrom(user.Account.Value);

                return new ShadowSocks2022User()
                {
                    Email = user.Email,
                    Level = user.Level,
                    Key = account.Key,
                };
            }
        },
        {Proxy.Socks.Account.Descriptor.FullName, (user) =>
            {
                var account = Proxy.Socks.Account.Parser.ParseFrom(user.Account.Value);

                return new SocksUser()
                {
                    Email = user.Email,
                    Level = user.Level,
                    Password = account.Password,
                    Username = account.Username,
                };
            }
        },
        {Proxy.Http.Account.Descriptor.FullName,(user) =>
            {
                var account = Proxy.Http.Account.Parser.ParseFrom(user.Account.Value);

                return new HttpUser()
                {
                    Email = user.Email,
                    Level = user.Level,
                    Password = account.Password,
                    Username = account.Username,
                };
            }
        },
    };

    public static BaseUser Decode(Common.Protocol.User user)
    {
        if (!_userDecoders.ContainsKey(user.Account.Type))
        {
            throw new Exception("Unsupported user type");
        }

        return _userDecoders[user.Account.Type](user);
    }
}