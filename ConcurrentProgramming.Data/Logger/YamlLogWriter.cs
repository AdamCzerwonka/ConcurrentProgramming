using System;
using System.IO;
using System.Threading.Tasks;
using YamlDotNet.RepresentationModel;

namespace ConcurrentProgramming.Data.Logger;

public class YamlLogWriter : ILogWriter
{
    private string _path;
    private readonly StreamWriter _fileWriter;

    public YamlLogWriter(string path)
    {
        _path = path;
        try
        {
            _fileWriter = new StreamWriter(path);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public Task Write(LogEntry log)
    {
        var yaml = new YamlStream(
            new YamlDocument(
                new YamlMappingNode(new YamlScalarNode(log.LogLevel.ToString()),
                    new YamlMappingNode(
                            new YamlScalarNode("date"), new YamlScalarNode(log.Time.ToString()),
                            new YamlScalarNode("message"), new YamlScalarNode(log.Message)))));

        yaml.Save(_fileWriter);
        _fileWriter.Flush();
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _fileWriter.Dispose();
    }
}