using System;
using System.Globalization;
using AIMCore.Parsers;

namespace AIMCore.IntegrationTests.Utilities;

public class ScaleMeasurementParser : MeasurementParser<string>
{
    public ScaleMeasurementParser()
    {
        Name = "Scale Parser";
    }

    public override Measurement Parse(string data)
    {
        var dataPoints = data.Split(';');

        DateTime.TryParse(dataPoints[0].Trim(), out var timestamp);
        float.TryParse(dataPoints[1].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out var weight);

        var measurement = new Measurement("measured_weight", weight, timestamp);
        return measurement;
    }
}
