using System;
using AIMCore.Communication;
using AIMCore.Exceptions;
using AIMCore.Parsers;

namespace AIMCore.Sensors;

public abstract class SensorBuilder<T> : ISensorBuilder
{
    protected string _name;
    protected string _requestCommand;
    protected ICommunicationStrategy<T> _communicationStrategy;
    protected IMeasurementParser<T> _parser;

    public SensorBuilder<T> SetName(string name)
    {
        _name = name;
        return this;
    }

    public SensorBuilder<T> SetRequestCommand(string requestCommand)
    {
        _requestCommand = requestCommand;
        return this;
    }

    public SensorBuilder<T> SetCommunicationStrategy(ICommunicationStrategy<T> communicationStrategy)
    {
        _communicationStrategy = communicationStrategy;
        return this;
    }

    public SensorBuilder<T> SetParser(IMeasurementParser<T> parser)
    {
        _parser = parser;
        return this;
    }


    public abstract ISensor Build();

    protected virtual void ValidateSensor(SensorBase<T> sensor)
    {
        if (string.IsNullOrEmpty(sensor.Name))
        {
            throw new AIMException("Instrument name cannot be empty.");
        }
        if (sensor.RequestCommand == null)
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
