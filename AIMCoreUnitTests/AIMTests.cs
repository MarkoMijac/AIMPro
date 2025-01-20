using System;
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
    public void LoadConfiguration_GivenConfigurationIsValid_LoadsConfiguration()
    {
        // Arrange
        var aim = new AIM();

        // Act
        aim.LoadConfiguration(_testConfiguration);

        // Assert
        Assert.Equal(_testConfiguration, aim.Configuration);
    }

    [Fact]
    public void LoadConfiguration_GivenConfigurationIsNull_ThrowsAIMException()
    {
        // Arrange
        var aim = new AIM();

        // Act & Assert
        var exception = Assert.Throws<AIMException>(() => aim.LoadConfiguration(null));
        Assert.Equal("Provided configuration is not valid!", exception.Message);
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
}
