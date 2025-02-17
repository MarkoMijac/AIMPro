using System;
namespace AIMCore.Converters;

public interface IReadingConverterFactory<T>
{
    List<IReadingConverter<T>> GetConverters();
}
