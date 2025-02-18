using AIMCore.Converters;

namespace AIMSmartScale.Converters;

public class ScaleReadingConverterFactory : IReadingConverterFactory<string>
{
    private List<IReadingConverter<string>> _converters;

    public ScaleReadingConverterFactory()
    {
        _converters = [new ScaleReadingConverter(), new EnviromentalReadingConverter(), new GyroscopeReadingConverter()];
    }

    public List<IReadingConverter<string>> GetConverters()
    {
        return _converters;
    }
}
