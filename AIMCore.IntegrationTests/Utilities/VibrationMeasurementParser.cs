using System;
using System.Globalization;
using AIMCore.Parsers;

namespace AIMCore.IntegrationTests.Utilities;

public class VibrationMeasurementParser : MeasurementParser<string>
{
    public VibrationMeasurementParser()
    {
        Name = "Vibration Parser";
    }

    public override Measurement Parse(string data)
    {
        var dataPoints = data.Split(';');

        DateTime.TryParse(dataPoints[0].Trim(), out var timestamp);
        float.TryParse(dataPoints[1].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out var vibration);

        var measurement = new Measurement("vibration_rate", vibration, timestamp);
        return measurement;
    }
}
