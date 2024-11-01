using System;

namespace AIMCore;

public interface IPredictionResult
{
    public double CorrectedMeasurement { get; set; }
    public double ConfidenceScore { get; set; }
    public double ErrorMargin { get; set; }
}
