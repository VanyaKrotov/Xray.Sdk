namespace Xray.Core.Models;

public class TlsCerts
{
    public List<string> Certificate { get; set; } = new();
    public List<string> Key { get; set; } = new();
}
