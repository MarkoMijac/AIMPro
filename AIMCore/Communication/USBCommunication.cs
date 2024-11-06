using System;

namespace AIMCore.Communication;

public class USBCommunication : CommunicationStrategy<byte[]>
{
    public USBCommunication()
    {
        Name = "USB";
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

    public override byte[] Receive()
    {
        throw new NotImplementedException();
    }

    public override Task<byte[]> ReceiveAsync()
    {
        throw new NotImplementedException();
    }

    public override void Send(byte[] command)
    {
        throw new NotImplementedException();
    }

    public override Task SendAsync(byte[] command)
    {
        throw new NotImplementedException();
    }
}
