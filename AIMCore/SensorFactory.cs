using System;
using AIMCore.Communication;
using AIMCore.Parsers;

namespace AIMCore;

public class SensorFactory
{
    public ISensor CreateSensor(string name, string requestCommand, ICommunicationStrategy communication, IMeasurementParser parser, object[] communicationParameters)
    {
        var sensor = new Sensor(name, requestCommand, communication, parser);
        return sensor;
    }
}
