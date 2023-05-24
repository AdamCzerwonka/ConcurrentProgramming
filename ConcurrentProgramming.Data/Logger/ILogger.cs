namespace ConcurrentProgramming.Data;

public interface ILogger
{
    void AddLog(string newLog);
    void Start();
    public void StopLogging();
    public int GetNumberOfUnwrittenLogs();
    void Dispose();
    void WriteLogs();
}