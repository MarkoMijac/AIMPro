using System;

namespace AIMCore.Exceptions;

public class AIMInvalidConfigurationException : AIMException
{
    public AIMInvalidConfigurationException() : base("Provided configuration is not valid!")
    {
        
    }

}
