using System;
using ConcurrentProgramming.Data.Logger.LogWriter;

namespace ConcurrentProgramming.Data.Logger;

public interface ILogger : IDisposable
{
    void Log(LogLevel level, string message);
    void RegisterWriter(ILogWriter writer);
    void LogInformation(string message);
    void LogWarning(string message);
    void LogError(string message);
}