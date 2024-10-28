using System;

namespace AIMCore;

public class AIMException : ApplicationException
{
    public AIMException() : base("An error occurred in the AIM system")
    {
        
    }
    
    public AIMException(string message) : base(message)
    {
    }
}

