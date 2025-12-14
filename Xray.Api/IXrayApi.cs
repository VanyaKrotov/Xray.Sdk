using Xray.Api.Models;

namespace Xray.Api.Service;

public interface IXrayApi
{
    Task<List<BaseUser>> GetInboundUsers(GetUsersOptions options);

    Task RemoveUser(string tag, string email);

    Task<long> GetInboundUsersCount(string tag);

    Task AddVlessUser(AddVlessUser values);

    Task AddTrojanUser(AddTrojanUser values);

    Task AddShadowsocksUser(AddShadowsocksUser values);

    Task AddShadowsocks2022User(AddShadowsocks2022User values);

    Task AddSocksUser(AddSocksUser values);

    Task AddHttpUser(AddHttpUser values);

    //

    Task<SysStats> GetSysStats();

    Task<List<UserStats>> GetAllUsersStats(bool reset = false);

    Task<UserStats> GetUserStats(string email, bool reset = false);

    Task<bool> GetUserOnlineStatus(string email);
}