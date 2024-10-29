using System;

namespace AIMCore.Communication;

public interface ICommunicationFactory
{
    List<ICommunicationStrategy> GetCommunicationStrategies();
}
