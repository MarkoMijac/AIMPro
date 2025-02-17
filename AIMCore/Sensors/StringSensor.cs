using System;
using AIMCore.Communication;
using AIMCore.Exceptions;
using AIMCore.Converters;

namespace AIMCore.Sensors;

public class StringSensor : SensorBase<string>
{
    public StringSensor(string name, string requestCommand, ICommunicationStrategy<string> communicationStrategy, IReadingConverter<string> readingConverter)
        : base(name, requestCommand, communicationStrategy, readingConverter)
    {
        
    }

    protected override void ValidateInput(string name, string? requestCommand, ICommunicationStrategy<string> communicationStrategy, IReadingConverter<string> readingConverter)
    {
        base.ValidateInput(name, requestCommand, communicationStrategy, readingConverter);

        if(requestCommand == string.Empty)
        {
            throw new AIMException("Request command cannot be empty.");
        }
    }

}
