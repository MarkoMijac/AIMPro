using System;
using AIMCore.Communication;
using AIMCore.Parsers;
using AIMCore.Sensors;

namespace AIMSmartScale.Sensors;

public class Gyroscope : SensorBase<string>
{
    public Gyroscope(string name, string requestCommand, ICommunicationStrategy<string> communicationStrategy, IMeasurementParser<string> parser) : base(name, requestCommand, communicationStrategy, parser)
    {
    }
}
