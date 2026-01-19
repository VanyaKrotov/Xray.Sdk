namespace Xray.Core.Exceptions;

public class CoreException : Exception
{
    public int Code { get; set; }

    public CoreException(string message, int code = 1) : base(message)
    {
        Code = code;
    }
}