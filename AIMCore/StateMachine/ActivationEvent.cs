using System;

namespace AIMCore.StateMachine;

internal enum ActivationEvent
{
    LoadConfiguration,
    ChangeConfiguration,
    StartMeasurementSession,
    EndMeasurementSession
}
