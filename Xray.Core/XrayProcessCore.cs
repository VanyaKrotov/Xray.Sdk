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

    public async Task StartAsync(XrayConfig config)
    {
        if (IsStarted())
        {
            throw new Exception("Xray already started");
        }

        var process = CreateProcess("run", "-config", "stdin:");

        await process.StandardInput.WriteAsync(config.ToJson());
        await process.StandardInput.FlushAsync();

        process.StandardInput.Close();

        if (process.HasExited)
        {
            var result = await Task.WhenAll(process.StandardError.ReadToEndAsync(), process.StandardOutput.ReadToEndAsync());

            await process.WaitForExitAsync();

            throw new Exception($"Failure xray start operation. Output: {result[0] ?? result[1] ?? "empty"}");
        }

        _coreProcess = process;
    }

    public async Task StopAsync()
    {
        if (!IsStarted())
        {
            return;
        }

        _coreProcess!.Kill();

        await _coreProcess.WaitForExitAsync();

        _coreProcess = null;
    }

    public void Start(XrayConfig config)
    {
        if (IsStarted())
        {
            throw new Exception("Xray already started");
        }

        var process = CreateProcess("run", "-config", "stdin:");

        process.StandardInput.Write(config.ToJson());
        process.StandardInput.Flush();
        process.StandardInput.Close();

        if (process.HasExited)
        {
            var error = process.StandardError.ReadToEnd();
            var outout = process.StandardOutput.ReadToEnd();

            process.WaitForExit();

            throw new Exception($"Failure xray start operation. Output: {error ?? outout ?? "empty"}");
        }

        _coreProcess = process;
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

    public void Dispose()
    {
        if (IsStarted())
        {
            Stop();
        }

        _coreProcess?.Dispose();
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
}

public class XrayProcessOptions
{
    public required string WorkingDirectory { get; set; }

    public string ProcessName { get; set; } = "xray";
}