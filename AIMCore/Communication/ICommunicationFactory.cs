using System;

namespace AIMCore.Communication;

public interface ICommunicationFactory<T>
{
    List<ICommunicationStrategy<T>> GetCommunicationStrategies();
}
