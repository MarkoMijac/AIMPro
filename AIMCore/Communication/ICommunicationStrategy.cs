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

    T Execute(T command);    
    Task<T> ExecuteAsync(T command);
}
