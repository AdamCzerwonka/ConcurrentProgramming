using System;
using System.Threading;

namespace ConcurrentProgramming.Data.Logger;

public class LogEntry
{
    public LogLevel LogLevel { get; set; }
    public DateTime Time { get; set; }
    public int ThreadId { get; set; } = Environment.CurrentManagedThreadId;
    public string Message { get; set; } = null!;

}