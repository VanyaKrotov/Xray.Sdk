namespace Xray.Api.Models;

/// <summary>
/// Represents system-level runtime statistics returned by the Xray-core GetSysStats API.
/// </summary>
public class SysStats
{
    /// <summary>
    /// Number of currently active goroutines in the Xray process.
    /// Indicates concurrency level and internal workload.
    /// </summary>
    public ulong NumGoroutine { get; set; }

    /// <summary>
    /// Total number of completed garbage collection cycles since process start.
    /// Useful for analyzing memory churn and GC frequency.
    /// </summary>
    public ulong NumGC { get; set; }

    /// <summary>
    /// Amount of heap memory (in bytes) currently allocated and still in use.
    /// Represents the live heap size.
    /// </summary>
    public ulong Alloc { get; set; }

    /// <summary>
    /// Total number of bytes allocated during the lifetime of the process.
    /// This value only increases and never decreases.
    /// </summary>
    public ulong TotalAlloc { get; set; }

    /// <summary>
    /// Total amount of memory (in bytes) obtained from the operating system.
    /// Includes heap, stacks, caches, and runtime structures.
    /// </summary>
    public ulong Sys { get; set; }

    /// <summary>
    /// Total number of heap allocation operations performed since startup.
    /// </summary>
    public ulong Mallocs { get; set; }

    /// <summary>
    /// Total number of freed heap objects since startup.
    /// </summary>
    public ulong Frees { get; set; }

    /// <summary>
    /// Number of currently allocated heap objects.
    /// Calculated as Mallocs - Frees.
    /// </summary>
    public ulong LiveObjects { get; set; }

    /// <summary>
    /// Total time (in nanoseconds) spent in GC stop-the-world pauses.
    /// Higher values indicate heavier garbage collection activity.
    /// </summary>
    public ulong PauseTotalNs { get; set; }

    /// <summary>
    /// Total uptime of the Xray process in seconds.
    /// </summary>
    public ulong Uptime { get; set; }
}
