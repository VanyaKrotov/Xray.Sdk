using Xray.Config.Models;

namespace Xray.Core;

public interface IXrayProcessCore : IXrayCore
{
    public Task StartAsync(XrayConfig config);

    public Task StopAsync();
}