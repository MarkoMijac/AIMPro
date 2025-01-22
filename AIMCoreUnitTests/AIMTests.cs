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
        var baseInstrumentTestData = new TimeSeriesData("Base Instrument");
        baseInstrumentTestData.AddMeasurement(new Measurement(1, DateTime.Now));
        A.CallTo(() => baseInstrument.StopReading()).Returns(baseInstrumentTestData);

        var sensor1 = configuration.Sensors[0];
        var sensor1TestData = new TimeSeriesData("Sensor 1");
        sensor1TestData.AddMeasurement(new Measurement(2, DateTime.Now));
        A.CallTo(() => sensor1.StopReading()).Returns(sensor1TestData);

        var sensor2 = configuration.Sensors[1];
        var sensor2TestData = new TimeSeriesData("Sensor 2");
        sensor2TestData.AddMeasurement(new Measurement(3, DateTime.Now));
        A.CallTo(() => sensor2.StopReading()).Returns(sensor2TestData);

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
    public void StartMeasurementSession_GivenNoConfiguration_ThrowsException()
    {
        // Arrange
        var aim = new AIM();

        // Act & Assert
        var exception = Assert.Throws<AIMNoConfigurationProvidedException>(() => aim.StartMeasurementSession());
        Assert.Equal(AIMStatus.NoConfiguration, aim.Status);
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

        // Assert
        A.CallTo(() => baseInstrument.Connect()).MustHaveHappenedOnceExactly();
        A.CallTo(() => sensor1.Connect()).MustHaveHappenedOnceExactly();
        A.CallTo(() => sensor2.Connect()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void StartMeasurementSession_GivenErrorDuringConnecting_ThrowsException()
    {
        // Arrange
        var aim = new AIM();
        var validConfiguration = CreateValidConfiguration();
        aim.LoadConfiguration(validConfiguration);

        var baseInstrument = validConfiguration.BaseInstrument;
        A.CallTo(() => baseInstrument.Connect()).Throws<AIMSensorConnectionException>();

        // Act & Assert
        var exception = Assert.Throws<AIMSensorConnectionException>(() => aim.StartMeasurementSession());
    }

    [Fact]
    public void StartMeasurementSession_GivenConfigurationLoaded_AllSensorsStartReading()
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

        // Assert
        A.CallTo(() => baseInstrument.StartReading()).MustHaveHappenedOnceExactly();
        A.CallTo(() => sensor1.StartReading()).MustHaveHappenedOnceExactly();
        A.CallTo(() => sensor2.StartReading()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void StartMeasurementSession_GivenErrorDuringStartReading_ThrowsException()
    {
        // Arrange
        var aim = new AIM();
        var validConfiguration = CreateValidConfiguration();
        aim.LoadConfiguration(validConfiguration);

        var baseInstrument = validConfiguration.BaseInstrument;
        A.CallTo(() => baseInstrument.StartReading()).Throws<AIMSensorReadingException>();

        // Act & Assert
        var exception = Assert.Throws<AIMSensorReadingException>(() => aim.StartMeasurementSession());
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
    public void EndMeasurementSession_GivenConfigurationNotLoaded_ThrowsException()
    {
        // Arrange
        var aim = new AIM();

        // Act & Assert
        var exception = Assert.Throws<AIMNoConfigurationProvidedException>(() => aim.EndMeasurementSession());
        Assert.Equal(AIMStatus.NoConfiguration, aim.Status);
    }

    [Fact]
    public void EndMeasurementSession_GivenSessionNotStarted_ThrowsException()
    {
        // Arrange
        var aim = new AIM();
        var validConfiguration = CreateValidConfiguration();
        aim.LoadConfiguration(validConfiguration);

        // Act & Assert
        var exception = Assert.Throws<AIMMeasurementSessionNotStartedException>(() => aim.EndMeasurementSession());
        Assert.Equal(AIMStatus.Ready, aim.Status);
    }

    [Fact]
    public void EndMeasurementSession_GivenSessionStarted_AllSensorsStopReading()
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

        // Assert
        A.CallTo(() => baseInstrument.StopReading()).MustHaveHappenedOnceExactly();
        A.CallTo(() => sensor1.StopReading()).MustHaveHappenedOnceExactly();
        A.CallTo(() => sensor2.StopReading()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void EndMeasurementSession_GivenSessionStarted_AllSensorsDisconnecting()
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

        // Assert
        A.CallTo(() => baseInstrument.Disconnect()).MustHaveHappenedOnceExactly();
        A.CallTo(() => sensor1.Disconnect()).MustHaveHappenedOnceExactly();
        A.CallTo(() => sensor2.Disconnect()).MustHaveHappenedOnceExactly();
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
        Assert.NotEmpty(session.BaseInstrumentData.Measurements);

        Assert.NotNull(session.SensorDataSeries);
        Assert.NotEmpty(session.SensorDataSeries);
        Assert.All(session.SensorDataSeries, data =>
        {
            Assert.NotEmpty(data.Measurements);
        });
    }

    [Fact]
    public void EndMeasurementSession_GivenSessionStarted_StatusIsReady()
    {
        // Arrange
        var aim = new AIM();
        var validConfiguration = CreateValidConfiguration();
        aim.LoadConfiguration(validConfiguration);
        aim.StartMeasurementSession();

        // Act
        aim.EndMeasurementSession();

        // Assert
        Assert.Equal(AIMStatus.Ready, aim.Status);
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
    public void Predict_GivenSessionEndedAndSessionIsInvalid_ThrowsException()
    {
        // Arrange
        var aim = new AIM();
        var emptyConfiguration = CreateNoDataConfiguration();
        aim.LoadConfiguration(emptyConfiguration);
        aim.StartMeasurementSession();
        var session = aim.EndMeasurementSession();

        // Act & Assert
        var exception = Assert.Throws<AIMInvalidSessionDataException>(() => aim.Predict(session));
    }

    [Fact]
    public void Predict_SessionDataIsValid_InvokesPrediction()
    {
        // Arrange
        var aim = new AIM();
        var validConfiguration = CreateValidConfiguration();
        aim.LoadConfiguration(validConfiguration);
        aim.StartMeasurementSession();
        var session = aim.EndMeasurementSession();

        var aiModel = validConfiguration.AIModel;

        // Act
        aim.Predict(session);

        // Assert
        A.CallTo(() => aiModel.Predict(session)).MustHaveHappenedOnceExactly();
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
