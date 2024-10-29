using System;
using AIMCore.Communication;
using AIMCore.Parsers;
using AIMCore.Sensors;

namespace AIMCore;

public class Configuration
{
    public string Name { get; set; }

    public ISensor BaseInstrument { get; set; }

    public List<ISensor> Sensors { get; set; }

    public IModel AIModel { get; set; }

    public Configuration()
    {
        
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
