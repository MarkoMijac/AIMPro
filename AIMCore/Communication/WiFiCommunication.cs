using System;

namespace AIMCore.Communication;

public class WiFiCommunication : CommunicationStrategy
{
    public WiFiCommunication()
    {
        Name = "WiFi";
        IsConnected = false;
    }

    public override void Connect()
    {
        throw new NotImplementedException();
    }

    public override void Disconnect()
    {
        throw new NotImplementedException();
    }

    public override string Receive()
    {
        throw new NotImplementedException();
    }

    public override void Send(string command)
    {
        throw new NotImplementedException();
    }
}
