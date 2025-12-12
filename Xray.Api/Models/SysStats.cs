namespace Xray.Api.Models;

public class SysStats
{
    public ulong NumGoroutine { get; set; }

    public ulong NumGC { get; set; }

    public ulong Alloc { get; set; }

    public ulong TotalAlloc { get; set; }

    public ulong Sys { get; set; }

    public ulong Mallocs { get; set; }

    public ulong Frees { get; set; }

    public ulong LiveObjects { get; set; }

    public ulong PauseTotalNs { get; set; }

    public ulong Uptime { get; set; }
}