using System;
using AIMCore.Exceptions;

namespace AIMCore.IntegrationTests;

public class AIModelTests
{
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
        var path = "/home/ubuntustudio/Documents/AIMPro/AIMCore.IntegrationTests/TestData/ValidModel.onnx";
        var model = new AIModel("TestModel", path);
        var session = new MeasurementSession();
        var scaleData = new TimeSeriesData("measured_weight");
        scaleData.AddMeasurement(new Measurement(20.0, DateTime.Now));
        var vibrationSensorData = new TimeSeriesData("vibration_rate");
        vibrationSensorData.AddMeasurement(new Measurement(1.2, DateTime.Now));
        var gyroscopData = new TimeSeriesData("incline_angle");
        gyroscopData.AddMeasurement(new Measurement(0.5, DateTime.Now));

        session.SetInstrumentData(scaleData);
        session.AddSensorData(vibrationSensorData);
        session.AddSensorData(gyroscopData);

        // Act
        var result = model.Predict(session);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<PredictionResult>(result);
        Assert.InRange(result.CorrectedMeasurement, 0, float.MaxValue);
        Assert.InRange(result.ConfidenceScore, 0, 1);
    }

}
