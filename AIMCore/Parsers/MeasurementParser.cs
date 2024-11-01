using System;

namespace AIMCore.Parsers;

public abstract class MeasurementParser : IMeasurementParser
{
    public string Name {get; protected set;}

    public abstract Measurement Parse(string data);

    public override string ToString()
    {
        return Name;
    }
}
