using System;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using AIMCore;
using AIMCore.Parsers;

namespace AIMSmartScale.Parsers;

public class ScaleMeasurementParser : MeasurementParser<string>
{
    public ScaleMeasurementParser()
    {
        Name = "Scale Parser";
    }

    public override TimeSeriesData Parse(string data)
    {
        var timeSeries = new TimeSeriesData("ScaleMeasurement");
        var dataPoints = data.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var point in dataPoints)
        {
            var parts = point.Split(';');
            if (parts.Length == 2 && DateTime.TryParse(parts[0].Trim(), out var timestamp) && float.TryParse(parts[1].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out var weight))
            {
                timeSeries.AddMeasurement(new Measurement(weight, timestamp));
            }
            else
            {
                throw new FormatException($"Invalid data format: '{point}'");
            }
        }

        // Return all measurements as a TimeSeriesData object
        return timeSeries;
    }
}
