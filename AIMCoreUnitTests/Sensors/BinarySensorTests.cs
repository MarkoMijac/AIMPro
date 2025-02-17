using System;
using System.Text;
using AIMCore;
using AIMCore.Communication;
using AIMCore.Exceptions;
using AIMCore.Parsers;
using AIMCore.Sensors;
using FakeItEasy;

namespace AIMCoreUnitTests.Sensors;

public class BinarySensorTests
{
    private BinarySensor sensor;
    private ICommunicationStrategy<byte[]> communication;
    private IMeasurementParser<byte[]> parser;

    private byte[] getTempRequest = new byte[] { 0x47, 0x45, 0x54, 0x5F, 0x54, 0x45, 0x4D, 0x50 };

    public BinarySensorTests()
    {
        communication = A.Fake<CommunicationStrategy<byte[]>>();
        parser = A.Fake<MeasurementParser<byte[]>>();
        sensor = new BinarySensor("Thermometer",getTempRequest, communication, parser);
    }

    [Fact]
    public void Constructor_GivenEmptyName_ThrowsException()
    {
        // Arrange
        string emptyName = string.Empty;

        // Act
        Action act = () => new BinarySensor(emptyName, getTempRequest, communication, parser);

        // Assert
        Assert.Throws<AIMException>(act);
    }

    [Fact]
    public void Constructor_GivenRequestCommandIsNull_ThrowsException()
    {
        // Arrange
        byte[] nullRequestCommand = null;

        // Act
        Action act = () => new BinarySensor("Thermometer", nullRequestCommand, communication, parser);

        // Assert
        Assert.Throws<AIMException>(act);
    }

    [Fact]
    public void Constructor_GivenRequestCommandIsEmpty_ThrowsException()
    {
        // Arrange
        byte[] requestCommand = new byte[0];

        // Act
        Action act = () => new BinarySensor("Thermometer", requestCommand, communication, parser);

        // Assert
        Assert.Throws<AIMException>(act);
    }

    [Fact]
    public void Constructor_GivenCommunicationStrategyIsNull_ThrowsException()
    {
        // Arrange
        ICommunicationStrategy<byte[]> nullCommunication = null;

        // Act
        Action act = () => new BinarySensor("Thermometer", getTempRequest, nullCommunication, parser);

        // Assert
        Assert.Throws<AIMException>(act);
    }

    [Fact]
    public void Constructor_GivenParserIsNull_ThrowsException()
    {
        // Arrange
        IMeasurementParser<byte[]> nullParser = null;

        // Act
        Action act = () => new BinarySensor("Thermometer", getTempRequest, communication, nullParser);

        // Assert
        Assert.Throws<AIMException>(act);
    }

    [Fact]
    public void Constructor_GivenValidInputParameters_SetsPropertiesCorrectly()
    {
        // Arrange
        string sensorName = "Thermometer";

        // Act
        var sensor = new BinarySensor(sensorName, getTempRequest, communication, parser);

        // Assert
        Assert.Equal(sensorName, sensor.Name);
        Assert.Equal(getTempRequest, sensor.RequestCommand);
        Assert.Equal(communication, sensor.CommunicationStrategy);
        Assert.Equal(parser, sensor.Parser);
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
    public void GetParser_GivenParserIsSet_ReturnsParser()
    {
        // Arrange

        // Act
        var actual = sensor.Parser;

        // Assert
        Assert.Equal(parser, actual);
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

    [Fact]
    public void StartReading_InvokesCommunicationStrategySend()
    {
        // Arrange

        // Act
        sensor.StartReading();

        // Assert
        A.CallTo(() => communication.Send(getTempRequest)).MustHaveHappened();
    }

    [Fact]
    public async Task StartReadingAsync_InvokesCommunicationStrategySendAsync()
    {
        // Arrange

        // Act
        await sensor.StartReadingAsync();

        // Assert
        A.CallTo(() => communication.SendAsync(getTempRequest)).MustHaveHappened();
    }

    [Fact]
    public void StopReading_GivenReadingIsStarted_InvokesCommunicationStrategyReceive()
    {
        // Arrange
        sensor.StartReading();

        // Act
        sensor.StopReading();

        // Assert
        A.CallTo(() => sensor.CommunicationStrategy.Receive()).MustHaveHappened();
    }

    [Fact]
    public void StopReading_GivenReadingIsStarted_InvokesParserParse()
    {
        // Arrange
        var resultArray = Encoding.UTF8.GetBytes("25.0");
        A.CallTo(() => communication.Receive()).Returns(resultArray);
        sensor.StartReading();

        // Act
        sensor.StopReading();

        // Assert
        A.CallTo(() => sensor.Parser.Parse(resultArray)).MustHaveHappened();
    }

    [Fact]
    public async Task StopReadingAsync_GivenReadingIsStarted_InvokesCommunicationStrategyReceiveAsync()
    {
        // Arrange
        sensor.StartReading();

        // Act
        await sensor.StopReadingAsync();

        // Assert
        A.CallTo(() => sensor.CommunicationStrategy.ReceiveAsync()).MustHaveHappened();
    }

    [Fact]
    public async Task StopReadingAsync_GivenReadingIsStarted_InvokesParserParse()
    {
        // Arrange
        var resultArray = Encoding.UTF8.GetBytes("25.0");
        A.CallTo(() => communication.ReceiveAsync()).Returns(resultArray);
        sensor.StartReading();

        // Act
        await sensor.StopReadingAsync();

        // Assert
        A.CallTo(() => sensor.Parser.Parse(resultArray)).MustHaveHappened();
    }

    [Fact]
    public void StopReading_GivenReadingIsStarted_ShouldReturnParsedData()
    {
        // Arrange
        var resultArray = Encoding.UTF8.GetBytes("25.0");
        var reading = new SensorReading("Thermometer");
        reading.AddMeasurement("Temperature", 25.0f);
        reading.TimeStamp = DateTime.Now;

        A.CallTo(() => communication.Receive()).Returns(resultArray);
        A.CallTo(() => parser.Parse(resultArray)).Returns(reading);
        sensor.StartReading();

        // Act
        var actual = sensor.StopReading();

        // Assert
        Assert.Equal(actual, reading);
    }

    [Fact]
    public async Task StopReadingAsync_GivenReadingIsStarted_ShouldReturnParsedData()
    {
        // Arrange
        var resultArray = Encoding.UTF8.GetBytes("25.0");
        var reading = new SensorReading("Thermometer");
        reading.AddMeasurement("Temperature", 25.0f);
        reading.TimeStamp = DateTime.Now;

        A.CallTo(() => communication.ReceiveAsync()).Returns(resultArray);
        A.CallTo(() => parser.Parse(resultArray)).Returns(reading);
        sensor.StartReading();

        // Act
        var actual = await sensor.StopReadingAsync();

        // Assert
        Assert.Equal(actual, reading);
    }

    [Fact]
    public void StartReading_GivenReadingIsStarted_ThrowsException()
    {
        // Arrange
        sensor.StartReading();

        // Act
        Action act = () => sensor.StartReading();

        // Assert
        Assert.Throws<AIMException>(act);
    }

    [Fact]
    public void StopReading_GivenReadingIsNotStarted_ThrowsException()
    {
        // Arrange
        

        // Act
        Action act = () => sensor.StopReading();

        // Assert
        Assert.Throws<AIMException>(act);
    }

    [Fact]
    public async Task StartReadingAsync_GivenReadingIsStarted_ThrowsException()
    {
        // Arrange
        await sensor.StartReadingAsync();

        // Act
        Func<Task> act = sensor.StartReadingAsync;

        // Assert
        await Assert.ThrowsAsync<AIMException>(act);
    }

    [Fact]
    public async Task StopReadingAsync_GivenReadingIsNotStarted_ThrowsException()
    {
        // Arrange

        // Act
        Func<Task> act = sensor.StopReadingAsync;

        // Assert
        await Assert.ThrowsAsync<AIMException>(act);
    }
}
