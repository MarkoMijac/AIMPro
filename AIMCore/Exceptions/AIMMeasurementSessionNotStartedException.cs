using System;

namespace AIMCore.Exceptions;

public class AIMMeasurementSessionNotStartedException : AIMException
{
    public AIMMeasurementSessionNotStartedException() : base("Measurement session not started!")
    {
        
    }

}
