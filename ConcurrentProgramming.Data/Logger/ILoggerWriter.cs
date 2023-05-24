namespace ConcurrentProgramming.Data;

public interface ILoggerWriter
{
    string path { get; set; }
    void Write(string log);
}