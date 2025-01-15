using System;
using System.IO.Ports;
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
        var measurementParserFactory = new ScaleMeasurementParserFactory();

        var serialPort = new SerialPort()
        {
            PortName = "/dev/ttyUSB0",
            BaudRate = 9600,
            Parity = Parity.None,
            DataBits = 8,
            StopBits = StopBits.One,
            Handshake = Handshake.None,
            RtsEnable = true
        };

        var portWrapper = new SerialPortWrapper(serialPort);

        var uartStrategy = new UARTCommunication(portWrapper);
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
