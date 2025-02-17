using System;
using System.Globalization;
using AIMCore.Converters;

namespace AIMCore.IntegrationTests.Utilities;

public class GyroscopeReadingConverter : ReadingConverterBase<string>
{
    public GyroscopeReadingConverter()
    {
        Name = "Gyroscope converter";
    }

    public override SensorReading Convert(string data)
    {
        var dataPoints = data.Split(';');

        DateTime.TryParse(dataPoints[0].Trim(), out var timestamp);
        float.TryParse(dataPoints[1].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out var incline);

        var reading = new SensorReading("Gyrocope");
        reading.AddMeasurement("incline", incline);
        reading.TimeStamp = timestamp;
        return reading;
    }
}
