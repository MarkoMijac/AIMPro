using System;

namespace AIMCore.Sensors;

public interface ISensor
{
    string Name { get; }   
    void Connect();
    void Disconnect();
    void StartReading();
    Measurement StopReading();

    Task ConnectAsync();
    Task DisconnectAsync();
    Task StartReadingAsync();
    Task<Measurement> StopReadingAsync();
    bool IsConnected { get; }
}
