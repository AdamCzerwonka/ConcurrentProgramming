using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConcurrentProgramming.Data.Logger;

public class JsonLogWriter : ILogWriter
{
    private List<LogEntry> _entries = new();
    private string _path;

    public JsonLogWriter(string path)
    {
        _path = path;
    }

    public Task Write(LogEntry log)
    {
        _entries.Add(log);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        var json = JsonSerializer.Serialize(_entries);
        File.WriteAllText(_path, json);
    }
}