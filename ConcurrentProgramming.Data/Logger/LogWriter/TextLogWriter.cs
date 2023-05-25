using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrentProgramming.Data.Logger.LogWriter;

public class TextLogWriter : ILogWriter
{
    public string path { get; set; }
    private readonly StreamWriter _fileWriter;

    public TextLogWriter(string path)
    {
        this.path = path;
        try
        {
            _fileWriter = new StreamWriter(path);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task Write(LogEntry log)
    {
        var builder = new StringBuilder();
        builder.Append($"[{log.LogLevel.ToString()}] ");
        builder.Append($"[{log.Time}] ");
        builder.Append($"[{log.ThreadId}] ");
        builder.Append(log.Message);
        await _fileWriter.WriteLineAsync(builder.ToString());
        _fileWriter.Flush();
    }

    public void Dispose()
    {
        _fileWriter.Dispose();
    }
}