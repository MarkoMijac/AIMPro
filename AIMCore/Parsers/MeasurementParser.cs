using System;

namespace AIMCore.Parsers;

public abstract class MeasurementParser<T> : IMeasurementParser<T>
{
    public string Name {get; protected set;}

    public abstract TimeSeriesData Parse(T data);

    public override string ToString()
    {
        return Name;
    }
}
