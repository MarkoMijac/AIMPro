using System;

namespace AIMCore;

public interface IAIModel
{
    public string Name { get; }
    public string Path { get; }

    public void LoadModel();    
    public void UnloadModel();
    public IPredictionResult Predict(MeasurementSession session);
    public Task<IPredictionResult> PredictAsync(MeasurementSession session);
}
