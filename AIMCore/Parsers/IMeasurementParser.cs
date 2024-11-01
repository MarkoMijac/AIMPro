using System;

namespace AIMCore.Parsers;

public interface IMeasurementParser
{
    public string Name {get;}
    Measurement Parse(string data);
}
