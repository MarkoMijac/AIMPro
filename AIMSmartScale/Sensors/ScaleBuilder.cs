using System;
using AIMCore.Sensors;

namespace AIMSmartScale.Sensors;

public class ScaleBuilder : SensorBuilder<string>
{
    public override ISensor Build()
    {
        return new Scale(_name, _requestCommand, _communicationStrategy, _parser);
    }
}
