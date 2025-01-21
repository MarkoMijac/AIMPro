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

    public bool IsValid()
    {
        return IsBaseInstrumentDataValid() == true && AreSensorDataSeriesValid() == true;
    }

    private bool IsBaseInstrumentDataValid()
    {
        return BaseInstrumentData != null && BaseInstrumentData.Measurements.Count > 0;
    }

    private bool AreSensorDataSeriesValid()
    {
        return SensorDataSeries.Count > 0 && SensorDataSeries.All(sensorData => sensorData.Measurements.Count > 0);
    }
}
