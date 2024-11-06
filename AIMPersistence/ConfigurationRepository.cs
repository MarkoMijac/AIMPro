using System;
using AIMCore;
using AIMCore.Communication;
using AIMCore.Configurations;
using AIMCore.Sensors;
using AIMSmartScale.Parsers;
using AIMSmartScale.Sensors;

namespace AIMPersistence;

public class ConfigurationRepository
{
    public Configuration GetDefaultConfiguration()
    {
        var comStrategyFactory = new TextBaseCommunicationFactory();
        var measurementParserFactory = new ScaleMeasurementParserFactory();

        var uartStrategy = comStrategyFactory.GetCommunicationStrategies().OfType<UARTCommunication>().First();
        var parser = measurementParserFactory.GetParsers().OfType<ScaleMeasurementParser>().First();

        var instrument = new Scale("SCALE", "GET_WEIGHT", uartStrategy, parser);

        var sensors = new List<ISensor>();
        var gyroscope = new Gyroscope("GYROSCOPE", "GET_TILT", uartStrategy, parser);
        var vibrationsensor = new Accelerometer("ACCELEROMETER", "GET_VIBR", uartStrategy, parser);
        sensors.Add(gyroscope);
        sensors.Add(vibrationsensor);

        var aiModel = new AIModel("ScaleModel", @"/home/ubuntustudio/Documents/AimFiles/Models/ScaleModel.onnx");

        var builder = new ConfigurationBuilder();
        builder.SetName("ScaleConfig")
        .SetInstrument(instrument)
        .SetSensors(sensors)
        .SetAIModel(aiModel);

        return builder.Build();
    }
}
