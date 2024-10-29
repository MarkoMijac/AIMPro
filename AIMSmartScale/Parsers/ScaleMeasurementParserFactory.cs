using System;
using AIMCore.Parsers;

namespace AIMSmartScale.Parsers;

public class ScaleMeasurementParserFactory : IMeasurementParserFactory
{
    private List<IMeasurementParser> _parsers;

    public ScaleMeasurementParserFactory()
    {
        _parsers = new List<IMeasurementParser> { new ScaleMeasurementParser() };
    }

    public List<IMeasurementParser> GetParsers()
    {
        return _parsers;
    }
}
