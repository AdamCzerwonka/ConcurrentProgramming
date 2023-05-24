using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace ConcurrentProgramming.Data.Logger;

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
        // var xml = new XmlSerializer(typeof(XmlElement));
        // var logsElement = new XmlDocument().CreateElement("logs");
        // var doc = new XmlDocument();
        // foreach (var logType in Enum.GetValues(typeof(LogEntry)))
        // {
        //     var type = new XmlDocument().CreateElement(logType.ToString() + "s");
        //     foreach (var entry in _entries)
        //     {
        //         var entryName = new XmlDocument().CreateElement(entry.LogLevel.ToString());
        //     
        //     }
        // }
        var serializer = new XmlSerializer(typeof(LogEntry[]));
        using (var writer = new StreamWriter(_path))
        {
            serializer.Serialize(writer,_entries.ToArray());
        }
    }
}