using System;
using System.Threading.Tasks;
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
        var scaleConverter = new ScaleReadingConverter();
        var scale = new StringSensor("WeightScale", "GET_WEIGHT", scaleStrategy, scaleConverter);
        configuration.BaseInstrument = scale;
        
        //Setting up sensors
        //Setting up sensor 1
        var vibrationStrategy = new VibrationCommunicationStrategy();
        var vibrationConverter = new VibrationReadingConverter();
        var vibrationSensor = new StringSensor("VibrationSensor", "GET_VIBR", vibrationStrategy, vibrationConverter);
        configuration.Sensors.Add(vibrationSensor);

        //Setting up sensor 2
        var gyroscopeStrategy = new GyroscopeCommunicationStrategy();
        var gyroscopeConverter = new GyroscopeReadingConverter();
        var gyroscopeSensor = new StringSensor("GyroscopeSensor", "GET_INCLINE", gyroscopeStrategy, gyroscopeConverter);
        configuration.Sensors.Add(gyroscopeSensor);

        return configuration;
    }

    [Fact]
    public void Predict_GivenSessionDataIsValid_ReturnsPrediction()
    {
        // Arrange
        var aim = new AIM();
        var validConfiguration = CreateValidConfiguration();
        aim.LoadConfiguration(validConfiguration);
        aim.Connect();
        var session = aim.GetData();
        aim.Disconnect();

        // Act
        var prediction = aim.Predict(session);

        // Assert
        Assert.NotNull(prediction);
    }

    [Fact]
    public async Task PredictAsync_GivenSessionDataIsValid_ReturnsPrediction()
    {
        // Arrange
        var aim = new AIM();
        var validConfiguration = CreateValidConfiguration();
        aim.LoadConfiguration(validConfiguration);

        await aim.ConnectAsync();
        var session = await aim.GetDataAsync();
        await aim.DisconnectAsync();

        // Act
        var prediction = await aim.PredictAsync(session);

        // Assert
        Assert.NotNull(prediction);
    }
}
