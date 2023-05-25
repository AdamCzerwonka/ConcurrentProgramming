using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ConcurrentProgramming.Data.Logger.LogWriter;

public class XmlLogWriter : ILogWriter
{
    private List<LogEntry> _entries = new();
    private string _path;

    public XmlLogWriter(string path)
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
        var serializer = new XmlSerializer(typeof(LogEntry[]));
        using (var writer = new StreamWriter(_path))
        {
            serializer.Serialize(writer,_entries.ToArray());
        }
    }
}