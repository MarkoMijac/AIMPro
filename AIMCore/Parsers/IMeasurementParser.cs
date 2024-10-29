using System;

namespace AIMCore.Parsers;

public interface IMeasurementParser
{
    public string Name {get;}
    MeasurementData Parse(string data);
}
