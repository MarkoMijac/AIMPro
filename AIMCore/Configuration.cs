using System;
using AIMCore.Communication;
using AIMCore.Parsers;

namespace AIMCore;

public class Configuration
{
    private SensorFactory _sensorFactory;

    public string Name { get; set; }

    public ISensor BaseInstrument { get; set; }

    public List<ISensor> Sensors { get; set; }

    public IModel AIModel { get; set; }

    public Configuration(SensorFactory sensorFactory)
    {
        _sensorFactory = sensorFactory;
    }

    public Configuration()
    {
        
    }

    public void AddInstrument(string name, string requestCommand, ICommunicationStrategy communication, IMeasurementParser parser, params object[] communicationParameters)
    {
        ISensor sensor = _sensorFactory.CreateSensor(name, requestCommand, communication, parser, communicationParameters);
        BaseInstrument = sensor;
    }

    public void RemoveInstrument()
    {
        BaseInstrument = null;
    }

    
    public void ValidateConfiguration()
    {
        if(Name.Length == 0)
        {
            throw new AIMException("Configuration name is not set!");
        }

        if(BaseInstrument == null)
        {
            throw new AIMException("Measurement instrument is not set!");
        }

        if(AIModel == null)
        {
            throw new AIMException("AI model service is not set!");
        }

        if(Sensors.Count == 0)
        {
            throw new AIMException("No sensors added!");
        }
    }
}
