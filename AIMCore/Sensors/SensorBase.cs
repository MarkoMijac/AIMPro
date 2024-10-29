using System;
using AIMCore.Communication;
using AIMCore.Parsers;

namespace AIMCore.Sensors;

public abstract class SensorBase : ISensor
{
    public string Name { get; protected set; }

    public string RequestCommand { get; protected set; }

    public virtual bool IsConnected { get; protected set; }

    protected SensorBase(string name)
    {
        Name = name;
        IsConnected = false;
    }

    public abstract void Connect();
    public abstract void Disconnect();
    public abstract void RequestData();
    public abstract MeasurementData ReceiveData();
}
