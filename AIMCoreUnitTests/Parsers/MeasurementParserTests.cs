using System;
using AIMCore;
using AIMCore.Parsers;
using FakeItEasy;

namespace AIMCoreUnitTests.Parsers;

public class MeasurementParserTests
{
    private TestParser _testParser;

    public MeasurementParserTests()
    {
        _testParser = new TestParser("Test parser");
    }

    [Fact]
    public void ToString_GivenParserNameSet_ReturnsName()
    {
        //Act
        var result = _testParser.ToString();

        //Assert
        Assert.Equal("Test parser", result);
    }

    [Fact]
    public void Parse_GivenData_ReturnsTimeSeriesData()
    {
        //Act
        var result = _testParser.Parse("Test data");

        //Assert
        Assert.IsType<TimeSeriesData>(result);
        Assert.Equal("Test source", result.SourceName);
    }
}
