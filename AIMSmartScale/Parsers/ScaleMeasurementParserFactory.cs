using System;
using AIMCore.Parsers;

namespace AIMSmartScale.Parsers;

public class ScaleMeasurementParserFactory : IMeasurementParserFactory
{
    public List<IMeasurementParser> GetParsers()
    {
        return new List<IMeasurementParser> { new ScaleMeasurementParser() };
    }

    public IMeasurementParser GetParser(string parserName)
    {
        return new ScaleMeasurementParser();
    }
}
