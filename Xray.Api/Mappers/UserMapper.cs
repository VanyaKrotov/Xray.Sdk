using Xray.Api.Models;

namespace Xray.Api.Mappers;

public static class UserMapper
{
    private readonly static Dictionary<string, Func<Xray.Common.Protocol.User, BaseUser>> _userDecoders = new Dictionary<string, Func<Xray.Common.Protocol.User, BaseUser>>()
    {
        {Xray.Proxy.Trojan.Account.Descriptor.FullName, (user) =>
            {
                var account = Xray.Proxy.Trojan.Account.Parser.ParseFrom(user.Account.Value);

                return new TrojanUser()
                {
                    Email = user.Email,
                    Level = user.Level,
                    Password = account.Password
                };
            }
        },
        {Xray.Proxy.Vless.Account.Descriptor.FullName, (user) =>
            {
                var account = Xray.Proxy.Vless.Account.Parser.ParseFrom(user.Account.Value);

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
        {Xray.Proxy.Shadowsocks.Account.Descriptor.FullName, (user) =>
            {
                var account = Xray.Proxy.Shadowsocks.Account.Parser.ParseFrom(user.Account.Value);

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
        {Xray.Proxy.Shadowsocks2022.Account.Descriptor.FullName, (user) =>
            {
                var account = Xray.Proxy.Shadowsocks2022.Account.Parser.ParseFrom(user.Account.Value);

                return new ShadowSocks2022User()
                {
                    Email = user.Email,
                    Level = user.Level,
                    Key = account.Key,
                };
            }
        },
        {Xray.Proxy.Socks.Account.Descriptor.FullName, (user) =>
            {
                var account = Xray.Proxy.Socks.Account.Parser.ParseFrom(user.Account.Value);

                return new SocksUser()
                {
                    Email = user.Email,
                    Level = user.Level,
                    Password = account.Password,
                    Username = account.Username,
                };
            }
        },
        {Xray.Proxy.Http.Account.Descriptor.FullName,(user) =>
            {
                var account = Xray.Proxy.Http.Account.Parser.ParseFrom(user.Account.Value);

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

    public static BaseUser Decode(Xray.Common.Protocol.User user)
    {
        if (!_userDecoders.ContainsKey(user.Account.Type))
        {
            throw new Exception("Unsupported user type");
        }

        return _userDecoders[user.Account.Type](user);
    }
}