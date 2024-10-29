using System;

namespace AIMCore.Parsers;

public abstract class MeasurementParser : IMeasurementParser
{
    public string Name {get; protected set;}

    public abstract MeasurementData Parse(string data);

    public override string ToString()
    {
        return Name;
    }
}
