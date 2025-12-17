using Xray.Config.Models;

namespace Xray.Core;

public interface IXrayCore : IDisposable
{
    public bool TryStart(XrayConfig config);

    public void Start(XrayConfig config);

    public void Stop();

    public bool IsStarted();

    public string Version();
}