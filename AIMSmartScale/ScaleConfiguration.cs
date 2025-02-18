
using AIMCore;
using AIMCore.Configurations;
using AIMCore.Sensors;
using AIMSmartScale.CommunicationStrategies;
using AIMSmartScale.Converters;

namespace AIMSmartScale.Sensors;

public class ScaleConfiguration
{
    public Configuration GetDefault()
    {
        var readingConverterFactory = new ScaleReadingConverterFactory();

        var scaleConverter = readingConverterFactory.GetConverters().OfType<ScaleReadingConverter>().First();
        var environmentSensorConverter = readingConverterFactory.GetConverters().OfType<EnviromentalReadingConverter>().First();
        var gyroscopeConverter = readingConverterFactory.GetConverters().OfType<GyroscopeReadingConverter>().First();

        var instrument = new Scale("SCALE", new Hx711CommunicationStrategy(), scaleConverter);

        var sensors = new List<ISensor>();
        var gyroscope = new Gyroscope("GYROSCOPE", new Mpu6050CommunicationStrategy(), gyroscopeConverter);
        var enviromentalSensor = new EnvironmentalSensor("ENVIRONMENTAL", new DhtCommunicationStrategy(), environmentSensorConverter);
        //var vibrationsensor = new Accelerometer("ACCELEROMETER", "GET_VIBR", uartStrategy, converter);
        sensors.Add(gyroscope);
        sensors.Add(enviromentalSensor);

        //sensors.Add(vibrationsensor);

        var aiModel = new AIModel("ScaleModel", @"/home/ubuntustudio/Documents/AimFiles/Models/ScaleModel.onnx");

        var builder = new ConfigurationBuilder();
        builder.SetName("ScaleConfig")
        .SetInstrument(instrument)
        .SetSensors(sensors)
        .SetAIModel(aiModel);

        return builder.Build();
    }
}
