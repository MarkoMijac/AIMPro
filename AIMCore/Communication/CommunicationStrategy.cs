using System;

namespace AIMCore.Communication;

public abstract class CommunicationStrategy : ICommunicationStrategy
{
    public virtual bool IsConnected { get; protected set;}
    public string Name { get; protected set;}

    public abstract void Connect();
    public abstract void Disconnect();
    public abstract string Receive();
    public abstract void Send(string command);

    public override string ToString()
    {
        return Name;
    }
}
