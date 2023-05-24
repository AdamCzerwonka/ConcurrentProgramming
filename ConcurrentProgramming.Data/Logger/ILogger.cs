namespace ConcurrentProgramming.Data;

public interface ILogger
{
    void AddLog(string newLog);
    void WriteLogs();
}