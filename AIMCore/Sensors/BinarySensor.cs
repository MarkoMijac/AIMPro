using System;
using AIMCore.Communication;
using AIMCore.Exceptions;
using AIMCore.Converters;

namespace AIMCore.Sensors;

public class BinarySensor : SensorBase<byte[]>
{
    public BinarySensor(string name, byte[] requestCommand, ICommunicationStrategy<byte[]> communicationStrategy, IReadingConverter<byte[]> readingConverter)
        : base(name, requestCommand, communicationStrategy, readingConverter)
    {
        
    }

    protected override void ValidateInput(string name, byte[]? requestCommand, ICommunicationStrategy<byte[]> communicationStrategy, IReadingConverter<byte[]> readingConverter)
    {
        base.ValidateInput(name, requestCommand, communicationStrategy, readingConverter);

        if(requestCommand == null || requestCommand.Length == 0)
        {
            throw new AIMException("Request command cannot be null or empty.");
        }
    }

}
