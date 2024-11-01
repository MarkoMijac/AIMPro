using System;

namespace AIMCore;

public class TimeSeriesData
{
    public string SourceName { get; set; }
    public List<Measurement> Measurements { get; set; }

    public TimeSeriesData(string sourceName)
    {
        SourceName = sourceName;
        Measurements = new List<Measurement>();        
    }

    public void AddMeasurement(Measurement measurement)
    {
        Measurements.Add(measurement);
    }

}
