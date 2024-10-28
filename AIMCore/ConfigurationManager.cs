using System;
using System.Text.Json;
using AIMCore.Communication;

namespace AIMCore;

public class ConfigurationManager
{
    private SensorFactory _sensorFactory;

    public ConfigurationManager(SensorFactory sensorFactory)
    {
        _sensorFactory = sensorFactory;
    }

    public Configuration LoadConfiguration(string path)
    {
        string json = ReadConfigurationFile(path);
        var configuration = JsonSerializer.Deserialize<Configuration>(json);
        return configuration;
    }

    public void SaveConfiguration(Configuration configuration, string path)
    {
        try
        {
            configuration.ValidateConfiguration();

            string json = JsonSerializer.Serialize(configuration);
            File.WriteAllText(path, json);
        }
        catch (AIMException e)
        {
            Console.WriteLine(e.Message);
            return;
        }
    }

    private string ReadConfigurationFile(string path)
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"Configuration file not found at path: {path}");
        }

        return File.ReadAllText(path);
    }
}
