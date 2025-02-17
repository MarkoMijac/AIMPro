using System;

namespace AIMCore.Sensors;

public interface ISensor
{
    string Name { get; }   
    void Connect();
    void Disconnect();
    void StartReading();
    SensorReading StopReading();

    Task ConnectAsync();
    Task DisconnectAsync();
    Task StartReadingAsync();
    Task<SensorReading> StopReadingAsync();
    bool IsConnected { get; }
    SensorReading Read();
    Task<SensorReading> ReadAsync();
}
