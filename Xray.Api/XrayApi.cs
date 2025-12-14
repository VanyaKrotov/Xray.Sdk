using Google.Protobuf;
using Grpc.Net.Client;
using Xray.App.Log.Command;
using Xray.App.Proxyman.Command;
using Xray.App.Stats.Command;
using Xray.Common.Protocol;
using Xray.Common.Serial;
using Xray.Api.Mappers;
using Xray.Api.Models;

namespace Xray.Api.Service;

public class XrayApi : IXrayApi
{
    private readonly HandlerService.HandlerServiceClient _handler;

    private readonly LoggerService.LoggerServiceClient _logger;

    private readonly StatsService.StatsServiceClient _stats;

    public XrayApi(string host)
    {
        var channel = GrpcChannel.ForAddress(host);

        _handler = new HandlerService.HandlerServiceClient(channel);
        _logger = new LoggerService.LoggerServiceClient(channel);
        _stats = new StatsService.StatsServiceClient(channel);
    }

    public async Task<long> GetInboundUsersCount(string tag)
    {
        var response = await _handler.GetInboundUsersCountAsync(new GetInboundUserRequest() { Tag = tag, });

        return response.Count;
    }

    public async Task<List<BaseUser>> GetInboundUsers(GetUsersOptions options)
    {
        var response = await _handler.GetInboundUsersAsync(new GetInboundUserRequest() { Tag = "default", });

        return response.Users.Select(x => UserMapper.Decode(x)).ToList();
    }

    public async Task RemoveUser(string tag, string email)
    {
        await _handler.AlterInboundAsync(new AlterInboundRequest()
        {
            Operation = new TypedMessage
            {
                Type = RemoveUserOperation.Descriptor.FullName,
                Value = new RemoveUserOperation() { Email = email }.ToByteString()
            },
            Tag = tag
        });
    }

    private AlterInboundRequest CreateAddUserOperation(BaseAddUser model, TypedMessage account)
    {
        return new AlterInboundRequest()
        {
            Tag = model.Tag,
            Operation = new TypedMessage()
            {
                Type = AddUserOperation.Descriptor.FullName,
                Value = new AddUserOperation()
                {
                    User = new User()
                    {
                        Email = model.Email,
                        Level = model.Level,
                        Account = account
                    }
                }.ToByteString()
            }
        };
    }

    public async Task AddVlessUser(AddVlessUser values)
    {
        await _handler.AlterInboundAsync(CreateAddUserOperation(values, new TypedMessage()
        {
            Type = Xray.Proxy.Vless.Account.Descriptor.FullName,
            Value = new Xray.Proxy.Vless.Account()
            {
                Id = values.Uuid,
                Flow = values.Flow,
                Encryption = "none",
            }.ToByteString()
        }));
    }

    public async Task AddTrojanUser(AddTrojanUser values)
    {
        await _handler.AlterInboundAsync(CreateAddUserOperation(values, new TypedMessage()
        {
            Type = Xray.Proxy.Trojan.Account.Descriptor.FullName,
            Value = new Xray.Proxy.Trojan.Account()
            {
                Password = values.Password
            }.ToByteString()
        }));
    }

    public async Task AddShadowsocksUser(AddShadowsocksUser values)
    {
        await _handler.AlterInboundAsync(CreateAddUserOperation(values, new TypedMessage()
        {
            Type = Xray.Proxy.Shadowsocks.Account.Descriptor.FullName,
            Value = new Xray.Proxy.Shadowsocks.Account()
            {
                Password = values.Password,
                CipherType = (Xray.Proxy.Shadowsocks.CipherType)values.CipherType,
                IvCheck = values.IvCheck
            }.ToByteString()
        }));
    }

    public async Task AddShadowsocks2022User(AddShadowsocks2022User values)
    {
        await _handler.AlterInboundAsync(CreateAddUserOperation(values, new TypedMessage()
        {
            Type = Xray.Proxy.Shadowsocks2022.Account.Descriptor.FullName,
            Value = new Xray.Proxy.Shadowsocks2022.Account()
            {
                Key = values.Key
            }.ToByteString()
        }));
    }

    public async Task AddSocksUser(AddSocksUser values)
    {
        await _handler.AlterInboundAsync(CreateAddUserOperation(values, new TypedMessage()
        {
            Type = Xray.Proxy.Socks.Account.Descriptor.FullName,
            Value = new Xray.Proxy.Socks.Account()
            {
                Password = values.Password,
                Username = values.Username,
            }.ToByteString()
        }));
    }

    public async Task AddHttpUser(AddHttpUser values)
    {
        await _handler.AlterInboundAsync(CreateAddUserOperation(values, new TypedMessage()
        {
            Type = Xray.Proxy.Http.Account.Descriptor.FullName,
            Value = new Xray.Proxy.Http.Account()
            {
                Password = values.Password,
                Username = values.Username,
            }.ToByteString()
        }));
    }

    //

    public async Task<SysStats> GetSysStats()
    {
        var response = await _stats.GetSysStatsAsync(new SysStatsRequest());

        return StatsMapper.MapSysStats(response);
    }

    public async Task<List<UserStats>> GetAllUsersStats(bool reset = false)
    {
        var response = await _stats.QueryStatsAsync(new QueryStatsRequest()
        {
            Pattern = "user>>>",
            Reset = reset
        });

        return StatsMapper.MapUserStats(response);
    }

    public async Task<UserStats> GetUserStats(string email, bool reset = false)
    {
        var response = await _stats.QueryStatsAsync(new QueryStatsRequest()
        {
            Pattern = $"user>>>{email}>>>",
            Reset = reset
        });

        return StatsMapper.MapUserStats(response).First();
    }

    public async Task<bool> GetUserOnlineStatus(string email)
    {
        try
        {
            var response = await _stats.GetStatsOnlineAsync(new GetStatsRequest()
            {
                Name = $"user>>>{email}>>>online",
            });

            return true;
        }
        catch (Exception exc)
        {
            if (exc.Message.Contains("online not found."))
            {
                return false;
            }

            throw;
        }
    }
}
