using System;

namespace AIMCore.Communication;

public class BinaryBasedCommunicationFactory : ICommunicationFactory<byte[]>
{
    private readonly List<ICommunicationStrategy<byte[]>> _communicationStrategies;

    public BinaryBasedCommunicationFactory()
    {
        _communicationStrategies = new List<ICommunicationStrategy<byte[]>>
        {
            new USBCommunication(),
            new BluetoothCommunication()
        };
    }

    public List<ICommunicationStrategy<byte[]>> GetCommunicationStrategies()
    {
        return _communicationStrategies;
    }
}
