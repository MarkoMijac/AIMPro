using System;

namespace AIMCore.Sensors;

public interface ISensor
{
    string Name { get; }   
    void Connect();
    void Disconnect();
    void RequestData();
    Measurement ReceiveData();
    bool IsConnected { get; }
}
