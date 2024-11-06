using System;

namespace AIMCore.Communication;

public interface ICommunicationStrategy<T>
{
    string Name {get;}
    bool IsConnected { get; }
    
    void Connect();
    void Disconnect();
    
    Task ConnectAsync();
    Task DisconnectAsync();

    void Send(T command);
    Task SendAsync(T command);

    T Receive();
    Task<T> ReceiveAsync();
    
}
