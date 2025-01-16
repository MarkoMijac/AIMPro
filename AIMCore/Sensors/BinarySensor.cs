using System;
using AIMCore.Communication;
using AIMCore.Parsers;

namespace AIMCore.Sensors;

public class BinarySensor : SensorBase<byte[]>
{
    public BinarySensor(string name, byte[] requestCommand, ICommunicationStrategy<byte[]> communicationStrategy, IMeasurementParser<byte[]> measurementParser)
        : base(name, requestCommand, communicationStrategy, measurementParser)
    {
        
    }

    protected override void ValidateInput(string name, byte[]? requestCommand, ICommunicationStrategy<byte[]> communicationStrategy, IMeasurementParser<byte[]> parser)
    {
        base.ValidateInput(name, requestCommand, communicationStrategy, parser);

        if(requestCommand == null || requestCommand.Length == 0)
        {
            throw new AIMException("Request command cannot be null or empty.");
        }
    }

}
