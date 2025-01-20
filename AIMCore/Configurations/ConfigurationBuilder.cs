using System;
using AIMCore.Sensors;

namespace AIMCore.Configurations;

public class ConfigurationBuilder
{
    private ISensor _instrument;
    private List<ISensor> _sensors = new List<ISensor>();
    private IAIModel _aiModel;
    private string _name;

    public ConfigurationBuilder SetName(string name)
    {
        _name = name;
        return this;
    }

    public ConfigurationBuilder SetInstrument(ISensor instrument)
    {
        _instrument = instrument;
        return this;
    }   

    public ConfigurationBuilder SetSensors(List<ISensor> sensors)
    {
        _sensors.AddRange(sensors);
        return this;
    }

    public ConfigurationBuilder SetAIModel(IAIModel model)
    {
        _aiModel = model;
        return this;
    }


    public Configuration Build()
    {
        var configuration = new Configuration(_name);
        configuration.BaseInstrument = _instrument;
        configuration.Sensors.AddRange(_sensors);
        configuration.AIModel = _aiModel;

        ValidateConfiguration(configuration);

        return configuration;
    }

    private void ValidateConfiguration(Configuration configuration)
    {
        if (configuration.Name == null || configuration.Name == "")
        {
            throw new AIMException("Configuration name is not set!");
        }

        if (configuration.BaseInstrument == null)
        {
            throw new AIMException("Measurement instrument is not set!");
        }

        if (configuration.AIModel == null)
        {
            throw new AIMException("AI model service is not set!");
        }

        if (configuration.Sensors.Count == 0)
        {
            throw new AIMException("No sensors added!");
        }
    }
}
