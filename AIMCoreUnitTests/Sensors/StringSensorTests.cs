using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using AIMCore;
using AIMCore.Communication;
using AIMCore.Exceptions;
using AIMCore.Parsers;
using AIMCore.Sensors;
using FakeItEasy;

namespace AIMCoreUnitTests.Sensors;

public class StringSensorTests
{
    private StringSensor sensor;
    private ICommunicationStrategy<string> communication;
    private IMeasurementParser<string> parser;

    public StringSensorTests()
    {
        communication = A.Fake<CommunicationStrategy<string>>();
        parser = A.Fake<MeasurementParser<string>>();
        sensor = new StringSensor("Thermometer", "GET_TEMP", communication, parser);
    }

    [Fact]
    public void Constructor_GivenEmptyName_ThrowsException()
    {
        // Arrange
        string emptyName = string.Empty;

        // Act
        Action act = () => new StringSensor(emptyName, "GET_TEMP", communication, parser);

        // Assert
        Assert.Throws<AIMException>(act);
    }

    [Fact]
    public void Constructor_GivenRequestCommandIsNull_ThrowsException()
    {
        // Arrange
        string nullRequestCommand = null;

        // Act
        Action act = () => new StringSensor("Thermometer", nullRequestCommand, communication, parser);

        // Assert
        Assert.Throws<AIMException>(act);
    }

    [Fact]
    public void Constructor_GivenRequestCommandIsEmpty_ThrowsException()
    {
        // Arrange
        string requestCommand = "";

        // Act
        Action act = () => new StringSensor("Thermometer", requestCommand, communication, parser);

        // Assert
        Assert.Throws<AIMException>(act);
    }

    [Fact]
    public void Constructor_GivenCommunicationStrategyIsNull_ThrowsException()
    {
        // Arrange
        ICommunicationStrategy<string> nullCommunication = null;

        // Act
        Action act = () => new StringSensor("Thermometer", "GET_TEMP", nullCommunication, parser);

        // Assert
        Assert.Throws<AIMException>(act);
    }

    [Fact]
    public void Constructor_GivenParserIsNull_ThrowsException()
    {
        // Arrange
        IMeasurementParser<string> nullParser = null;

        // Act
        Action act = () => new StringSensor("Thermometer", "GET_TEMP", communication, nullParser);

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
        var sensor = new StringSensor(sensorName, requestCommand, communication, parser);

        // Assert
        Assert.Equal(sensorName, sensor.Name);
        Assert.Equal(requestCommand, sensor.RequestCommand);
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
        A.CallTo(() => communication.Receive()).Returns("25.0");
        sensor.StartReading();

        // Act
        sensor.StopReading();

        // Assert
        A.CallTo(() => sensor.Parser.Parse("25.0")).MustHaveHappened();
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
        A.CallTo(() => communication.ReceiveAsync()).Returns("25.0");
        sensor.StartReading();

        // Act
        await sensor.StopReadingAsync();

        // Assert
        A.CallTo(() => sensor.Parser.Parse("25.0")).MustHaveHappened();
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
