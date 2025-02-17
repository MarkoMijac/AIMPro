using System;
using AIMCore.Exceptions;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;

namespace AIMCore;

public class AIModel : IAIModel
{
    private InferenceSession _model;
    public string Name { get; private set; }
    public string Path { get; private set; }

    public AIModel(string name, string path)
    {
        Name = name;
        LoadModel(path);
    }

    private void LoadModel(string path)
    {
        ValidatePath(path);

        try
        {
            _model = new InferenceSession(path);
            Path = path;
        }
        catch (Exception)
        {
            throw new AIMLoadingModelException();
        }
    }

    private void ValidatePath(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new AIMInvalidModelPathException();
        }
        else if(File.Exists(path)==false)
        {
            throw new AIMInvalidModelPathException();
        }
        else if (System.IO.Path.GetExtension(path).ToLower() != ".onnx")
        {
            throw new AIMInvalidModelPathException();
        }
    }

    public void UnloadModel()
    {
        _model?.Dispose();
        _model = null;// Unload the model
    }

    public IPredictionResult Predict(MeasurementSession session)
    {
        if(session==null || session.IsValid()==false)
        {
            throw new AIMInvalidSessionDataException();
        }

        var inputTensors = PrepareInputTensors(session);

        using (var results = _model.Run(inputTensors))
        {
            var outputValues = results.First().AsEnumerable<float>().ToArray();

            // Assuming the model outputs a single corrected value
            return new PredictionResult
            {
                CorrectedMeasurement = outputValues[0]
            };
        }
    }

    public async Task<IPredictionResult> PredictAsync(MeasurementSession session)
    {
        if(session==null || session.IsValid()==false)
        {
            throw new AIMInvalidSessionDataException();
        }
    
        var inputTensors = await PrepareInputTensorsAsync(session);
    
        using (var results = await Task.Run(() => _model.Run(inputTensors)))
        {
            var outputValues = results.First().AsEnumerable<float>().ToArray();
            
            // Assuming the model outputs a single corrected value and confidence score
            return new PredictionResult
            {
                CorrectedMeasurement = outputValues[0],
            };
        }
    }

    private async Task<List<NamedOnnxValue>> PrepareInputTensorsAsync(MeasurementSession session)
    {
        return await Task.Run(() => PrepareInputTensors(session));
    }

    private List<NamedOnnxValue> PrepareInputTensors(MeasurementSession session)
    {
        // Combine base instrument data and sensor data into a single array
        var baseInstrumentValues = session.BaseInstrumentReading.Measurements.Values.ToArray();
        var sensorsValues = session.SensorsReadings.SelectMany(reading => reading.Measurements.Values.ToArray()).ToArray();
        var combinedValues = baseInstrumentValues.Concat(sensorsValues).ToArray();

        // Create a single tensor from the combined values
        var combinedTensor = new DenseTensor<float>(combinedValues, new[] { 1, combinedValues.Length });
        var inputs = new List<NamedOnnxValue>
        {
            NamedOnnxValue.CreateFromTensor(session.BaseInstrumentReading.Name, combinedTensor)
        };

        return inputs;
    }

    public void Dispose()
    {
        UnloadModel();
    }
}
