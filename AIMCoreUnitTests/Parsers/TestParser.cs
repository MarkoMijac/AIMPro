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

    public override Measurement Parse(string data)
    {
        return new Measurement("Test source", 2, DateTime.Now);
    }

}
