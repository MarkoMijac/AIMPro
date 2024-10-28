using System;

namespace AIMCore.Parsers;

public interface IMeasurementParserFactory
{
    IMeasurementParser Create(string parserType);
}
