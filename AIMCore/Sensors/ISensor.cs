using System;

namespace AIMCore.Sensors;

public interface ISensor
{
    string Name { get; }   
    void Connect();
    void Disconnect();

    Task ConnectAsync();
    Task DisconnectAsync();
    bool IsConnected { get; }
    SensorReading Read();
    Task<SensorReading> ReadAsync();
}
