using System.Globalization;
using AIMCore;
using AIMCore.Converters;

namespace AIMSmartScale.Converters;

public class GyroscopeReadingConverter : ReadingConverterBase<string>
{
    private const float Alpha = 0.98f;
    private float roll = 0.0f;
    private float pitch = 0.0f;
    private bool initialized = false;

    public GyroscopeReadingConverter()
    {
        Name = "Gyroscope converter";
    }

    public override SensorReading Convert(string input)
    {
        Dictionary<string, string> data = ParseSensorData(input);

        DateTime.TryParse(data["time"].Trim(), out var timestamp);
        TryParseFloat(data, "accelX", out float accelX);
        TryParseFloat(data, "accelY", out float accelY);
        TryParseFloat(data, "accelZ", out float accelZ);
        TryParseFloat(data, "gyroX", out float gyroX);
        TryParseFloat(data, "gyroY", out float gyroY);
        TryParseFloat(data, "gyroZ", out float gyroZ);
        TryParseFloat(data, "temp", out float temp);

        Update(accelX, accelY, accelZ, gyroX, gyroY, Alpha);

        var reading = new SensorReading("gyroscope");
        reading.AddMeasurement("roll", roll);
        reading.AddMeasurement("pitch", pitch);
        reading.TimeStamp = timestamp;
        return reading;
    }

    public void Update(float accelX, float accelY, float accelZ, float gyroX, float gyroY, float dt)
    {
        float accelRoll = (float)(Math.Atan2(accelY, MathF.Sqrt(accelX * accelX + accelZ * accelZ)) * (180.0 / Math.PI));
        float accelPitch = (float)(Math.Atan2(-accelX, MathF.Sqrt(accelY * accelY + accelZ * accelZ)) * (180.0 / Math.PI));

        if (!initialized)
        {
            roll = accelRoll;
            pitch = accelPitch;
            initialized = true;
        }

        float gyroRoll = roll + (gyroX * dt);
        float gyroPitch = pitch + (gyroY * dt);

        roll = Alpha * gyroRoll + (1 - Alpha) * accelRoll;
        pitch = Alpha * gyroPitch + (1 - Alpha) * accelPitch;
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