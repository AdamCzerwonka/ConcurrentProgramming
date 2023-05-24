using System;

namespace ConcurrentProgramming.Data.Logger;

public interface ILogger : IDisposable
{
    void Log(LogLevel level, string message);
    void Start();
    public void StopLogging();
    public int GetNumberOfUnwrittenLogs();
    void RegisterWriter(ILogWriter writer);
}