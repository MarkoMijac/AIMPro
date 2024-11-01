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

    public IAIModel AIModel { get; set; }
}
