using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrentProgramming.Data.Logger;

public class Logger : ILogger
{
    private readonly ConcurrentQueue<LogEntry> _logs = new();
    private Task _writer;
    private readonly List<ILogWriter> _loggerWriters = new();
    private bool isLogging;
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private static ILogger? _instance;
    private static object _loggerLock = new();

    public static ILogger GetLogger()
    {
        if (_instance is null)
        {
            lock (_loggerLock)
            {
                _instance = new Logger();
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

    public void Start()
    {
        isLogging = true;
        var token = _cancellationTokenSource.Token;
        _writer = Task.Run(() => WriteLogs(token), token);
    }

    public void StopLogging()
    {
        _cancellationTokenSource.Cancel();
    }

    public int GetNumberOfUnwrittenLogs()
    {
        return _logs.Count;
    }

    public void Dispose()
    {
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource.Dispose();
        _writer.Wait();
        _writer.Dispose();
        foreach (var logWriter in _loggerWriters)
        {
            logWriter.Dispose();
        }
    }

    private async Task WriteLogs(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested || !_logs.IsEmpty)
        {
            if (_logs.TryDequeue(out var log))
            {
                await Task.WhenAll(_loggerWriters.Select(w => w.Write(log)));
            }
        }
    }
}