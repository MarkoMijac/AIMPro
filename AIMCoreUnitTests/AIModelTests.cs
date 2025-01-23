using System;
using AIMCore;
using AIMCore.Exceptions;

namespace AIMCoreUnitTests;

public class AIModelTests
{
    [Fact]
    public void Constructor_GivenPathEmpty_ThrowsException()
    {
        // Arrange
        var path = string.Empty;

        // Act
        Action act = () => new AIModel("TestModel", path);

        // Assert
        Assert.Throws<AIMInvalidModelPathException>(act);
    }
}
