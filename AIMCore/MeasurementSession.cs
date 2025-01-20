using System;

namespace AIMCore;

public class MeasurementSession
{
    public TimeSeriesData BaseInstrumentData { get; private set; }
    public List<TimeSeriesData> SensorDataSeries { get; private set; } = new List<TimeSeriesData>();

    public void SetInstrumentData(TimeSeriesData data)
    {
        BaseInstrumentData = data;
    }

    public void AddSensorData(TimeSeriesData data)
    {
        SensorDataSeries.Add(data);
    }
}
