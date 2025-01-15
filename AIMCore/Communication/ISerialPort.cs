using System;

namespace AIMCore.Communication;

public interface ISerialPort
{
    void Open();
    void Close();
    bool IsOpen { get; }
    string ReadLine();
    void WriteLine(string message);
    int BaudRate { get; set; }
    string PortName { get; set; }
}
