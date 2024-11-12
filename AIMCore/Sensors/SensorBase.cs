using System;
using System.Net.Http.Headers;
using AIMCore.Communication;
using AIMCore.Parsers;

namespace AIMCore.Sensors;

public abstract class SensorBase<T> : ISensor
{
    public string Name { get; protected set; }

    public T RequestCommand { get; protected set; }

    public IMeasurementParser<T> Parser { get; protected set; }

    public virtual bool IsConnected => CommunicationStrategy == null? false : CommunicationStrategy.IsConnected;
    public ICommunicationStrategy<T> CommunicationStrategy { get; protected set; }
    protected SensorBase(string name, T requestCommand, ICommunicationStrategy<T> communicationStrategy, IMeasurementParser<T> parser)
    {
        ValidateInput(name, requestCommand, communicationStrategy, parser);
        Name = name;
        RequestCommand = requestCommand;
        CommunicationStrategy = communicationStrategy;
        Parser = parser;
    }

    protected virtual void ValidateInput(string name, T? requestCommand, ICommunicationStrategy<T> communicationStrategy, IMeasurementParser<T> parser)
    {
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrEmpty(name))
        {
            throw new AIMException("Name cannot be null or empty.");
        }

        if (requestCommand == null)
        {
            throw new AIMException("Request command cannot be null.");
        }

        if (communicationStrategy == null)
        {
            throw new AIMException("Communication strategy cannot be null.");
        }

        if (parser == null)
        {
            throw new AIMException("Parser cannot be null.");
        }
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
