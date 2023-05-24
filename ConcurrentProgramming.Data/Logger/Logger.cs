using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace ConcurrentProgramming.Data;

public class Logger : ILogger
{
    private readonly ConcurrentQueue<string> _logs;
    private Task _writer;
    private ILoggerWriter _loggerWriter;
    private bool isLogging;

    public Logger(ILoggerWriter loggerWriter)
    {
        _loggerWriter = loggerWriter;
        _logs = new ConcurrentQueue<string>();
    }

    public void AddLog(string newLog)
    {
        _logs.Enqueue(newLog);
    }

    public void Start()
    {
        isLogging = true;
        _writer = Task.Run(WriteLogs);
    }

    public void StopLogging()
    {
        isLogging = false;
    }

    public int GetNumberOfUnwrittenLogs()
    {
        return _logs.Count;
    }

    public void Dispose()
    {
        _writer.Dispose();
        _loggerWriter.Dispose();
    }

    public void WriteLogs()
    {
        while (isLogging && _logs.Count > 0)
        {
            string? log;
            var anyLogs = _logs.TryDequeue(out log);

            if (anyLogs)
            {
                _loggerWriter.Write(log);
            }
        }
    }
}