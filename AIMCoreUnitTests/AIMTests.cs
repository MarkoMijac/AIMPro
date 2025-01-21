using System;
using System.Runtime.Intrinsics.X86;
using AIMCore;
using AIMCore.Configurations;
using AIMCore.Exceptions;
using AIMCore.Sensors;
using FakeItEasy;

namespace AIMCoreUnitTests;

public class AIMTests
{
    private Configuration _validConfiguration;
    public AIMTests()
    {
        CreateTestConfiguration();
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

    private void CreateTestConfiguration()
    {
        _validConfiguration = new Configuration("Test Configuration");
        _validConfiguration.AIModel = A.Fake<IAIModel>();
        _validConfiguration.BaseInstrument = A.Fake<ISensor>();
        _validConfiguration.Sensors.Add(A.Fake<ISensor>());
        _validConfiguration.Sensors.Add(A.Fake<ISensor>());

        var baseInstrument = _validConfiguration.BaseInstrument;
        var baseInstrumentTestData = new TimeSeriesData("Base Instrument");
        baseInstrumentTestData.AddMeasurement(new Measurement(1, DateTime.Now));
        A.CallTo(() => baseInstrument.StopReading()).Returns(baseInstrumentTestData);

        var sensor1 = _validConfiguration.Sensors[0];
        var sensor1TestData = new TimeSeriesData("Sensor 1");
        sensor1TestData.AddMeasurement(new Measurement(2, DateTime.Now));
        A.CallTo(() => sensor1.StopReading()).Returns(sensor1TestData);

        var sensor2 = _validConfiguration.Sensors[1];
        var sensor2TestData = new TimeSeriesData("Sensor 2");
        sensor2TestData.AddMeasurement(new Measurement(3, DateTime.Now));
        A.CallTo(() => sensor2.StopReading()).Returns(sensor2TestData);
    }

    [Fact]
    public void Constructor_StatusIsConfigurationNotLoaded()
    {
        // Arrange
        var aim = new AIM();

        // Act & Assert
        Assert.Equal(AIMStatus.ConfigurationNotLoaded, aim.Status);
    }

    [Fact]
    public void LoadConfiguration_GivenConfigurationIsValid_LoadsConfiguration()
    {
        // Arrange
        var aim = new AIM();

        // Act
        aim.LoadConfiguration(_validConfiguration);

        // Assert
        Assert.Equal(_validConfiguration, aim.Configuration);
        Assert.Equal(AIMStatus.ConfigurationLoaded, aim.Status);
    }

    [Fact]
    public void LoadConfiguration_GivenConfigurationIsNull_ThrowsAIMException()
    {
        // Arrange
        var aim = new AIM();

        // Act & Assert
        var exception = Assert.Throws<AIMNoConfigurationProvidedException>(() => aim.LoadConfiguration(null));
        Assert.Equal(AIMStatus.ConfigurationNotLoaded, aim.Status);
    }

    [Fact]
    public void LoadConfiguration_GivenConfigurationIsNotValid_ThrowsAIMException()
    {
        // Arrange
        var aim = new AIM();
        var configuration = new Configuration("Invalid Configuration");

        // Act & Assert
        var exception = Assert.Throws<AIMInvalidConfigurationException>(() => aim.LoadConfiguration(configuration));
        Assert.Equal(AIMStatus.ConfigurationNotLoaded, aim.Status);
    }

    [Fact]
    public void StartMeasurementSession_GivenConfigurationNotLoaded_ThrowsException()
    {
        // Arrange
        var aim = new AIM();

        // Act & Assert
        var exception = Assert.Throws<AIMNoConfigurationProvidedException>(() => aim.StartMeasurementSession());
        Assert.Equal(AIMStatus.ConfigurationNotLoaded, aim.Status);
    }

    [Fact]
    public void StartMeasurementSession_GivenConfigurationLoaded_AllSensorsConnecting()
    {
        // Arrange
        var aim = new AIM();
        aim.LoadConfiguration(_validConfiguration);

        var baseInstrument = _validConfiguration.BaseInstrument;
        var sensor1 = _validConfiguration.Sensors[0];
        var sensor2 = _validConfiguration.Sensors[1];

        // Act
        aim.StartMeasurementSession();

        // Assert
        A.CallTo(() => baseInstrument.Connect()).MustHaveHappenedOnceExactly();
        A.CallTo(() => sensor1.Connect()).MustHaveHappenedOnceExactly();
        A.CallTo(() => sensor2.Connect()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void StartMeasurementSession_GivenConfigurationLoaded_AllSensorsStartReading()
    {
        // Arrange
        var aim = new AIM();
        aim.LoadConfiguration(_validConfiguration);

        var baseInstrument = _validConfiguration.BaseInstrument;
        var sensor1 = _validConfiguration.Sensors[0];
        var sensor2 = _validConfiguration.Sensors[1];

        // Act
        aim.StartMeasurementSession();

        // Assert
        A.CallTo(() => baseInstrument.StartReading()).MustHaveHappenedOnceExactly();
        A.CallTo(() => sensor1.StartReading()).MustHaveHappenedOnceExactly();
        A.CallTo(() => sensor2.StartReading()).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void StartMeasurementSession_GivenConfigurationLoaded_MeasurementSessionIsStarted()
    {
        // Arrange
        var aim = new AIM();
        aim.LoadConfiguration(_validConfiguration);

        // Act
        aim.StartMeasurementSession();

        // Assert
        Assert.Equal(AIMStatus.MeasurementSessionStarted, aim.Status);
    }

    [Fact]
    public void EndMeasurementSession_GivenConfigurationNotLoaded_ThrowsException()
    {
        // Arrange
        var aim = new AIM();

        // Act & Assert
        var exception = Assert.Throws<AIMMeasurementSessionNotStartedException>(() => aim.EndMeasurementSession());
    }

    [Fact]
    public void EndMeasurementSession_GivenSessionNotStarted_ThrowsException()
    {
        // Arrange
        var aim = new AIM();
        aim.LoadConfiguration(_validConfiguration);

        // Act & Assert
        var exception = Assert.Throws<AIMMeasurementSessionNotStartedException>(() => aim.EndMeasurementSession());
    }

    [Fact]
    public void EndMeasurementSession_GivenSessionStarted_AllSensorsStopReading()
    {
        // Arrange
        var aim = new AIM();
        aim.LoadConfiguration(_validConfiguration);

        var baseInstrument = _validConfiguration.BaseInstrument;
        var sensor1 = _validConfiguration.Sensors[0];
        var sensor2 = _validConfiguration.Sensors[1];

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
        aim.LoadConfiguration(_validConfiguration);

        var baseInstrument = _validConfiguration.BaseInstrument;
        var sensor1 = _validConfiguration.Sensors[0];
        var sensor2 = _validConfiguration.Sensors[1];

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
        aim.LoadConfiguration(_validConfiguration);

        

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
    public void EndMeasurementSession_GivenSessionStarted_MeasurementSessionIsStopped()
    {
        // Arrange
        var aim = new AIM();
        aim.LoadConfiguration(_validConfiguration);
        aim.StartMeasurementSession();

        // Act
        aim.EndMeasurementSession();

        // Assert
        Assert.Equal(AIMStatus.MeasurementSessionEnded, aim.Status);
    }

    [Fact]
    public void Predict_GivenConfigurationNotLoaded_ThrowsException()
    {
        // Arrange
        var aim = new AIM();

        // Act & Assert
        var exception = Assert.Throws<AIMNoSessionDataAvailableException>(() => aim.Predict());
    }

    [Fact]
    public void Predict_GivenSessionNotStarted_ThrowsException()
    {
        // Arrange
        var aim = new AIM();
        aim.LoadConfiguration(_validConfiguration);

        // Act & Assert
        var exception = Assert.Throws<AIMNoSessionDataAvailableException>(() => aim.Predict());
    }

    [Fact]
    public void Predict_GivenSessionNotEnded_ThrowsException()
    {
        // Arrange
        var aim = new AIM();
        aim.LoadConfiguration(_validConfiguration);
        aim.StartMeasurementSession();

        // Act & Assert
        var exception = Assert.Throws<AIMNoSessionDataAvailableException>(() => aim.Predict());
    }

    [Fact]
    public void Predict_GivenSessionEndedAndSessionIsInvalid_ThrowsException()
    {
        // Arrange
        var aim = new AIM();
        var emptyConfiguration = CreateNoDataConfiguration();
        aim.LoadConfiguration(emptyConfiguration);
        aim.StartMeasurementSession();
        aim.EndMeasurementSession();

        // Act & Assert
        var exception = Assert.Throws<AIMNoSessionDataAvailableException>(() => aim.Predict());
    }

    [Fact]
    public void Predict_GivenSessionEndedAndSessionIsValid_InvokesPrediction()
    {
        // Arrange
        var aim = new AIM();
        aim.LoadConfiguration(_validConfiguration);
        aim.StartMeasurementSession();
        var session = aim.EndMeasurementSession();

        var aiModel = _validConfiguration.AIModel;

        // Act
        aim.Predict();

        // Assert
        A.CallTo(() => aiModel.Predict(session)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void Predict_GivenSessionEndedAndSessionIsValid_ReturnsPrediction()
    {
        // Arrange
        var aim = new AIM();
        aim.LoadConfiguration(_validConfiguration);
        aim.StartMeasurementSession();
        aim.EndMeasurementSession();

        // Act
        var prediction = aim.Predict();

        // Assert
        Assert.NotNull(prediction);
    }
}
