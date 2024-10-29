using System;

namespace AIMCore.Parsers;

public interface IMeasurementParserFactory
{
    List<IMeasurementParser> GetParsers();

    IMeasurementParser GetParser(string parserType);
}
