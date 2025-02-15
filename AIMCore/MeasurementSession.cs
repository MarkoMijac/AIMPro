using System;

namespace AIMCore;

public class MeasurementSession
{
    public Measurement BaseInstrumentData { get; private set; }
    public List<Measurement> SensorDataSeries { get; private set; } = new List<Measurement>();

    public void SetInstrumentData(Measurement data)
    {
        BaseInstrumentData = data;
    }

    public void AddSensorData(Measurement data)
    {
        SensorDataSeries.Add(data);
    }

    public bool IsValid()
    {
        return IsBaseInstrumentDataValid() == true && AreSensorDataSeriesValid() == true;
    }

    private bool IsBaseInstrumentDataValid()
    {
        return BaseInstrumentData != null;
    }

    private bool AreSensorDataSeriesValid()
    {
        return SensorDataSeries.Count > 0 && SensorDataSeries.All(sensorData => sensorData != null);
    }
}
