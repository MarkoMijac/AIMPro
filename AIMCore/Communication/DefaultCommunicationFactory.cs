using System;

namespace AIMCore.Communication;

public class DefaultCommunicationFactory : ICommunicationFactory
{
    private readonly List<ICommunicationStrategy> _communicationStrategies;

    public DefaultCommunicationFactory()
    {
        _communicationStrategies = new List<ICommunicationStrategy>
        {
            new UARTCommunication("COM1", 9600),
            new USBCommunication(),
            new WiFiCommunication(),
            new BluetoothCommunication()
        };
    }

    public List<ICommunicationStrategy> GetCommunicationStrategies()
    {
        return _communicationStrategies;
    }
}
