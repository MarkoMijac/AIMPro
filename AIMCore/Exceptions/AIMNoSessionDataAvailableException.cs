using System;

namespace AIMCore.Exceptions;

public class AIMNoSessionDataAvailableException : AIMException
{
    public AIMNoSessionDataAvailableException() : base("No measurement session data available!")
    {
        
    }
}
