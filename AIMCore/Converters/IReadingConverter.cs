using System;

namespace AIMCore.Converters;

public interface IReadingConverter<T>
{
    public string Name {get;}
    SensorReading Convert(T data);
}
