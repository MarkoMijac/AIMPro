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

    public override TimeSeriesData Parse(string data)
    {
        return new TimeSeriesData("Test source");
    }

}
