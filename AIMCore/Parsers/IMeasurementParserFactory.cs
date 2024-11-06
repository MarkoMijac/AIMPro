using System;

namespace AIMCore.Parsers;

public interface IMeasurementParserFactory<T>
{
    List<IMeasurementParser<T>> GetParsers();
}
