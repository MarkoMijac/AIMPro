using System;
using AIMCore.Exceptions;

namespace AIMCore.StateMachine;

internal class AIMStateManager
{
    public AIMStatus CurrentStatus { get; private set; }
    private Dictionary<Transition, AIMStatus> _allowedTransitions;

    public AIMStateManager()
    {
        CurrentStatus = AIMStatus.NoConfiguration;
        DefineTransitions();
    }

    private void DefineTransitions()
    {
        _allowedTransitions = new Dictionary<Transition, AIMStatus>
        {
            { new Transition(AIMStatus.NoConfiguration, ActivationEvent.LoadConfiguration), AIMStatus.Ready },
            { new Transition(AIMStatus.Ready, ActivationEvent.ChangeConfiguration), AIMStatus.Ready },
            { new Transition(AIMStatus.Ready, ActivationEvent.StartMeasurementSession), AIMStatus.Measuring },
            { new Transition(AIMStatus.Measuring, ActivationEvent.EndMeasurementSession), AIMStatus.Ready },
        };
    }

    public void PerformTransition(ActivationEvent activationEvent)
    {
        var transition = new Transition(CurrentStatus, activationEvent);
        if (_allowedTransitions.ContainsKey(transition))
        {
            CurrentStatus = _allowedTransitions[transition];
        }
        else
        {
            throw new AIMInvalidTransitionException();
        }
    }
}
