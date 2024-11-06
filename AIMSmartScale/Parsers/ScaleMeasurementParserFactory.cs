using System;
using AIMCore.Parsers;

namespace AIMSmartScale.Parsers;

public class ScaleMeasurementParserFactory : IMeasurementParserFactory<string>
{
    private List<IMeasurementParser<string>> _parsers;

    public ScaleMeasurementParserFactory()
    {
        _parsers = new List<IMeasurementParser<string>> { new ScaleMeasurementParser() };
    }

    public List<IMeasurementParser<string>> GetParsers()
    {
        return _parsers;
    }
}
