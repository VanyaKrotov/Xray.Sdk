namespace Xray.Core.Exceptions;

public class CoreException : Exception
{
    public int Code { get; set; }

    public CoreException(string message, int code) : base(message)
    {
        Code = code;
    }
}