using System;
using System.Text;
using AIMCore;
using AIMCore.Communication;
using AIMCore.Exceptions;
using AIMCore.Converters;
using AIMCore.Sensors;
using FakeItEasy;

namespace AIMCoreUnitTests.Sensors;

public class BinarySensorTests
{
    private BinarySensor sensor;
    private ICommunicationStrategy<byte[]> communication;
    private IReadingConverter<byte[]> converter;

    private byte[] getTempRequest = new byte[] { 0x47, 0x45, 0x54, 0x5F, 0x54, 0x45, 0x4D, 0x50 };

    public BinarySensorTests()
    {
        communication = A.Fake<CommunicationStrategy<byte[]>>();
        converter = A.Fake<ReadingConverterBase<byte[]>>();
        sensor = new BinarySensor("Thermometer",getTempRequest, communication, converter);
    }

    [Fact]
    public void Constructor_GivenEmptyName_ThrowsException()
    {
        // Arrange
        string emptyName = string.Empty;

        // Act
        Action act = () => new BinarySensor(emptyName, getTempRequest, communication, converter);

        // Assert
        Assert.Throws<AIMException>(act);
    }

    [Fact]
    public void Constructor_GivenRequestCommandIsNull_ThrowsException()
    {
        // Arrange
        byte[] nullRequestCommand = null;

        // Act
        Action act = () => new BinarySensor("Thermometer", nullRequestCommand, communication, converter);

        // Assert
        Assert.Throws<AIMException>(act);
    }

    [Fact]
    public void Constructor_GivenRequestCommandIsEmpty_ThrowsException()
    {
        // Arrange
        byte[] requestCommand = new byte[0];

        // Act
        Action act = () => new BinarySensor("Thermometer", requestCommand, communication, converter);

        // Assert
        Assert.Throws<AIMException>(act);
    }

    [Fact]
    public void Constructor_GivenCommunicationStrategyIsNull_ThrowsException()
    {
        // Arrange
        ICommunicationStrategy<byte[]> nullCommunication = null;

        // Act
        Action act = () => new BinarySensor("Thermometer", getTempRequest, nullCommunication, converter);

        // Assert
        Assert.Throws<AIMException>(act);
    }

    [Fact]
    public void Constructor_GivenConverterIsNull_ThrowsException()
    {
        // Arrange
        IReadingConverter<byte[]> nullConverter = null;

        // Act
        Action act = () => new BinarySensor("Thermometer", getTempRequest, communication, nullConverter);

        // Assert
        Assert.Throws<AIMException>(act);
    }

    [Fact]
    public void Constructor_GivenValidInputParameters_SetsPropertiesCorrectly()
    {
        // Arrange
        string sensorName = "Thermometer";

        // Act
        var sensor = new BinarySensor(sensorName, getTempRequest, communication, converter);

        // Assert
        Assert.Equal(sensorName, sensor.Name);
        Assert.Equal(getTempRequest, sensor.RequestCommand);
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

        // Act
        var actual = sensor.RequestCommand;

        // Assert
        Assert.Equal(getTempRequest, actual);
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
