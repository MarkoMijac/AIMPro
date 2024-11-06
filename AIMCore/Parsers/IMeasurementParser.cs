using System;

namespace AIMCore.Parsers;

public interface IMeasurementParser<T>
{
    public string Name {get;}
    TimeSeriesData Parse(T data);
}
