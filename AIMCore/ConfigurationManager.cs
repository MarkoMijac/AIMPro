using System;
using System.Text.Json;

namespace AIMCore;

public static class ConfigurationManager
{
    public static Configuration LoadConfiguration(string path)
    {
        Configuration configuration = new Configuration();
        string json = ReadConfigurationFile(path);
        configuration = JsonSerializer.Deserialize<Configuration>(json) ?? new Configuration();



        return configuration;
    }

    public static void SaveConfiguration(Configuration configuration, string path)
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

    private static string ReadConfigurationFile(string path)
    {
        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"Configuration file not found at path: {path}");
        }

        return File.ReadAllText(path);
    }
}
