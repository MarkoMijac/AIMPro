using System;
using AIMCore.Communication;
using AIMCore.Parsers;

namespace AIMCore;

public class SensorFactory
{
    private ICommunicationFactory _communicationFactory;
    private IMeasurementParserFactory _parserFactory;

    public SensorFactory(ICommunicationFactory communicationFactory, IMeasurementParserFactory parserFactory)
    {
        _communicationFactory = communicationFactory;
        _parserFactory = parserFactory;
    }

    public ISensor CreateSensor(string name, string requestCommand, CommunicationType communicationType, string parserType, object[] communicationParameters)
    {
        var communicationStrategy = _communicationFactory.Create(communicationType, communicationParameters);
        var parser = _parserFactory.GetParser(parserType);

        var sensor = new Sensor(name, requestCommand, communicationStrategy, parser);
        return sensor;
    }
}
