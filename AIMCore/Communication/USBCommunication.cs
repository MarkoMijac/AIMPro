using System;

namespace AIMCore.Communication;

public class USBCommunication : ICommunicationStrategy
{
    public bool IsConnected => throw new NotImplementedException();

    public void Connect()
    {
        throw new NotImplementedException();
    }

    public void Disconnect()
    {
        throw new NotImplementedException();
    }

    public string Receive()
    {
        throw new NotImplementedException();
    }

    public void Send(string command)
    {
        throw new NotImplementedException();
    }
}
