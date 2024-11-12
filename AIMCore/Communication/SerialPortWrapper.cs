using System;
using System.IO.Ports;

namespace AIMCore.Communication;

public class SerialPortWrapper : ISerialPort
{
    private readonly SerialPort _serialPort;

    public SerialPortWrapper(SerialPort serialPort)
    {
        _serialPort = serialPort;
    }

    public void Open()
    {
        _serialPort.Open();
    }

    public void Close()
    {
        _serialPort.Close();
    }

    public bool IsOpen => _serialPort.IsOpen;

    public int BaudRate
    {
        get => _serialPort.BaudRate;
        set => _serialPort.BaudRate = value;
    }

    public string PortName
    {
        get => _serialPort.PortName;
        set => _serialPort.PortName = value;
    }

    public string ReadLine()
    {
        return _serialPort.ReadLine();
    }

    public void WriteLine(string message)
    {
        _serialPort.WriteLine(message);
    }

}
