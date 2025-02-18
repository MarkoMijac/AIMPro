using AIMCore.Communication;
using AIMCore.Converters;
using AIMCore.Sensors;

namespace AIMSmartScale.Sensors;

public class EnvironmentalSensor(string name, ICommunicationStrategy<string> communicationStrategy, IReadingConverter<string> converter) : SensorBase<string>(name, "", communicationStrategy, converter)
{
}
