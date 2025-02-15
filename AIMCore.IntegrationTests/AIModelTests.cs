using System;
using AIMCore.Exceptions;

namespace AIMCore.IntegrationTests;

public class AIModelTests
{
    private string _validModelPath = "/home/ubuntustudio/Documents/AIMPro/AIMCore.IntegrationTests/TestData/ValidModel.onnx";

    [Fact]
    public void Constructor_GivenInvalidPath_ThrowsException()
    {
        // Arrange
        var path = "invalid_path";

        // Act
        Action act = () => new AIModel("TestModel", path);

        // Assert
        Assert.Throws<AIMInvalidModelPathException>(act);
    }

    [Fact]
    public void Constructor_GivenInvalidFileExtension_ThrowsException()
    {
        // Arrange
        var path = "/home/ubuntustudio/Documents/AIMPro/AIMCore.IntegrationTests/TestData/InvalidFile.ext";

        // Act
        Action act = () => new AIModel("TestModel", path);

        // Assert
        Assert.Throws<AIMInvalidModelPathException>(act);
    }

    [Fact]
    public void Constructor_GivenEmptyOnnxFile_ThrowsException()
    {
        // Arrange
        var path = "/home/ubuntustudio/Documents/AIMPro/AIMCore.IntegrationTests/TestData/EmptyModel.onnx";

        // Act
        Action act = () => new AIModel("TestModel", path);

        // Assert
        Assert.Throws<AIMLoadingModelException>(act);
    }

    [Fact]
    public void Constructor_GivenValidOnnxFile_CreatesModel()
    {
        // Arrange
        var path = "/home/ubuntustudio/Documents/AIMPro/AIMCore.IntegrationTests/TestData/ValidModel.onnx";

        // Act
        var model = new AIModel("TestModel", path);

        // Assert
        Assert.NotNull(model);
    }

    [Fact]
    public void Predict_GivenValidSession_ReturnsPredictionResult()
    {
        // Arrange
        var path = _validModelPath;
        var model = new AIModel("TestModel", path);
        var session = new MeasurementSession();
        var measuredWeight = new Measurement("measured_weight", 20.0, DateTime.Now);
        var vibrationRate = new Measurement("vibration_rate", 1.2, DateTime.Now);
        var inclineAngle = new Measurement("incline_angle", 0.5, DateTime.Now);

        session.SetInstrumentData(measuredWeight);
        session.AddSensorData(vibrationRate);
        session.AddSensorData(inclineAngle);

        // Act
        var result = model.Predict(session);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<PredictionResult>(result);
        Assert.InRange(result.CorrectedMeasurement, 0, float.MaxValue);
    }

    [Fact]
    public void Predict_GivenSessionIsNull_ThrowsException()
    {
        // Arrange
        var path = _validModelPath;
        var model = new AIModel("TestModel", path);
        MeasurementSession session = null;

        // Act
        Action act = () => model.Predict(session);

        // Assert
        Assert.Throws<AIMInvalidSessionDataException>(act);
    }

    [Fact]
    public void Predict_GivenInvalidSession_ThrowsException()
    {
        // Arrange
        var path = _validModelPath;
        var model = new AIModel("TestModel", path);
        var session = new MeasurementSession();

        // Act
        Action act = () => model.Predict(session);

        // Assert
        Assert.Throws<AIMInvalidSessionDataException>(act);
    }

}
