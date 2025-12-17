using System.Diagnostics;
using System.Text.RegularExpressions;
using Xray.Config.Models;

namespace Xray.Core;

public class XrayProcessCore : IXrayProcessCore
{
    private Process? _coreProcess;

    private readonly XrayProcessOptions _options;

    public XrayProcessCore(XrayProcessOptions options)
    {
        _options = options;
    }

    private string _processPath => Path.Combine(_options.WorkingDirectory, _options.ProcessName);

    public string Version()
    {
        var output = RunCommand("-version");
        var match = Regex.Match(output, @"(?<version>\d+\.\d+\.\d+)");
        if (match.Success)
        {
            return match.Groups["version"].Value;
        }

        return "unknown";
    }

    public void Start(XrayConfig config)
    {
        if (IsStarted())
        {
            throw new Exception("Xray already started");
        }

        var process = StartCoreProcess(config);
        if (process.HasExited)
        {
            var error = process.StandardError.ReadToEnd();
            var outout = process.StandardOutput.ReadToEnd();

            process.WaitForExit();

            throw new Exception($"Failure xray start operation. Output: {error ?? outout ?? "empty"}");
        }

        _coreProcess = process;
    }

    public bool TryStart(XrayConfig config)
    {
        if (IsStarted())
        {
            return false;
        }

        var process = StartCoreProcess(config);
        if (process.HasExited)
        {
            return false;
        }

        _coreProcess = process;

        return true;
    }

    public bool IsStarted()
    {
        return _coreProcess != null && !_coreProcess.HasExited;
    }

    public void Stop()
    {
        if (!IsStarted())
        {
            return;
        }

        _coreProcess!.Kill();
        _coreProcess.WaitForExit();
        _coreProcess = null;
    }

    private Process StartCoreProcess(XrayConfig config)
    {
        var process = CreateProcess("run", "-config", "stdin:");

        process.StandardInput.Write(config.ToJson());
        process.StandardInput.Flush();
        process.StandardInput.Close();

        return process;
    }

    private string RunCommand(params string[] args)
    {
        var process = CreateProcess(args);

        string output = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();

        process.WaitForExit();

        if (!string.IsNullOrEmpty(error))
        {
            throw new Exception(error);
        }

        return output;
    }

    private Process CreateProcess(params string[] args)
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = _processPath,
                Arguments = string.Join(" ", args),
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        process.Start();

        return process;
    }

    public void Dispose()
    {
       if(IsStarted())
        {
            Stop();
        }

        _coreProcess?.Dispose();
    }
}

public class XrayProcessOptions
{
    public required string WorkingDirectory { get; set; }

    public string ProcessName { get; set; } = "xray";
}