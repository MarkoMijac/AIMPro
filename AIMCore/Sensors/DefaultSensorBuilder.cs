using System;
using AIMCore.Communication;
using AIMCore.Parsers;

namespace AIMCore.Sensors;

public class DefaultSensorBuilder : ISensorBuilder
{
    private string _name;
    private string _requestCommand;
    private ICommunicationStrategy _communicationStrategy;
    private IMeasurementParser _parser;

    public DefaultSensorBuilder SetName(string name)
    {
        _name = name;
        return this;
    }

    public DefaultSensorBuilder SetRequestCommand(string requestCommand)
    {
        _requestCommand = requestCommand;
        return this;
    }

    public DefaultSensorBuilder SetCommunicationStrategy(ICommunicationStrategy communicationStrategy)
    {
        _communicationStrategy = communicationStrategy;
        return this;
    }

    public DefaultSensorBuilder SetParser(IMeasurementParser parser)
    {
        _parser = parser;
        return this;
    }


    public ISensor Build()
    {
        var sensor = new DefaultSensor(_name, _requestCommand, _communicationStrategy, _parser);
        ValidateSensor(sensor);
        return sensor;
    }

    private void ValidateSensor(DefaultSensor sensor)
    {
        if (string.IsNullOrEmpty(sensor.Name))
        {
            throw new AIMException("Instrument name cannot be empty.");
        }
        if (string.IsNullOrEmpty(sensor.RequestCommand))
        {
            throw new AIMException("Request command cannot be empty.");
        }
        if (sensor.CommunicationStrategy == null)
        {
            throw new AIMException("Communication type must be selected.");
        }
        if (sensor.Parser == null)
        {
            throw new AIMException("Parser must be selected.");
        }
    }
}
