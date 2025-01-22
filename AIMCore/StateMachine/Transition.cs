using System;

namespace AIMCore.StateMachine;

internal class Transition
{
    public AIMStatus Status { get; private set; }
    public ActivationEvent ActivationEvent { get; private set; }
    
    public Transition(AIMStatus sourceStatus, ActivationEvent activationEvent)
    {
        Status = sourceStatus;
        ActivationEvent = activationEvent;
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        var transition = (Transition)obj;
        return Status == transition.Status && ActivationEvent == transition.ActivationEvent;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Status, ActivationEvent);
    }
}
