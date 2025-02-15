using System;
using AIMCore.Communication;
using AIMCore.Configurations;
using AIMCore.IntegrationTests.Utilities;
using AIMCore.Sensors;
using AIMCore.StateMachine;

namespace AIMCore.IntegrationTests;

public class AIMTests
{
    private string _validModelPath = "/home/ubuntustudio/Documents/AIMPro/AIMCore.IntegrationTests/TestData/ValidModel.onnx";

    private Configuration CreateValidConfiguration()
    {
        var configuration = new Configuration("TestConfiguration");

        //Setting up AI model
        var model = new AIModel("TestModel", _validModelPath);
        configuration.AIModel = model;

        //Setting up base instrument
        var scaleStrategy = new ScaleCommunicationStrategy();
        var scaleParser = new ScaleMeasurementParser();
        var scale = new StringSensor("WeightScale", "GET_WEIGHT", scaleStrategy, scaleParser);
        configuration.BaseInstrument = scale;
        
        //Setting up sensors
        //Setting up sensor 1
        var vibrationStrategy = new VibrationCommunicationStrategy();
        var vibrationParser = new VibrationMeasurementParser();
        var vibrationSensor = new StringSensor("VibrationSensor", "GET_VIBR", vibrationStrategy, vibrationParser);
        configuration.Sensors.Add(vibrationSensor);

        //Setting up sensor 2
        var gyroscopeStrategy = new GyroscopeCommunicationStrategy();
        var gyroscopeParser = new GyroscopeMeasurementParser();
        var gyroscopeSensor = new StringSensor("GyroscopeSensor", "GET_INCLINE", gyroscopeStrategy, gyroscopeParser);
        configuration.Sensors.Add(gyroscopeSensor);

        return configuration;
    }

    [Fact]
    public void StartMeasurementSession_GivenConfigurationLoaded_AllSensorsConnecting()
    {
        // Arrange
        var aim = new AIM();
        var validConfiguration = CreateValidConfiguration();
        aim.LoadConfiguration(validConfiguration);

        var baseInstrument = validConfiguration.BaseInstrument;
        var sensor1 = validConfiguration.Sensors[0];
        var sensor2 = validConfiguration.Sensors[1];

        // Act
        aim.StartMeasurementSession();

        //Assert
        Assert.True(baseInstrument.IsConnected);
        Assert.True(sensor1.IsConnected);
        Assert.True(sensor2.IsConnected);
    }

    [Fact]
    public void StartMeasurementSession_GivenConfigurationLoaded_StatusIsMeasuring()
    {
        // Arrange
        var aim = new AIM();
        var validConfiguration = CreateValidConfiguration();
        aim.LoadConfiguration(validConfiguration);

        // Act
        aim.StartMeasurementSession();

        // Assert
        Assert.Equal(AIMStatus.Measuring, aim.Status);
    }

    [Fact]
    public void EndMeasurementSession_GivenSessionStarted_AllSensorsDisconnected()
    {
        // Arrange
        var aim = new AIM();
        var validConfiguration = CreateValidConfiguration();
        aim.LoadConfiguration(validConfiguration);

        var baseInstrument = validConfiguration.BaseInstrument;
        var sensor1 = validConfiguration.Sensors[0];
        var sensor2 = validConfiguration.Sensors[1];

        aim.StartMeasurementSession();

        // Act
        aim.EndMeasurementSession();

        //Assert
        Assert.False(baseInstrument.IsConnected);
        Assert.False(sensor1.IsConnected);
        Assert.False(sensor2.IsConnected);
    }

    [Fact]
    public void EndMeasurementSession_GivenSessionStarted_ReturnsMeasurementSession()
    {
        // Arrange
        var aim = new AIM();
        var validConfiguration = CreateValidConfiguration();
        aim.LoadConfiguration(validConfiguration);
        aim.StartMeasurementSession();

        // Act
        var session = aim.EndMeasurementSession();

        // Assert
        Assert.NotNull(session);
        Assert.NotNull(session.BaseInstrumentData);
        Assert.True(session.BaseInstrumentData!= null);
        Assert.InRange(session.BaseInstrumentData.Value, 19, 22);

        Assert.NotNull(session.SensorDataSeries);
        Assert.True(session.SensorDataSeries.Count == 2);
        Assert.True(session.SensorDataSeries[0] != null);
        Assert.InRange(session.SensorDataSeries[0].Value, 0, 2);
        Assert.True(session.SensorDataSeries[1]!= null);
        Assert.InRange(session.SensorDataSeries[1].Value, 0, 30);
    }

    [Fact]
    public void Predict_GivenSessionDataIsValid_ReturnsPrediction()
    {
        // Arrange
        var aim = new AIM();
        var validConfiguration = CreateValidConfiguration();
        aim.LoadConfiguration(validConfiguration);
        aim.StartMeasurementSession();
        var session = aim.EndMeasurementSession();

        // Act
        var prediction = aim.Predict(session);

        // Assert
        Assert.NotNull(prediction);
    }
}
