using System;
using System.IO.Ports;
using System.Runtime.InteropServices;

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

    public override string Receive()
    {
        if(_serialPort.IsOpen)
        {
            return _serialPort.ReadLine();
        }
        else
        {
            throw new AIMException("Serial port is not open");
        }
    }

    public override async Task<string> ReceiveAsync()
    {
        return await Task.Run(() => Receive());
    }

    public override void Send(string command)
    {
        if(_serialPort.IsOpen)
        {
            _serialPort.WriteLine(command);
        }
        else
        {
            throw new AIMException("Serial port is not open");
        }
    }

    public override Task SendAsync(string command)
    {
        return Task.Run(() => Send(command));
    }
}
