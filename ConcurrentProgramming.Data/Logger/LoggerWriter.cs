using System;
using System.IO;

namespace ConcurrentProgramming.Data;

public class LoggerWriter : ILoggerWriter
{
    public string path { get; set; }
    private readonly StreamWriter _fileWriter;

    public LoggerWriter(string path)
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

    public void Write(string log)
    {
        _fileWriter.WriteLine(log);
        _fileWriter.Flush();
    }
}