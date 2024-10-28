using System;
using System.IO.Ports;

namespace AIMCore.Communication;

public class UARTCommunication : ICommunicationStrategy
{
    private readonly string _portName;
    private readonly int _baudRate;
    private SerialPort _serialPort;
    public UARTCommunication(string portName, int baudRate)
    {
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
            WriteTimeout = 500
        };
    }
    
    public void Connect()
    {
        if(_serialPort.IsOpen == false)
        {
            _serialPort.Open();
        }
    }

    public void Disconnect()
    {
        if(_serialPort.IsOpen)
        {
            _serialPort.Close();
        }
    }

    public bool IsConnected => _serialPort.IsOpen;

    public string Receive()
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

    public void Send(string command)
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
}
