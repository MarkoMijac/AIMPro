using System;
using System.IO.Ports;
using System.Runtime.InteropServices;

namespace AIMCore.Communication;

public class UARTCommunication : CommunicationStrategy<string>
{
    private readonly string _portName;
    private readonly int _baudRate;
    private SerialPort _serialPort;
    public override bool IsConnected => _serialPort?.IsOpen ?? false;

    public UARTCommunication(string portName, int baudRate)
    {
        Name = "UART";

        _portName = portName;
        _baudRate = baudRate;
        _serialPort = new SerialPort(_portName, _baudRate)
        {
            Parity = Parity.None,
            StopBits = StopBits.One,
            DataBits = 8,
            Handshake = Handshake.None,
            RtsEnable = true,
            ReadTimeout = 500,
            WriteTimeout = 500,
            NewLine = "\r\n"
        };
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
            throw new InvalidOperationException("Serial port is not open");
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
            throw new InvalidOperationException("Serial port is not open");
        }
    }

    public override Task SendAsync(string command)
    {
        return Task.Run(() => Send(command));
    }
}
