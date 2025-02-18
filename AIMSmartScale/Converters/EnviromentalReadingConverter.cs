using System.Globalization;
using AIMCore;
using AIMCore.Converters;

namespace AIMSmartScale.Converters;

public class EnviromentalReadingConverter : ReadingConverterBase<string>
{
    public EnviromentalReadingConverter()
    {
        Name = "Enviromental converter";
    }

    public override SensorReading Convert(string input)
    {
        var data = ParseSensorData(input);

        DateTime.TryParse(data["time"].Trim(), out var timestamp);
        TryParseFloat(data, "temp", out float temp);
        TryParseFloat(data, "humidity", out float humidity);

        var reading = new SensorReading("environment");
        reading.AddMeasurement("temperature", temp);
        reading.AddMeasurement("humidity", humidity);
        reading.TimeStamp = timestamp;
        return reading;
    }

    private static Dictionary<string, string> ParseSensorData(string input)
    {
        var data = new Dictionary<string, string>();
        string[] parts = input.Split(';');

        foreach (string part in parts)
        {
            string[] keyValue = part.Split(':');
            if (keyValue.Length == 2)
            {
                data[keyValue[0].Trim()] = keyValue[1].Trim();
            }
        }

        return data;
    }

    private static bool TryParseFloat(Dictionary<string, string> data, string key, out float value)
    {
        if (data.TryGetValue(key, out string strValue))
        {
            return float.TryParse(strValue, NumberStyles.Float, CultureInfo.InvariantCulture, out value);
        }

        value = 0f;
        return false;
    }
}