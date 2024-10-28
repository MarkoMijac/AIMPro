using System;

namespace AIMCore.Communication;

public interface ICommunicationStrategy
{
    void Connect();
    void Disconnect();
    void Send(string command);
    string Receive();
    bool IsConnected { get; }
}
