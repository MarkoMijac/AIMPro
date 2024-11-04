using System;
using System.IO;
using System.Text.Json;
using AIMCore.Configurations;

namespace AIMCore
{
    public static class ConfigurationSerializer
    {
        public static string Serialize(Configuration config)
        {
            return JsonSerializer.Serialize(config);
        }

        public static Configuration Deserialize(string json)
        {
            return JsonSerializer.Deserialize<Configuration>(json);
        }

        public static void SaveToFile(Configuration config, string filePath)
        {
            var json = Serialize(config);
            File.WriteAllText(filePath, json);
        }

        public static Configuration LoadFromFile(string filePath)
        {
            var json = File.ReadAllText(filePath);
            return Deserialize(json);
        }
    }
}
