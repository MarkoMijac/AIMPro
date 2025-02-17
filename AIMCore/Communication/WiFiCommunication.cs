using System;

namespace AIMCore.Communication;

public class WiFiCommunication : CommunicationStrategy<string>
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

    public override Task ConnectAsync()
    {
        throw new NotImplementedException();
    }

    public override void Disconnect()
    {
        throw new NotImplementedException();
    }

    public override Task DisconnectAsync()
    {
        throw new NotImplementedException();
    }

    public override string Execute(string command)
    {
        throw new NotImplementedException();
    }

    public override Task<string> ExecuteAsync(string command)
    {
        throw new NotImplementedException();
    }
}
