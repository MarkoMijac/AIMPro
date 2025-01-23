using System;

namespace AIMCore.Exceptions;

public class AIMLoadingModelException : AIMException
{
    public AIMLoadingModelException() : base("Failed to load the AI model.")
    {
    }
}
