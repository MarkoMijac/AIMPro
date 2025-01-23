using System;

namespace AIMCore.Exceptions;

public class AIMInvalidModelPathException : AIMException
{
    public AIMInvalidModelPathException() : base("Invalid model path.")
    {
    }
}
