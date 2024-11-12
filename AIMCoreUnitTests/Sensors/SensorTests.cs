using System;
using System.Security.Cryptography;
using AIMCore;
using AIMCore.Communication;
using AIMCore.Parsers;
using AIMCore.Sensors;
using FakeItEasy;

namespace AIMCoreUnitTests.Sensors;

public class SensorTests
{
    private TestSensor sensor;
    private ICommunicationStrategy<string> communication;
    private IMeasurementParser<string> parser;

    public SensorTests()
    {
        communication = A.Fake<CommunicationStrategy<string>>();
        parser = A.Fake<MeasurementParser<string>>();
        sensor = new TestSensor("Thermometer", "GET_TEMP", communication, parser);
    }

    [Fact]
    public void Constructor_GivenEmptyName_ThrowsException()
    {
        // Arrange
        string emptyName = string.Empty;

        // Act
        Action act = () => new TestSensor(emptyName, "GET_TEMP", communication, parser);

        // Assert
        Assert.Throws<AIMException>(act);
    }

    [Fact]
    public void Constructor_GivenRequestCommandIsNull_ThrowsException()
    {
        // Arrange
        string nullRequestCommand = null;

        // Act
        Action act = () => new TestSensor("Thermometer", nullRequestCommand, communication, parser);

        // Assert
        Assert.Throws<AIMException>(act);
    }

    [Fact]
    public void Constructor_GivenParserIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        IMeasurementParser<string> nullParser = null;

        // Act
        Action act = () => new TestSensor("Thermometer", "GET_TEMP", communication, nullParser);

        // Assert
        Assert.Throws<AIMException>(act);
    }

    [Fact]
    public void Constructor_GivenCommunicationIsNull_ShouldThrowArgumentNullException()
    {
        // Arrange
        ICommunicationStrategy<string> nullCommunication = null;

        // Act
        Action act = () => new TestSensor("Thermometer", "GET_TEMP", nullCommunication, parser);

        // Assert
        Assert.Throws<AIMException>(act);
    }

    [Fact]
    public void Name_GivenName_ShouldReturnName()
    {
        // Arrange
        string expected = "Thermometer";

        // Act
        string actual = sensor.Name;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void RequestCommand_GivenRequestCommand_ShouldReturnRequestCommand()
    {
        // Arrange
        string expected = "GET_TEMP";

        // Act
        string actual = sensor.RequestCommand;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void CommunicationStrategy_GivenCommunicationStrategy_ShouldReturnCommunicationStrategy()
    {
        // Arrange

        // Act
        var actual = sensor.CommunicationStrategy;

        // Assert
        Assert.Equal(communication, actual);
    }

    [Fact]
    public void Parser_GivenParser_ShouldReturnParser()
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
        A.CallTo(() => communication.Send("GET_TEMP")).MustHaveHappened();
    }

    [Fact]
    public async Task StartReadingAsync_InvokesCommunicationStrategySendAsync()
    {
        // Arrange

        // Act
        await sensor.StartReadingAsync();

        // Assert
        A.CallTo(() => communication.SendAsync("GET_TEMP")).MustHaveHappened();
    }

    [Fact]
    public void StopReading_GivenReadingIsStarted_InvokesCommunicationStrategyReceive()
    {
        // Arrange
        A.CallTo(() => communication.Receive()).Returns("25.0");
        sensor.StartReading();

        // Act
        sensor.StopReading();

        // Assert
        A.CallTo(() => communication.Receive()).MustHaveHappened();
    }

    [Fact]
    public async Task StopReadingAsync_GivenReadingIsStarted_InvokesCommunicationStrategyReceiveAsync()
    {
        // Arrange
        A.CallTo(() => communication.ReceiveAsync()).Returns("25.0");
        sensor.StartReading();

        // Act
        await sensor.StopReadingAsync();

        // Assert
        A.CallTo(() => communication.ReceiveAsync()).MustHaveHappened();
    }

    [Fact]
    public void StopReading_GivenReadingIsStarted_ShouldReturnParsedData()
    {
        // Arrange
        var data = new TimeSeriesData("Thermometer");
        data.AddMeasurement(new Measurement(25.0, DateTime.Now));

        A.CallTo(() => communication.Receive()).Returns("25.0");
        A.CallTo(() => parser.Parse("25.0")).Returns(data);
        sensor.StartReading();

        // Act
        var actual = sensor.StopReading();

        // Assert
        Assert.Equal(actual, data);
    }

    [Fact]
    public async Task StopReadingAsync_GivenReadingIsStarted_ShouldReturnParsedData()
    {
        // Arrange
        var data = new TimeSeriesData("Thermometer");
        data.AddMeasurement(new Measurement(25.0, DateTime.Now));

        A.CallTo(() => communication.ReceiveAsync()).Returns("25.0");
        A.CallTo(() => parser.Parse("25.0")).Returns(data);
        sensor.StartReading();

        // Act
        var actual = await sensor.StopReadingAsync();

        // Assert
        Assert.Equal(actual, data);
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

}
