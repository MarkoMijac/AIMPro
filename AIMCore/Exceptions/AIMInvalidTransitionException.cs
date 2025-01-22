using System;

namespace AIMCore.Exceptions;

public class AIMInvalidTransitionException : AIMException
{
    public AIMInvalidTransitionException() : base("Invalid transition")
    {
    }
}
