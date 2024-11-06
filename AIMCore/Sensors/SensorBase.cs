using System;
using AIMCore.Communication;
using AIMCore.Parsers;

namespace AIMCore.Sensors;

public abstract class SensorBase<T> : ISensor
{
    public string Name { get; protected set; }

    public T RequestCommand { get; protected set; }

    public IMeasurementParser<T> Parser { get; protected set; }

    public virtual bool IsConnected => CommunicationStrategy == null? false : CommunicationStrategy.IsConnected;
    public ICommunicationStrategy<T> CommunicationStrategy { get; set; }
    protected SensorBase(string name, T requestCommand, ICommunicationStrategy<T> communicationStrategy, IMeasurementParser<T> parser)
    {
        Name = name;
        RequestCommand = requestCommand;
        CommunicationStrategy = communicationStrategy;
        Parser = parser;
    }

    public virtual void Connect()
    {
        CommunicationStrategy.Connect();
    }

    public virtual async Task ConnectAsync()
    {
        await CommunicationStrategy.ConnectAsync();
    }

    public virtual void Disconnect()
    {
        CommunicationStrategy.Disconnect();
    }

    public virtual async Task DisconnectAsync()
    {
        await CommunicationStrategy.DisconnectAsync();
    }
    
    public virtual void StartReading()
    {
        CommunicationStrategy.Send(RequestCommand);
    }

    public virtual async Task StartReadingAsync()
    {
        await CommunicationStrategy.SendAsync(RequestCommand);
    }
    
    public virtual TimeSeriesData StopReading()
    {
        T data = CommunicationStrategy.Receive();
        return Parser.Parse(data);
    }
    public virtual async Task<TimeSeriesData> StopReadingAsync()
    {
        T data = await CommunicationStrategy.ReceiveAsync();
        return Parser.Parse(data);
    }
}
