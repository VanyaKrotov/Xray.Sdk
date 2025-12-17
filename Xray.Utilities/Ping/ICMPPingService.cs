using System.Net.NetworkInformation;
using System.Text;

namespace Xray.Utilities.Ping;

public class ICMPPingService
{
    private readonly Ping _ping;
    private readonly byte[] _buffer;
    private readonly PingOptions _options;

    public ICMPPingService(int ttl = 64, int bufferSize = 32)
    {
        _ping = new Ping();
        _buffer = Encoding.ASCII.GetBytes(new string('a', bufferSize));
        _options = new PingOptions(ttl, dontFragment: true);
    }

    /// <summary>
    /// Выполняет один ICMP‑пинг и возвращает результат.
    /// </summary>
    /// <param name="host">IP или домен</param>
    /// <param name="timeout">Таймаут в миллисекундах</param>
    /// <returns>Объект PingReply с информацией о времени отклика</returns>
    public async Task<PingReply> PingAsync(string host, int timeout = 3000)
    {
        return await _ping.SendPingAsync(host, timeout, _buffer, _options);
    }

    /// <summary>
    /// Проверяет несколько раз и возвращает статистику.
    /// </summary>
    public async Task<PingStatistics> PingMultipleAsync(string host, int count = 4, int timeout = 3000, int intervalMs = 1000)
    {
        int sent = 0, received = 0;
        long min = long.MaxValue, max = 0, sum = 0;

        for (int i = 0; i < count; i++)
        {
            sent++;
            var reply = await PingAsync(host, timeout);

            if (reply.Status == IPStatus.Success)
            {
                received++;
                long time = reply.RoundtripTime;
                min = Math.Min(min, time);
                max = Math.Max(max, time);
                sum += time;
            }

            await Task.Delay(intervalMs);
        }

        return new PingStatistics(sent, received, min, max, received > 0 ? sum / received : -1);
    }
}

public record PingStatistics(int Sent, int Received, long Min, long Max, long Avg);
