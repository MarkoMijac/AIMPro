using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using AIMCore;
using AIMCore.Communication;
using AIMCore.Exceptions;
using AIMCore.Converters;
using AIMCore.Sensors;
using FakeItEasy;

namespace AIMCoreUnitTests.Sensors;

public class StringSensorTests
{
    private StringSensor sensor;
    private ICommunicationStrategy<string> communication;
    private IReadingConverter<string> converter;

    public StringSensorTests()
    {
        communication = A.Fake<CommunicationStrategy<string>>();
        converter = A.Fake<ReadingConverterBase<string>>();
        sensor = new StringSensor("Thermometer", "GET_TEMP", communication, converter);
    }

    [Fact]
    public void Constructor_GivenEmptyName_ThrowsException()
    {
        // Arrange
        string emptyName = string.Empty;

        // Act
        Action act = () => new StringSensor(emptyName, "GET_TEMP", communication, converter);

        // Assert
        Assert.Throws<AIMException>(act);
    }

    [Fact]
    public void Constructor_GivenRequestCommandIsNull_ThrowsException()
    {
        // Arrange
        string nullRequestCommand = null;

        // Act
        Action act = () => new StringSensor("Thermometer", nullRequestCommand, communication, converter);

        // Assert
        Assert.Throws<AIMException>(act);
    }

    [Fact]
    public void Constructor_GivenRequestCommandIsEmpty_ThrowsException()
    {
        // Arrange
        string requestCommand = "";

        // Act
        Action act = () => new StringSensor("Thermometer", requestCommand, communication, converter);

        // Assert
        Assert.Throws<AIMException>(act);
    }

    [Fact]
    public void Constructor_GivenCommunicationStrategyIsNull_ThrowsException()
    {
        // Arrange
        ICommunicationStrategy<string> nullCommunication = null;

        // Act
        Action act = () => new StringSensor("Thermometer", "GET_TEMP", nullCommunication, converter);

        // Assert
        Assert.Throws<AIMException>(act);
    }

    [Fact]
    public void Constructor_GivenConverterIsNull_ThrowsException()
    {
        // Arrange
        IReadingConverter<string> nullConverter = null;

        // Act
        Action act = () => new StringSensor("Thermometer", "GET_TEMP", communication, nullConverter);

        // Assert
        Assert.Throws<AIMException>(act);
    }

    [Fact]
    public void Constructor_GivenValidInputParameters_SetsPropertiesCorrectly()
    {
        // Arrange
        string sensorName = "Thermometer";
        string requestCommand = "GET_TEMP";

        // Act
        var sensor = new StringSensor(sensorName, requestCommand, communication, converter);

        // Assert
        Assert.Equal(sensorName, sensor.Name);
        Assert.Equal(requestCommand, sensor.RequestCommand);
        Assert.Equal(communication, sensor.CommunicationStrategy);
        Assert.Equal(converter, sensor.Converter);
    }

    [Fact]
    public void GetName_GivenNameIsSet_ReturnsName()
    {
        // Arrange
        string expected = "Thermometer";

        // Act
        string actual = sensor.Name;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetRequestCommand_GivenRequestCommandIsSet_ReturnsRequestCommand()
    {
        // Arrange
        string expected = "GET_TEMP";

        // Act
        string actual = sensor.RequestCommand;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetCommunicationStrategy_GivenCommunicationStrategyIsSet_ReturnsCommunicationStrategy()
    {
        // Arrange

        // Act
        var actual = sensor.CommunicationStrategy;

        // Assert
        Assert.Equal(communication, actual);
    }

    [Fact]
    public void GetConverter_GivenConverterIsSet_ReturnsConverter()
    {
        // Arrange

        // Act
        var actual = sensor.Converter;

        // Assert
        Assert.Equal(converter, actual);
    }

    [Fact]
    public void Connect_InvokesCommunicationStrategyConnect()
    {
        // Arrange

        // Act
        sensor.Connect();

        // Assert
        A.CallTo(() => communication.Connect()).MustHaveHappened();
    }

    [Fact]
    public async Task ConnectAsync_InvokesCommunicationStrategyConnectAsync()
    {
        // Arrange

        // Act
        await sensor.ConnectAsync();

        // Assert
        A.CallTo(() => communication.ConnectAsync()).MustHaveHappened();
    }

    [Fact]
    public void Disconnect_InvokesCommunicationStrategyDisconnect()
    {
        // Arrange

        // Act
        sensor.Disconnect();

        // Assert
        A.CallTo(() => communication.Disconnect()).MustHaveHappened();
    }

    [Fact]
    public async Task DisconnectAsync_InvokesCommunicationStrategyDisconnectAsync()
    {
        // Arrange

        // Act
        await sensor.DisconnectAsync();

        // Assert
        A.CallTo(() => communication.DisconnectAsync()).MustHaveHappened();
    }

}
