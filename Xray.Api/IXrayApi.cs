using Xray.Api.Models;

namespace Xray.Api.Service;

public interface IXrayApi
{
    /// <summary>
    /// Returns the list of users for an inbound.
    /// </summary>
    /// <param name="options">Parameter is intended to select tag/filters. The implementation defaults to tag "default"</param>
    /// <returns>List of inbound users</returns>
    Task<List<BaseUser>> GetInboundUsers(GetUsersOptions options);

    /// <summary>
    /// Removes a user identified by email from the inbound identified by tag. Implementation issues an AlterInbound request with a RemoveUserOperation message.
    /// </summary>
    /// <param name="tag">Inbound tag</param>
    /// <param name="email">User email</param>
    Task RemoveUser(string tag, string email);

    /// <summary>
    /// Returns the user count for the inbound with the given tag. Calls the handler's GetInboundUsersCount RPC and returns response.Count.
    /// </summary>
    /// <param name="tag">Inbound tag</param>
    /// <returns>Count of inbound users</returns>
    Task<long> GetInboundUsersCount(string tag);

    /// <summary>
    /// Adds a VLESS user using provided values (email, uuid, level, flow, etc.). Implementation builds a typed AddUserOperation with a VLESS account payload and calls AlterInbound.
    /// </summary>
    /// <param name="user">User model for creation</param>
    Task AddVlessUser(AddVlessUser user);

    /// <summary>
    /// Adds a Trojan user. Builds AddUserOperation with Trojan account payload (password) and calls AlterInbound.
    /// </summary>
    /// <param name="user">User model for creation</param>
    Task AddTrojanUser(AddTrojanUser user);

    /// <summary>
    /// Adds a Shadowsocks user with cipher, password and optional IV-check. Translates SDK CipherType to proto CipherType and sends AlterInbound.
    /// </summary>
    /// <param name="user">User model for creation</param>
    Task AddShadowsocksUser(AddShadowsocksUser user);

    /// <summary>
    /// Adds a Shadowsocks2022 user using the provided Key.
    /// </summary>
    /// <param name="user">User model for creation</param>
    Task AddShadowsocks2022User(AddShadowsocks2022User user);

    /// <summary>
    /// Adds a SOCKS user (username/password) via AlterInbound.
    /// </summary>
    /// <param name="user">User model for creation</param>
    Task AddSocksUser(AddSocksUser user);

    /// <summary>
    /// Adds an HTTP proxy user (username/password) via AlterInbound.
    /// </summary>
    /// <param name="user">User model for creation</param>
    Task AddHttpUser(AddHttpUser user);

    //

    /// <summary>
    /// Retrieves system-level statistics through the Stats service and maps the response to the SDK SysStats model using StatsMapper.
    /// </summary>
    /// <returns>Stats information</returns>
    Task<SysStats> GetSysStats();

    /// <summary>
    /// Queries stats with pattern "user>>>" to collect per-user stats.
    /// </summary>
    /// <param name="reset">Will clear counters if true</param>
    /// <returns>Result user stats</returns>
    Task<List<UserStats>> GetAllUsersStats(bool reset = false);

    /// <summary>
    /// Queries stats for a single user by email (pattern "user>>>{email}>>>").
    /// </summary>
    /// <param name="email">Email of user</param>
    /// <param name="reset">Will clear counters if true</param>
    /// <returns>Result user stats</returns>
    Task<UserStats> GetUserStats(string email, bool reset = false);

    /// <summary>
    /// Checks if a user's online stat exists by calling GetStatsOnline.
    /// <para>If RPC fails with message containing "online not found." returns</para>
    /// </summary>
    /// <param name="email">Email of user</param>
    /// <returns>true if the user is online</returns>
    Task<bool> GetUserOnlineStatus(string email);

    /// <summary>
    /// Get all online users exists by calling GetAllOnlineUsers.
    /// </summary>
    /// <returns>List of online users</returns>
    Task<ICollection<string>> GetAllOnlineUsers();
}