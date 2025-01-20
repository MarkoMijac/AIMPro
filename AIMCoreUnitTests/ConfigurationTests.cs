using System;
using AIMCore;
using AIMCore.Configurations;
using AIMCore.Sensors;
using FakeItEasy;

namespace AIMCoreUnitTests;

public class ConfigurationTests
{
    [Fact]
    public void Configuration_GivenNameProvided_SetsName()
    {
        // Arrange
        var name = "Test Configuration";

        // Act
        var config = new Configuration(name);

        // Assert
        Assert.Equal(name, config.Name);
    }

    [Fact]
    public void IsValid_GivenBaseInstrumentIsNull_ReturnsFalse()
    {
        // Arrange
        var config = new Configuration("Test Configuration");

        // Act
        var result = config.IsValid();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValid_GivenSensorsListIsEmpty_ReturnsFalse()
    {
        // Arrange
        var config = new Configuration("Test Configuration")
        {
            BaseInstrument = A.Fake<ISensor>(),
            AIModel = A.Fake<IAIModel>()
        };

        // Act
        var result = config.IsValid();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValid_GivenAIModelIsNull_ReturnsFalse()
    {
        // Arrange
        var config = new Configuration("Test Configuration")
        {
            BaseInstrument = A.Fake<ISensor>(),
            Sensors = new List<ISensor> { A.Fake<ISensor>() }
        };

        // Act
        var result = config.IsValid();

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValid_GivenAllPropertiesAreSet_ReturnsTrue()
    {
        // Arrange
        var config = new Configuration("Test Configuration")
        {
            BaseInstrument = A.Fake<ISensor>(),
            Sensors = new List<ISensor> { A.Fake<ISensor>() },
            AIModel = A.Fake<IAIModel>()
        };

        // Act
        var result = config.IsValid();

        // Assert
        Assert.True(result);
    }
}
