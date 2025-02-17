using System;
using AIMCore.Converters;

namespace AIMSmartScale.Converters;

public class ScaleReadingConverterFactory : IReadingConverterFactory<string>
{
    private List<IReadingConverter<string>> _converters;

    public ScaleReadingConverterFactory()
    {
        _converters = new List<IReadingConverter<string>> { new ScaleReadingConverter() };
    }

    public List<IReadingConverter<string>> GetConverters()
    {
        return _converters;
    }
}
