using System;

namespace AIMCore.Communication;

public interface ICommunicationFactory
{
    ICommunicationStrategy Create(CommunicationType communicationType, params object[] parameters);
}
