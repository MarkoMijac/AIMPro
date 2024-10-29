using System;

namespace AIMCore.Parsers;

public interface IMeasurementParserFactory
{
    List<IMeasurementParser> GetParsers();
}
