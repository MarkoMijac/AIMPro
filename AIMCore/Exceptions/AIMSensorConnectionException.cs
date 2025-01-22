using System;

namespace AIMCore.Exceptions;

public class AIMSensorConnectionException : AIMException
{
    public AIMSensorConnectionException() : base("Failed to connect to sensor.")
    {
        
    }

}
