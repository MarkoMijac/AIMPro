using System;

namespace AIMCore.Communication;

public interface ICommunicationStrategy
{
    string Name {get;}
    void Connect();
    void Disconnect();
    void Send(string command);
    string Receive();
    bool IsConnected { get; }
}
