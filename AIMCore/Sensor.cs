using System;
using AIMCore.Communication;
using AIMCore.Parsers;

namespace AIMCore;

public class Sensor : ISensor
{
    protected string _requestCommand;
    protected ICommunicationStrategy _communicationStrategy;    
    protected IMeasurementParser _parser;
    public string Name { get; private set; }

    public bool IsConnected => _communicationStrategy.IsConnected;

    public Sensor(string name, string requestCommand, ICommunicationStrategy communicationStrategy, IMeasurementParser parser)
    {
        Name = name;
        _communicationStrategy = communicationStrategy;
        _requestCommand = requestCommand;
        _parser = parser;
    }
    
    public virtual void RequestData()
    {
        _communicationStrategy.Send(_requestCommand);
    }

    public virtual MeasurementData ReceiveData()
    {
        string data = _communicationStrategy.Receive();
        return _parser.Parse(data);
    }

    public void Connect()
    {
        _communicationStrategy.Connect();
    }

    public void Disconnect()
    {
        _communicationStrategy.Disconnect();
    }
}
