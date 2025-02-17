using System;

namespace AIMCore.Converters;

public abstract class ReadingConverterBase<T> : IReadingConverter<T>
{
    public string Name {get; protected set;}

    public abstract SensorReading Convert(T data);

    public override string ToString()
    {
        return Name;
    }
}
