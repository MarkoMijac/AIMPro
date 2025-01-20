using System;
using System.Runtime.Intrinsics.X86;
using AIMCore;
using AIMCore.Configurations;
using AIMCore.Sensors;
using FakeItEasy;

namespace AIMCoreUnitTests;

public class AIMTests
{
    private Configuration _testConfiguration;
    public AIMTests()
    {
        _testConfiguration = new Configuration("Test Configuration");
        _testConfiguration.AIModel = A.Fake<IAIModel>();
        _testConfiguration.BaseInstrument = A.Fake<ISensor>();
        _testConfiguration.Sensors.Add(A.Fake<ISensor>());
        _testConfiguration.Sensors.Add(A.Fake<ISensor>());
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
        aim.LoadConfiguration(_testConfiguration);

        // Assert
        Assert.Equal(_testConfiguration, aim.Configuration);
        Assert.Equal(AIMStatus.ConfigurationLoaded, aim.Status);
    }

    [Fact]
    public void LoadConfiguration_GivenConfigurationIsNull_ThrowsAIMException()
    {
        // Arrange
        var aim = new AIM();

        // Act & Assert
        var exception = Assert.Throws<AIMException>(() => aim.LoadConfiguration(null));
        Assert.Equal("Provided configuration is not valid!", exception.Message);
        Assert.Equal(AIMStatus.ConfigurationNotLoaded, aim.Status);
    }

    [Fact]
    public void LoadConfiguration_GivenConfigurationIsNotValid_ThrowsAIMException()
    {
        // Arrange
        var aim = new AIM();
        var configuration = new Configuration("Invalid Configuration");

        // Act & Assert
        var exception = Assert.Throws<AIMException>(() => aim.LoadConfiguration(configuration));
        Assert.Equal("Provided configuration is not valid!", exception.Message);
    }

    [Fact]
    public void StartMeasurementSession_GivenConfigurationNotLoaded_ThrowsException()
    {
        // Arrange
        var aim = new AIM();

        // Act & Assert
        var exception = Assert.Throws<AIMException>(() => aim.StartMeasurementSession());
        Assert.Equal("No configuration loaded!", exception.Message);
        Assert.Equal(AIMStatus.ConfigurationNotLoaded, aim.Status);
    }

    [Fact]
    public void StartMeasurementSession_GivenConfigurationLoaded_AllSensorsConnecting()
    {
        // Arrange
        var aim = new AIM();
        aim.LoadConfiguration(_testConfiguration);

        var baseInstrument = _testConfiguration.BaseInstrument;
        var sensor1 = _testConfiguration.Sensors[0];
        var sensor2 = _testConfiguration.Sensors[1];

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
        aim.LoadConfiguration(_testConfiguration);

        var baseInstrument = _testConfiguration.BaseInstrument;
        var sensor1 = _testConfiguration.Sensors[0];
        var sensor2 = _testConfiguration.Sensors[1];

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
        aim.LoadConfiguration(_testConfiguration);

        // Act
        aim.StartMeasurementSession();

        // Assert
        Assert.Equal(AIMStatus.MeasurementSessionStarted, aim.Status);
    }

    [Fact]
    public void EndMeasurementSession_GivenSessionStarted_AllSensorsStopReading()
    {
        // Arrange
        var aim = new AIM();
        aim.LoadConfiguration(_testConfiguration);

        var baseInstrument = _testConfiguration.BaseInstrument;
        var sensor1 = _testConfiguration.Sensors[0];
        var sensor2 = _testConfiguration.Sensors[1];

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
        aim.LoadConfiguration(_testConfiguration);

        var baseInstrument = _testConfiguration.BaseInstrument;
        var sensor1 = _testConfiguration.Sensors[0];
        var sensor2 = _testConfiguration.Sensors[1];

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
        aim.LoadConfiguration(_testConfiguration);

        var baseInstrument = _testConfiguration.BaseInstrument;
        var baseInstrumentTestData = new TimeSeriesData("Base Instrument");
        baseInstrumentTestData.AddMeasurement(new Measurement(1, DateTime.Now));
        A.CallTo(() => baseInstrument.StopReading()).Returns(baseInstrumentTestData);

        var sensor1 = _testConfiguration.Sensors[0];
        var sensor1TestData = new TimeSeriesData("Sensor 1");
        sensor1TestData.AddMeasurement(new Measurement(2, DateTime.Now));
        A.CallTo(() => sensor1.StopReading()).Returns(sensor1TestData);

        var sensor2 = _testConfiguration.Sensors[1];
        var sensor2TestData = new TimeSeriesData("Sensor 2");
        sensor2TestData.AddMeasurement(new Measurement(3, DateTime.Now));
        A.CallTo(() => sensor2.StopReading()).Returns(sensor2TestData);

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
        aim.LoadConfiguration(_testConfiguration);
        aim.StartMeasurementSession();

        // Act
        aim.EndMeasurementSession();

        // Assert
        Assert.Equal(AIMStatus.MeasurementSessionStopped, aim.Status);
    }

}
