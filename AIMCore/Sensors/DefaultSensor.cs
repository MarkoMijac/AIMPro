using System;
using AIMCore.Communication;
using AIMCore.Parsers;

namespace AIMCore.Sensors;

public class DefaultSensor : SensorBase
{
    private ICommunicationStrategy communicationStrategy;
    private IMeasurementParser parser;

    public override bool IsConnected => CommunicationStrategy.IsConnected;

    public ICommunicationStrategy CommunicationStrategy { get => communicationStrategy; protected set => communicationStrategy = value; }
    public IMeasurementParser Parser { get => parser; protected set => parser = value; }

    public DefaultSensor(string name, string requestCommand, ICommunicationStrategy communicationStrategy, IMeasurementParser parser)
    :base(name)
    {
        CommunicationStrategy = communicationStrategy;
        RequestCommand = requestCommand;
        Parser = parser;
    }
    
    public override void RequestData()
    {
        CommunicationStrategy.Send(RequestCommand);
    }

    public override Measurement ReceiveData()
    {
        string data = CommunicationStrategy.Receive();
        return Parser.Parse(data);
    }

    public override void Connect()
    {
        CommunicationStrategy.Connect();
    }

    public override void Disconnect()
    {
        CommunicationStrategy.Disconnect();
    }
}
