using Xray.Config.Models;

namespace Xray.Core;

public interface IXrayCore : IDisposable
{
    public void Start(XrayConfig config);

    public void Stop();

    public bool IsStarted();

    public string Version();
}