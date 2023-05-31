using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ConcurrentProgramming.Data.Logger.LogWriter;

namespace ConcurrentProgramming.Data.Logger;

public class Logger : ILogger
{
    private readonly ConcurrentQueue<LogEntry> _logs = new();
    private readonly List<ILogWriter> _loggerWriters = new();
    private static ILogger? _instance;
    private static readonly object LoggerLock = new();
    private Timer logTimer = null!;

    public static ILogger GetLogger()
    {
        // ReSharper disable once InvertIf
        if (_instance is null)
        {
            lock (LoggerLock)
            {
                // ReSharper disable once InvertIf
                if (_instance is null)
                {
                    _instance = new Logger();
                    ((Logger)_instance).Start();
                }
            }
        }

        return _instance;
    }

    public void RegisterWriter(ILogWriter writer)
    {
        _loggerWriters.Add(writer);
    }

    public void Log(LogLevel level, string message)
    {
        var entry = new LogEntry()
        {
            LogLevel = level,
            Message = message,
            Time = DateTime.Now
        };

        _logs.Enqueue(entry);
    }

    public void LogInformation(string message)
    {
        Log(LogLevel.Information, message);
    }

    public void LogWarning(string message)
    {
        Log(LogLevel.Warning, message);
    }

    public void LogError(string message)
    {
        Log(LogLevel.Error, message);
    }

    private void Start()
    {
        logTimer = new Timer((state) => WriteLogs(state), null, TimeSpan.Zero, TimeSpan.FromMilliseconds(30));
    }

    private async Task WriteLogs(object? _)
    {
        if (_logs.TryDequeue(out var log))
        {
            await Task.WhenAll(_loggerWriters.Select(w => w.Write(log)));
        }
    }

    public void Dispose()
    {
        logTimer.Dispose();

        foreach (var logWriter in _loggerWriters)
        {
            logWriter.Dispose();
        }

    }
}