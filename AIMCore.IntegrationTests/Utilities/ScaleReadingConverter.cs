using System;
using System.Globalization;
using AIMCore.Converters;

namespace AIMCore.IntegrationTests.Utilities;

public class ScaleReadingConverter : ReadingConverterBase<string>
{
    public ScaleReadingConverter()
    {
        Name = "Scale Converter";
    }

    public override SensorReading Convert(string data)
    {
        var dataPoints = data.Split(';');

        DateTime.TryParse(dataPoints[0].Trim(), out var timestamp);
        float.TryParse(dataPoints[1].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out var weight);

        var reading = new SensorReading("measured_weight");
        reading.AddMeasurement("weight", weight);
        reading.TimeStamp = timestamp;
        return reading;
    }
}
