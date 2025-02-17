using System;
using System.Globalization;
using AIMCore.Converters;

namespace AIMCore.IntegrationTests.Utilities;

public class VibrationReadingConverter : ReadingConverterBase<string>
{
    public VibrationReadingConverter()
    {
        Name = "Vibration Converter";
    }

    public override SensorReading Convert(string data)
    {
        var dataPoints = data.Split(';');

        DateTime.TryParse(dataPoints[0].Trim(), out var timestamp);
        float.TryParse(dataPoints[1].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out var vibration);

        var reading = new SensorReading("Vibration reading");
        reading.AddMeasurement("vibrationRate", vibration);
        reading.TimeStamp = timestamp;
        return reading;
    }
}
