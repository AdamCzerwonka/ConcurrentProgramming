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
    
    public Logger(ILoggerWriter loggerWriter)
    {
        _loggerWriter = loggerWriter;
        _logs = new ConcurrentQueue<string>();
        _writer = Task.Run(WriteLogs);
    }

    public void AddLog(string newLog)
    {
        _logs.Enqueue(newLog);
    }

    public void WriteLogs()
    {
        while (true)
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