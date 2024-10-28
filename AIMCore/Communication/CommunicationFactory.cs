using System;

namespace AIMCore.Communication;

public class CommunicationFactory
{
    public ICommunicationStrategy Create(CommunicationType communicationType, params object[] parameters)
    {
        switch (communicationType)
        {
            case CommunicationType.UART:
                return new UARTCommunication((string)parameters[0], (int)parameters[1]);
            case CommunicationType.USB:
                return new USBCommunication();
            case CommunicationType.WiFi:
                return new WiFiCommunication();
            case CommunicationType.Bluetooth:
                return new BluetoothCommunication();
            default:
                throw new ArgumentException("Invalid communication type");
        }
    }
}
