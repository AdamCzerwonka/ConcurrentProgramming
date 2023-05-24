using System;

namespace ConcurrentProgramming.Data.Logger;

public class LogEntry
{
    public LogLevel LogLevel { get; set; }
    public DateTime Time { get; set; }
    public string Message { get; set; } = null!;
}