using System;
using System.Net.Http.Headers;
using AIMCore.Communication;
using AIMCore.Exceptions;
using AIMCore.Converters;

namespace AIMCore.Sensors;

public abstract class SensorBase<T> : ISensor
{
    public string Name { get; protected set; }

    public T RequestCommand { get; protected set; }

    public IReadingConverter<T> Converter { get; protected set; }

    public bool IsReading { get; protected set; }

    public virtual bool IsConnected => CommunicationStrategy == null? false : CommunicationStrategy.IsConnected;
    public ICommunicationStrategy<T> CommunicationStrategy { get; protected set; }
    protected SensorBase(string name, T requestCommand, ICommunicationStrategy<T> communicationStrategy, IReadingConverter<T> converter)
    {
        ValidateInput(name, requestCommand, communicationStrategy, converter);
        Name = name;
        RequestCommand = requestCommand;
        CommunicationStrategy = communicationStrategy;
        Converter = converter;

        IsReading = false;
    }

    protected virtual void ValidateInput(string name, T? requestCommand, ICommunicationStrategy<T> communicationStrategy, IReadingConverter<T> converter)
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

        if (converter == null)
        {
            throw new AIMException("Converter cannot be null.");
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

    public SensorReading Read()
    {
        if(IsReading == true)
        {
            throw new AIMException("Sensor is already reading.");
        }

        IsReading = true;
        T data = CommunicationStrategy.Execute(RequestCommand);
        var sensorReading = Converter.Convert(data);
        IsReading = false;
        
        return sensorReading;
    }

    public async Task<SensorReading> ReadAsync()
    {
        if(IsReading == true)
        {
            throw new AIMException("Sensor is already reading.");
        }

        IsReading = true;
        T data = await CommunicationStrategy.ExecuteAsync(RequestCommand);
        var sensorReading = Converter.Convert(data);
        IsReading = false;
        
        return sensorReading;
    }
}
