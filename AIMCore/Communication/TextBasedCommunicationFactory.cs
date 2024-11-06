using System;

namespace AIMCore.Communication;

public class TextBaseCommunicationFactory : ICommunicationFactory<string>
{
    private readonly List<ICommunicationStrategy<string>> _communicationStrategies;

    public TextBaseCommunicationFactory()
    {
        _communicationStrategies = new List<ICommunicationStrategy<string>>
        {
            new UARTCommunication("COM1", 9600),
            new WiFiCommunication()
        };
    }

    public List<ICommunicationStrategy<string>> GetCommunicationStrategies()
    {
        return _communicationStrategies;
    }
}
