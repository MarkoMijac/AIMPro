using System;

namespace AIMCore.Communication;

public class BluetoothCommunication : CommunicationStrategy<byte[]>
{
    public BluetoothCommunication()
    {
        IsConnected = false;
        Name = "Bluetooth";
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

    public override byte[] Execute(byte[] command)
    {
        throw new NotImplementedException();
    }

    public override Task<byte[]> ExecuteAsync(byte[] command)
    {
        throw new NotImplementedException();
    }
}
