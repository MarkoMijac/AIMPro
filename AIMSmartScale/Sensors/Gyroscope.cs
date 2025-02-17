using System;
using AIMCore.Communication;
using AIMCore.Converters;
using AIMCore.Sensors;

namespace AIMSmartScale.Sensors;

public class Gyroscope : SensorBase<string>
{
    public Gyroscope(string name, string requestCommand, ICommunicationStrategy<string> communicationStrategy, IReadingConverter<string> converter) : base(name, requestCommand, communicationStrategy, converter)
    {
    }
}
