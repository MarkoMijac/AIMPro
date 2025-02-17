using System;
using AIMCore;
using AIMCore.Converters;

namespace AIMCoreUnitTests.Converters;

public class TestReadingConverter : ReadingConverterBase<string>
{
    public TestReadingConverter(string name)
    {
        Name = name;
    }

    public override SensorReading Convert(string data)
    {
        var reading = new SensorReading("Test source");
        reading.AddMeasurement("Test data", 2);
        reading.TimeStamp = DateTime.Now;
        return reading;
    }

}
