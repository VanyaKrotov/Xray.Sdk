using Xray.App.Stats.Command;
using Xray.Api.Models;

namespace Xray.Api.Mappers;

public static class StatsMapper
{
    public static SysStats MapSysStats(SysStatsResponse data)
    {
        return new SysStats()
        {
            Alloc = data.Alloc,
            Frees = data.Frees,
            LiveObjects = data.LiveObjects,
            Mallocs = data.Mallocs,
            NumGC = data.NumGC,
            NumGoroutine = data.NumGoroutine,
            PauseTotalNs = data.PauseTotalNs,
            Sys = data.Sys,
            TotalAlloc = data.TotalAlloc,
            Uptime = data.Uptime
        };
    }

    public static List<UserStats> MapUserStats(QueryStatsResponse response)
    {
        var usersMap = new Dictionary<string, UserStats>();

        foreach (var stat in response.Stat)
        {
            var nameParts = stat.Name.Split(">>>", StringSplitOptions.RemoveEmptyEntries);
            if (nameParts.Length < 4) continue;

            var email = nameParts[1];
            var type = nameParts[3];
            var value = stat.Value;

            if (!usersMap.TryGetValue(email, out var user))
            {
                user = new UserStats() { Email = email, Uplink = 0, Downlink = 0 };
                usersMap[email] = user;
            }

            if (type == "uplink")
            {
                user.Uplink += value;
            }
            else if (type == "downlink")
            {
                user.Downlink += value;
            }
        }

        return usersMap.Values.ToList();
    }
}