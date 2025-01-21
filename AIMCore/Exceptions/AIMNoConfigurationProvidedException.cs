using System;

namespace AIMCore.Exceptions;

public class AIMNoConfigurationProvidedException : AIMException
{
    public AIMNoConfigurationProvidedException() : base("No configuration provided!")
    {
        
    }

}
