using System;

namespace AIMCore;

public interface ISensor
{
    string Name { get; }   
    void Connect();
    void Disconnect();
    void RequestData();
    bool IsConnected { get; }
}
