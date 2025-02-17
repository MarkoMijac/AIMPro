using System;
using System.IO.Ports;
using System.Runtime.InteropServices;
using AIMCore.Exceptions;

namespace AIMCore.Communication;

public class UARTCommunication : CommunicationStrategy<string>
{
    private ISerialPort _serialPort;
    public override bool IsConnected => _serialPort?.IsOpen ?? false;

    public UARTCommunication(ISerialPort serialPort)
    {
        Name = "UART";
        _serialPort = serialPort;
    }
    
    public override void Connect()
    {
        if(IsConnected == false)
        {
            _serialPort.Open();
        }
    }

    public override async Task ConnectAsync()
    {
        await Task.Run(() => Connect());
    }

    public override void Disconnect()
    {
        if(IsConnected == true)
        {
            _serialPort.Close();
        }
    }

    public override async Task DisconnectAsync()
    {
        await Task.Run(() => Disconnect());
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
