using System;
using System.Runtime.InteropServices.Marshalling;
using AIMCore.Configurations;

namespace AIMCore;

public class AIM
{
    public Configuration Configuration { get; private set; }
    private MeasurementSession _measurementSession;

    public AIMStatus Status { get; private set; }

    public AIM()
    {
        Status = AIMStatus.ConfigurationNotLoaded;
    }

    public IPredictionResult Predict()
    {
        return Predict(_measurementSession);
    }

    private IPredictionResult Predict(MeasurementSession session)
    {
        if(session == null || session.IsValid() == false)
        {
            throw new AIMException("No measurement session data available!");
        }

        var aiModel = Configuration.AIModel;
        var prediction = aiModel.Predict(session);
        ClearMeasurementSession();

        return prediction;
    }

    public void LoadConfiguration(Configuration configuration)
    {
        ValidateConfiguration(configuration);
        Configuration = configuration;
        Status = AIMStatus.ConfigurationLoaded;
    }

    private static void ValidateConfiguration(Configuration configuration)
    {
        if (configuration == null)
        {
            throw new AIMException("No configuration loaded!");
        }
        else if(configuration.IsValid() == false)
        {
            throw new AIMException("Provided configuration is not valid!");

        }
    }

    private void ClearMeasurementSession()
    {
        _measurementSession = null;
    }

    public void StartMeasurementSession()
    {
        ValidateConfiguration(Configuration);

        _measurementSession = new MeasurementSession();
        ConnectAllSensors();
        StartReadingAllSensors();

        Status = AIMStatus.MeasurementSessionStarted;
    }

    private void StartReadingAllSensors()
    {
        var instrument = Configuration.BaseInstrument;
        instrument.StartReading();

        foreach (var sensor in Configuration.Sensors)
        {
            sensor.StartReading();
        }
    }

    private void ConnectAllSensors()
    {
        var instrument = Configuration.BaseInstrument;
        instrument.Connect();

        foreach (var sensor in Configuration.Sensors)
        {
            sensor.Connect();
        }
    }

    private async Task ConnectAllSensorsAsync()
    {
        List<Task> tasks = new List<Task>();
        
        var instrument = Configuration.BaseInstrument;
        var instrumentConnect = instrument.ConnectAsync();
        
        tasks.Add(instrumentConnect);

        foreach (var sensor in Configuration.Sensors)
        {
            var sensorConnect = sensor.ConnectAsync();
            tasks.Add(sensorConnect);
        }

        Task.WaitAll(tasks.ToArray());
    }

    public MeasurementSession EndMeasurementSession()
    {
        ValidateConfiguration(Configuration);

        if(Status != AIMStatus.MeasurementSessionStarted)
        {
            throw new AIMException("No measurement session started!");
        }

        var instrument = Configuration.BaseInstrument;
        var instrumentData = instrument.StopReading();
        _measurementSession.SetInstrumentData(instrumentData);

        instrument.Disconnect();
        

        foreach (var sensor in Configuration.Sensors)
        {
            var sensorData = sensor.StopReading();
            _measurementSession.AddSensorData(sensorData);
            
            sensor.Disconnect();
        }

        Status = AIMStatus.MeasurementSessionStopped;
        return _measurementSession;
    }
}
