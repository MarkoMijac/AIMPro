using System;
using System.Runtime.InteropServices.Marshalling;
using System.Runtime.Intrinsics.X86;
using AIMCore;
using AIMCore.Configurations;
using AIMCore.Exceptions;
using AIMCore.Sensors;
using AIMCore.StateMachine;
using FakeItEasy;
using FakeItEasy.Sdk;

namespace AIMCoreUnitTests;

public class AIMTests
{
    public AIMTests()
    {
        
    }

    private Configuration CreateNoDataConfiguration()
    {
        var configuration = new Configuration("No Data Configuration");
        configuration.AIModel = A.Fake<IAIModel>();
        configuration.BaseInstrument = A.Fake<ISensor>();
        configuration.Sensors.Add(A.Fake<ISensor>());
        configuration.Sensors.Add(A.Fake<ISensor>());

        return configuration;
    }

    private Configuration CreateInvalidConfiguration()
    {
        return new Configuration("Invalid Configuration");
    }

    private Configuration CreateValidConfiguration()
    {
        var configuration = new Configuration("Test Configuration");
        configuration.AIModel = A.Fake<IAIModel>();
        configuration.BaseInstrument = A.Fake<ISensor>();
        configuration.Sensors.Add(A.Fake<ISensor>());
        configuration.Sensors.Add(A.Fake<ISensor>());

        var baseInstrument = configuration.BaseInstrument;
        var baseInstrumentReading = new SensorReading("Scale reading");
        baseInstrumentReading.AddMeasurement("weight", 1);
        baseInstrumentReading.TimeStamp = DateTime.Now;

        A.CallTo(() => baseInstrument.Read()).Returns(baseInstrumentReading);
        A.CallTo(() => baseInstrument.ReadAsync()).Returns(baseInstrumentReading);

        var sensor1 = configuration.Sensors[0];
        var sensorOneReading = new SensorReading("Sensor 1 reading");
        sensorOneReading.AddMeasurement("vibrationRate", 2);
        sensorOneReading.TimeStamp = DateTime.Now;

        A.CallTo(() => sensor1.Read()).Returns(sensorOneReading);
        A.CallTo(() => sensor1.ReadAsync()).Returns(sensorOneReading);

        var sensor2 = configuration.Sensors[1];
        var sensorTwoReading = new SensorReading("Sensor 2 reading");
        sensorTwoReading.AddMeasurement("incline", 3);
        sensorTwoReading.TimeStamp = DateTime.Now;

        A.CallTo(() => sensor2.Read()).Returns(sensorTwoReading);
        A.CallTo(() => sensor2.ReadAsync()).Returns(sensorTwoReading);

        return configuration;
    }

    [Fact]
    public void Constructor_StatusIsNoConfiguration()
    {
        // Arrange
        var aim = new AIM();

        // Act & Assert
        Assert.Equal(AIMStatus.NoConfiguration, aim.Status);
    }

    [Fact]
    public void Constructor_ConfigurationIsNull()
    {
        // Arrange
        var aim = new AIM();

        // Act & Assert
        Assert.Null(aim.Configuration);
    }

    [Fact]
    public void LoadConfiguration_GivenConfigurationIsNullAndNewConfigurationIsNull_ThrowsAIMException()
    {
        // Arrange
        var aim = new AIM();

        // Act & Assert
        var exception = Assert.Throws<AIMNoConfigurationProvidedException>(() => aim.LoadConfiguration(null));
        Assert.Equal(AIMStatus.NoConfiguration, aim.Status);
    }

    [Fact]
    public void LoadConfiguration_GivenConfigurationIsNullAndNewConfigurationIsNull_StatusIsNoConfiguration()
    {
        // Arrange
        var aim = new AIM();

        // Act & Assert
        Assert.Equal(AIMStatus.NoConfiguration, aim.Status);
    }

    [Fact]
    public void LoadConfiguration_GivenConfigurationIsNullAndNewConfigurationIsNotValid_ThrowsAIMException()
    {
        // Arrange
        var aim = new AIM();
        var invalidConfiguration = CreateInvalidConfiguration();

        // Act & Assert
        var exception = Assert.Throws<AIMInvalidConfigurationException>(() => aim.LoadConfiguration(invalidConfiguration));
        Assert.Equal(AIMStatus.NoConfiguration, aim.Status);
        Assert.Null(aim.Configuration);
    }

    [Fact]
    public void LoadConfiguration_GivenConfigurationIsNullAndNewConfigurationIsValid_LoadsNewConfiguration()
    {
        // Arrange
        var aim = new AIM();
        var validConfiguration = CreateValidConfiguration();

        // Act
        aim.LoadConfiguration(validConfiguration);

        // Assert
        Assert.Equal(validConfiguration, aim.Configuration);
        Assert.True(aim.Status == AIMStatus.Ready);
    }

    [Fact]
    public void LoadConfiguration_GivenConfigurationIsNotNullAndNewConfigurationIsNull_ThrowsAIMException()
    {
        // Arrange
        var aim = new AIM();
        var validConfiguration = CreateValidConfiguration();
        aim.LoadConfiguration(validConfiguration);

        // Act & Assert
        var exception = Assert.Throws<AIMNoConfigurationProvidedException>(() => aim.LoadConfiguration(null));
        Assert.Equal(validConfiguration, aim.Configuration);
        Assert.Equal(AIMStatus.Ready, aim.Status);
    }

    [Fact]
    public void LoadConfiguration_GivenConfigurationIsNotNullAndNewConfigurationIsNotValid_ThrowsAIMException()
    {
        // Arrange
        var aim = new AIM();
        var validConfiguration = CreateValidConfiguration();
        aim.LoadConfiguration(validConfiguration);

        var invalidConfiguration = CreateInvalidConfiguration();

        // Act & Assert
        var exception = Assert.Throws<AIMInvalidConfigurationException>(() => aim.LoadConfiguration(invalidConfiguration));
        Assert.Equal(validConfiguration, aim.Configuration);
        Assert.Equal(AIMStatus.Ready, aim.Status);
    }

    [Fact]
    public void LoadConfiguration_GivenConfigurationIsNotNullAndNewConfigurationIsValid_LoadsNewConfiguration()
    {
        // Arrange
        var aim = new AIM();
        var initialConfiguration = CreateValidConfiguration();
        aim.LoadConfiguration(initialConfiguration);

        var newConfiguration = CreateValidConfiguration();

        // Act
        aim.LoadConfiguration(newConfiguration);

        // Assert
        Assert.Equal(newConfiguration, aim.Configuration);
        Assert.True(aim.Status == AIMStatus.Ready);
    }

    [Fact]
    public void Connect_GivenConfigurationLoaded_StatusIsReady()
    {
        // Arrange
        var aim = new AIM();
        var validConfiguration = CreateValidConfiguration();
        aim.LoadConfiguration(validConfiguration);

        // Act
        aim.Connect();

        // Assert
        Assert.Equal(AIMStatus.Ready, aim.Status);
    }

    [Fact]
    public void Connect_GivenConfigurationNotLoaded_ThrowsAIMException()
    {
        // Arrange
        var aim = new AIM();

        // Act & Assert
        var exception = Assert.Throws<AIMNoConfigurationProvidedException>(() => aim.Connect());
    }

    [Fact]
    public void Connect_GivenAlreadyConnected_DoesNotAttemptConnecting()
    {
        // Arrange
        var aim = new AIM();
        var validConfiguration = CreateValidConfiguration();
        aim.LoadConfiguration(validConfiguration);

        var baseInstrument = validConfiguration.BaseInstrument;
        A.CallTo(() => baseInstrument.IsConnected).Returns(true);
        var sensor1 = validConfiguration.Sensors[0];
        A.CallTo(() => sensor1.IsConnected).Returns(true);  
        var sensor2 = validConfiguration.Sensors[1];
        A.CallTo(() => sensor2.IsConnected).Returns(true);
        // Act
        aim.Connect();

        //Assert
        A.CallTo(() => baseInstrument.Connect()).MustNotHaveHappened();
        A.CallTo(() => sensor1.Connect()).MustNotHaveHappened();
        A.CallTo(() => sensor2.Connect()).MustNotHaveHappened();
    }

    [Fact]
    public void Connect_GivenNotAlreadyConnected_AttemptsConnecting()
    {
        // Arrange
        var aim = new AIM();
        var validConfiguration = CreateValidConfiguration();
        aim.LoadConfiguration(validConfiguration);

        var baseInstrument = validConfiguration.BaseInstrument;
        A.CallTo(() => baseInstrument.IsConnected).Returns(false);
        var sensor1 = validConfiguration.Sensors[0];
        A.CallTo(() => sensor1.IsConnected).Returns(false);  
        var sensor2 = validConfiguration.Sensors[1];
        A.CallTo(() => sensor2.IsConnected).Returns(false);
        // Act
        aim.Connect();

        //Assert
        A.CallTo(() => baseInstrument.Connect()).MustHaveHappenedOnceExactly();
        A.CallTo(() => sensor1.Connect()).MustHaveHappenedOnceExactly();
        A.CallTo(() => sensor2.Connect()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async void ConnectAsync_GivenConfigurationLoaded_StatusIsReady()
    {
        // Arrange
        var aim = new AIM();
        var validConfiguration = CreateValidConfiguration();
        aim.LoadConfiguration(validConfiguration);

        // Act
        await aim.ConnectAsync();

        // Assert
        Assert.Equal(AIMStatus.Ready, aim.Status);
    }

    [Fact]
    public async void ConnectAsync_GivenConfigurationNotLoaded_ThrowsAIMException()
    {
        // Arrange
        var aim = new AIM();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<AIMNoConfigurationProvidedException>(() => aim.ConnectAsync());
    }

    [Fact]
    public async void ConnectAsync_GivenAlreadyConnected_DoesNotAttemptConnecting()
    {
        // Arrange
        var aim = new AIM();
        var validConfiguration = CreateValidConfiguration();
        aim.LoadConfiguration(validConfiguration);

        var baseInstrument = validConfiguration.BaseInstrument;
        A.CallTo(() => baseInstrument.IsConnected).Returns(true);
        var sensor1 = validConfiguration.Sensors[0];
        A.CallTo(() => sensor1.IsConnected).Returns(true);  
        var sensor2 = validConfiguration.Sensors[1];
        A.CallTo(() => sensor2.IsConnected).Returns(true);
        // Act
        await aim.ConnectAsync();

        //Assert
        A.CallTo(() => baseInstrument.ConnectAsync()).MustNotHaveHappened();
        A.CallTo(() => sensor1.ConnectAsync()).MustNotHaveHappened();
        A.CallTo(() => sensor2.ConnectAsync()).MustNotHaveHappened();
    }

    [Fact]
    public async void ConnectAsync_GivenNotAlreadyConnected_AttemptsConnecting()
    {
        // Arrange
        var aim = new AIM();
        var validConfiguration = CreateValidConfiguration();
        aim.LoadConfiguration(validConfiguration);

        var baseInstrument = validConfiguration.BaseInstrument;
        A.CallTo(() => baseInstrument.IsConnected).Returns(false);
        var sensor1 = validConfiguration.Sensors[0];
        A.CallTo(() => sensor1.IsConnected).Returns(false);  
        var sensor2 = validConfiguration.Sensors[1];
        A.CallTo(() => sensor2.IsConnected).Returns(false);
        // Act
        await aim.ConnectAsync();

        //Assert
        A.CallTo(() => baseInstrument.ConnectAsync()).MustHaveHappenedOnceExactly();
        A.CallTo(() => sensor1.ConnectAsync()).MustHaveHappenedOnceExactly();
        A.CallTo(() => sensor2.ConnectAsync()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void Predict_GivenSessionIsNull_ThrowsException()
    {
        //Arrange
        var aim = new AIM();

        //Act & Assert
        var exception = Assert.Throws<AIMNoSessionDataAvailableException>(() => aim.Predict(null));
    }

    [Fact]
    public async Task PredictAsync_GivenSessionIsNull_ThrowsException()
    {
        //Arrange
        var aim = new AIM();

        //Act & Assert
        var exception = await Assert.ThrowsAsync<AIMNoSessionDataAvailableException>(() => aim.PredictAsync(null));
    }

    [Fact]
    public void Predict_GivenSessionEndedAndSessionIsInvalid_ThrowsException()
    {
        // Arrange
        var aim = new AIM();
        var emptyConfiguration = CreateNoDataConfiguration();
        aim.LoadConfiguration(emptyConfiguration);
        var session = aim.GetData();

        // Act & Assert
        var exception = Assert.Throws<AIMInvalidSessionDataException>(() => aim.Predict(session));
    }

    [Fact]
    public async Task PredictAsync_GivenSessionEndedAndSessionIsInvalid_ThrowsException()
    {
        // Arrange
        var aim = new AIM();
        var emptyConfiguration = CreateNoDataConfiguration();
        aim.LoadConfiguration(emptyConfiguration);
        var session = await aim.GetDataAsync();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<AIMInvalidSessionDataException>(() => aim.PredictAsync(session));
    }

    [Fact]
    public void Predict_SessionDataIsValid_InvokesPrediction()
    {
        // Arrange
        var aim = new AIM();
        var validConfiguration = CreateValidConfiguration();
        aim.LoadConfiguration(validConfiguration);
        var session = aim.GetData();

        var aiModel = validConfiguration.AIModel;

        // Act
        aim.Predict(session);

        // Assert
        A.CallTo(() => aiModel.Predict(session)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task PredictAsync_SessionDataIsValid_InvokesPrediction()
    {
        // Arrange
        var aim = new AIM();
        var validConfiguration = CreateValidConfiguration();
        aim.LoadConfiguration(validConfiguration);
        var session = await aim.GetDataAsync();

        var aiModel = validConfiguration.AIModel;

        // Act
        await aim.PredictAsync(session);

        // Assert
        A.CallTo(() => aiModel.PredictAsync(session)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void Predict_GivenSessionDataIsValid_ReturnsPrediction()
    {
        // Arrange
        var aim = new AIM();
        var validConfiguration = CreateValidConfiguration();
        aim.LoadConfiguration(validConfiguration);

        var session = aim.GetData();

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
        var session = await aim.GetDataAsync();

        // Act
        var prediction = await aim.PredictAsync(session);

        // Assert
        Assert.NotNull(prediction);
    }
}
