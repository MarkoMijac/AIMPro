using System;

namespace AIMCore.Exceptions;

public class AIMSensorReadingException : AIMException
{
    public AIMSensorReadingException() : base("Failed to read sensor data.")
    {
        
    }

}
