using System;

namespace ConcurrentProgramming.Data.Logger;

public class LogEntry
{
    public DateTime Time { get; set; }
    public string Message { get; set; } = null!;
    public LogLevel LogLevel { get; set; }
}