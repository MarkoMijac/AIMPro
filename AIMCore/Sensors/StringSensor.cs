using System;
using AIMCore.Communication;
using AIMCore.Exceptions;
using AIMCore.Parsers;

namespace AIMCore.Sensors;

public class StringSensor : SensorBase<string>
{
    public StringSensor(string name, string requestCommand, ICommunicationStrategy<string> communicationStrategy, IMeasurementParser<string> measurementParser)
        : base(name, requestCommand, communicationStrategy, measurementParser)
    {
        
    }

    protected override void ValidateInput(string name, string? requestCommand, ICommunicationStrategy<string> communicationStrategy, IMeasurementParser<string> parser)
    {
        base.ValidateInput(name, requestCommand, communicationStrategy, parser);

        if(requestCommand == string.Empty)
        {
            throw new AIMException("Request command cannot be empty.");
        }
    }

}
