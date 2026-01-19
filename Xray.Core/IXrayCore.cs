using Xray.Config.Models;

namespace Xray.Core;

public interface IXrayCore : IDisposable
{
    /// <summary>
    /// Start the xray-core server by config
    /// </summary>
    /// <param name="guid">The xray config</param>
    public void Start(XrayConfig config);

    /// <summary>
    /// Stop the xray-core server
    /// </summary>
    public void Stop();

    /// <summary>
    /// Check if the xray-core server instance
    /// </summary>
    /// <returns>True if the server is running</returns>
    public bool IsStarted();

    /// <summary>
    /// Get the xray core version
    /// </summary>
    /// <returns>Xray core version</returns>
    public string Version();
}