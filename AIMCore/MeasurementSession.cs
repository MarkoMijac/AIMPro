using System;

namespace AIMCore;

public class MeasurementSession
{
    public SensorReading BaseInstrumentReading { get; private set; }
    public List<SensorReading> SensorsReadings { get; private set; } = new List<SensorReading>();

    public void SetInstrumentData(SensorReading reading)
    {
        BaseInstrumentReading = reading;
    }

    public void AddSensorData(SensorReading reading)
    {
        SensorsReadings.Add(reading);
    }

    public bool IsValid()
    {
        return IsBaseInstrumentDataValid() == true && AreSensorDataSeriesValid() == true;
    }

    private bool IsBaseInstrumentDataValid()
    {
        return BaseInstrumentReading != null && BaseInstrumentReading.Measurements.Count > 0;
    }

    private bool AreSensorDataSeriesValid()
    {
        return SensorsReadings.Count > 0 && SensorsReadings.All(reading => reading != null && reading.Measurements.Count > 0);
    }
}
