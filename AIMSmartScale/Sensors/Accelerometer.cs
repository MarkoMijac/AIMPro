using System;
using AIMCore.Communication;
using AIMCore.Converters;
using AIMCore.Sensors;

namespace AIMSmartScale.Sensors;

public class Accelerometer : SensorBase<string>
{
    public Accelerometer(string name, string requestCommand, ICommunicationStrategy<string> communicationStrategy, IReadingConverter<string> converter) : base(name, requestCommand, communicationStrategy, converter)
    {
    }
}
