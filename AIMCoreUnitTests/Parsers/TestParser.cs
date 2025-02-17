using System;
using AIMCore;
using AIMCore.Parsers;

namespace AIMCoreUnitTests.Parsers;

public class TestParser : MeasurementParser<string>
{
    public TestParser(string name)
    {
        Name = name;
    }

    public override SensorReading Parse(string data)
    {
        var reading = new SensorReading("Test source");
        reading.AddMeasurement("Test data", 2);
        reading.TimeStamp = DateTime.Now;
        return reading;
    }

}
