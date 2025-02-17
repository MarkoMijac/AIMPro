using System;

namespace AIMCore.Parsers;

public abstract class MeasurementParser<T> : IMeasurementParser<T>
{
    public string Name {get; protected set;}

    public abstract SensorReading Parse(T data);

    public override string ToString()
    {
        return Name;
    }
}
