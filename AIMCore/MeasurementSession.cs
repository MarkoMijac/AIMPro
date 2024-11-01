using System;

namespace AIMCore;

public class MeasurementSession
{
    public TimeSeriesData BaseInstrumentData { get; set; }
    public List<TimeSeriesData> SensorDataSeries { get; set; } = new List<TimeSeriesData>();

    public void SetInstrumentData(TimeSeriesData data)
    {
        BaseInstrumentData = data;
    }

    public void AddSensorData(TimeSeriesData data)
    {
        SensorDataSeries.Add(data);
    }
}
