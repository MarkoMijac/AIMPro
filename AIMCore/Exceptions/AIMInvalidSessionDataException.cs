using System;

namespace AIMCore.Exceptions;

public class AIMInvalidSessionDataException : AIMException
{
    public AIMInvalidSessionDataException() : base("Invalid session data.")
    {
        
    }

}
