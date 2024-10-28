using System;

namespace AIMCore.Parsers;

public interface IMeasurementParser
{
    MeasurementData Parse(string data);
}
