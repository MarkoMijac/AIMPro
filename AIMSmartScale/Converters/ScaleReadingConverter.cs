using System;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using AIMCore;
using AIMCore.Converters;

namespace AIMSmartScale.Converters;

public class ScaleReadingConverter : ReadingConverterBase<string>
{
    public ScaleReadingConverter()
    {
        Name = "Scale converter";
    }

    public override SensorReading Convert(string data)
    {
        var dataPoints = data.Split(';');

        DateTime.TryParse(dataPoints[0].Trim(), out var timestamp);
        float.TryParse(dataPoints[1].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out var weight);

        var reading = new SensorReading("weight");
        reading.AddMeasurement("weight", weight);
        reading.TimeStamp = timestamp;
        return reading;
    }
}
