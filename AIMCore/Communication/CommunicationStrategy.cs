using System;

namespace AIMCore.Communication;

public abstract class CommunicationStrategy<T> : ICommunicationStrategy<T>
{
    public virtual bool IsConnected { get; protected set;}
    public string Name { get; protected set;}

    public abstract void Connect();
    public abstract void Disconnect();

    public abstract Task ConnectAsync();
    public abstract Task DisconnectAsync();

    public abstract T Execute(T command);
    public abstract Task<T> ExecuteAsync(T command);
    public override string ToString()
    {
        return Name;
    }
}
