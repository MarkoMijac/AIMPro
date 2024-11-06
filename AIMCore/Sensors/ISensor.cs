using System;

namespace AIMCore.Sensors;

public interface ISensor
{
    string Name { get; }   
    void Connect();
    void Disconnect();
    void StartReading();
    TimeSeriesData StopReading();

    Task ConnectAsync();
    Task DisconnectAsync();
    Task StartReadingAsync();
    Task<TimeSeriesData> StopReadingAsync();
    bool IsConnected { get; }
}
