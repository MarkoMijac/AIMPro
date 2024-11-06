using System;
using AIMCore.Communication;
using AIMCore.Parsers;
using AIMCore.Sensors;

namespace AIMSmartScale.Sensors;

public class Accelerometer : SensorBase<string>
{
    public Accelerometer(string name, string requestCommand, ICommunicationStrategy<string> communicationStrategy, IMeasurementParser<string> parser) : base(name, requestCommand, communicationStrategy, parser)
    {
    }
}
