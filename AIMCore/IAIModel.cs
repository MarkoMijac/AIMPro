using System;

namespace AIMCore;

public interface IAIModel : IDisposable
{
    public string Name { get; }
    public string Path { get; }
    public IPredictionResult Predict(MeasurementSession session);
    public Task<IPredictionResult> PredictAsync(MeasurementSession session);
}
