using System;
using System.Globalization;
using AIMCore.Parsers;

namespace AIMCore.IntegrationTests.Utilities;

public class GyroscopeMeasurementParser : MeasurementParser<string>
{
    public GyroscopeMeasurementParser()
    {
        Name = "Gyroscope Parser";
    }

    public override TimeSeriesData Parse(string data)
    {
        var timeSeries = new TimeSeriesData("InclineMeasurement");
        var dataPoints = data.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var point in dataPoints)
        {
            var parts = point.Split(';');
            if (parts.Length == 2 && DateTime.TryParse(parts[0].Trim(), out var timestamp) && float.TryParse(parts[1].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out var incline))
            {
                timeSeries.AddMeasurement(new Measurement(incline, timestamp));
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
