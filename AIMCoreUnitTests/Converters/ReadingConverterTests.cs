using System;
using AIMCore;
using AIMCore.Converters;
using FakeItEasy;

namespace AIMCoreUnitTests.Converters;

public class ReadingConverterTests
{
    private TestReadingConverter _testConverter;

    public ReadingConverterTests()
    {
        _testConverter = new TestReadingConverter("Test converter");
    }

    [Fact]
    public void ToString_GivenConverterNameSet_ReturnsName()
    {
        //Act
        var result = _testConverter.ToString();

        //Assert
        Assert.Equal("Test converter", result);
    }

    [Fact]
    public void Convert_GivenData_ReturnsTimeSeriesData()
    {
        //Act
        var result = _testConverter.Convert("Test data");

        //Assert
        Assert.IsType<SensorReading>(result);
        Assert.Equal("Test source", result.Name);
    }
}
