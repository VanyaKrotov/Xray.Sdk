using Xray.Config.Models;
using Xray.Core.Exceptions;
using Xray.Core.Wrappers;

namespace Xray.Core;

public class XrayLibCore : IXrayLibCore
{
    private readonly Guid _guid = Guid.NewGuid();

    private readonly NativeWrapper _lib = new NativeWrapper();

    public string Version() => _lib.GetVersion();

    public void Start(XrayConfig config)
    {
        HandleResponse(_lib.Start(_guid, config.ToJson()));
    }

    public void Stop()
    {
        HandleResponse(_lib.Stop(_guid));
    }

    public bool IsStarted() => _lib.IsStarted(_guid);


    public void Dispose()
    {
        _lib.Dispose();
    }

    private void HandleResponse(NativeWrapper.TypedResponse response)
    {
        if (response.IsSuccess)
        {
            return;
        }

        throw new CoreException(response.Message, response.Code);
    }
}
