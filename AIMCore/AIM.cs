using System;
using System.Runtime.InteropServices.Marshalling;
using AIMCore.Configurations;
using AIMCore.Exceptions;
using AIMCore.StateMachine;

namespace AIMCore;

public class AIM
{
    public Configuration Configuration { get; private set; }

    private AIMStateManager _stateManager;

    internal AIMStatus Status
    {
        get
        {
            return _stateManager.CurrentStatus;
        }
    }

    public AIM()
    {
        _stateManager = new AIMStateManager();
    }

    public IPredictionResult Predict(MeasurementSession session)
    {
        if (session == null)
        {
            throw new AIMNoSessionDataAvailableException();
        }
        else if (session.IsValid() == false)
        {
            throw new AIMInvalidSessionDataException();
        }

        var aiModel = Configuration.AIModel;
        var prediction = aiModel.Predict(session);

        return prediction;
    }

    public async Task<IPredictionResult> PredictionAsync(MeasurementSession session)
    {
        if (session == null)
        {
            throw new AIMNoSessionDataAvailableException();
        }
        else if (session.IsValid() == false)
        {
            throw new AIMInvalidSessionDataException();
        }

        var aiModel = Configuration.AIModel;
        var prediction = await aiModel.PredictAsync(session);

        return prediction;
    }

    public void LoadConfiguration(Configuration configuration)
    {
        ValidateConfiguration(configuration);

        if (Configuration == null)
        {
            _stateManager.PerformTransition(ActivationEvent.LoadConfiguration);
        }
        else
        {
            _stateManager.PerformTransition(ActivationEvent.ChangeConfiguration);
        }
        Configuration = configuration;
    }

    private static void ValidateConfiguration(Configuration configuration)
    {
        if (configuration == null)
        {
            throw new AIMNoConfigurationProvidedException();
        }
        else if (configuration.IsValid() == false)
        {
            throw new AIMInvalidConfigurationException();
        }
    }

    public void StartMeasurementSession()
    {
        ValidateConfiguration(Configuration);

        _stateManager.PerformTransition(ActivationEvent.StartMeasurementSession);

        ConnectAllSensors();
        StartReadingAllSensors();
    }

    public async Task StartMeasurementSessionAsync()
    {
        ValidateConfiguration(Configuration);

        _stateManager.PerformTransition(ActivationEvent.StartMeasurementSession);

        await ConnectAllSensorsAsync();
        await StartReadingAllSensorsAsync();
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

    private async Task StartReadingAllSensorsAsync()
    {
        var tasks = new List<Task>();

        var instrument = Configuration.BaseInstrument;
        var instrumentReading = instrument.StartReadingAsync();
        tasks.Add(instrumentReading);

        foreach (var sensor in Configuration.Sensors)
        {
            var sensorReading = sensor.StartReadingAsync();
            tasks.Add(sensorReading);
        }

        await Task.WhenAll(tasks.ToArray());
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

        await Task.WhenAll(tasks.ToArray());
    }

    public MeasurementSession EndMeasurementSession()
    {
        ValidateConfiguration(Configuration);
        if (Status != AIMStatus.Measuring)
        {
            throw new AIMMeasurementSessionNotStartedException();
        }

        _stateManager.PerformTransition(ActivationEvent.EndMeasurementSession);

        var session = new MeasurementSession();
        StopReadingAllSensors(session);
        DisconnectAllSensors();

        return session;
    }

    private void DisconnectAllSensors()
    {
        var instrument = Configuration.BaseInstrument;
        instrument.Disconnect();

        foreach (var sensor in Configuration.Sensors)
        {
            sensor.Disconnect();
        }
    }

    private void StopReadingAllSensors(MeasurementSession session)
    {
        var instrument = Configuration.BaseInstrument;
        var instrumentData = instrument.StopReading();
        session.SetInstrumentData(instrumentData);

        foreach (var sensor in Configuration.Sensors)
        {
            var sensorData = sensor.StopReading();
            session.AddSensorData(sensorData);
        }
    }

    private async Task DisconnectAllSensorsAsync()
    {
        var tasks = new List<Task>();

        var instrument = Configuration.BaseInstrument;
        tasks.Add(instrument.DisconnectAsync());

        foreach (var sensor in Configuration.Sensors)
        {
            tasks.Add(sensor.DisconnectAsync());
        }

        await Task.WhenAll(tasks.ToArray());
    }

    private async Task StopReadingAllSensorsAsync(MeasurementSession session)
    {
        var instrument = Configuration.BaseInstrument;
        var instrumentData = await instrument.StopReadingAsync();
        session.SetInstrumentData(instrumentData);

        foreach (var sensor in Configuration.Sensors)
        {
            var sensorData = await sensor.StopReadingAsync();
            session.AddSensorData(sensorData);
        }
    }

    public async Task<MeasurementSession> EndMeasurementSessionAsync()
    {
        ValidateConfiguration(Configuration);
        if (Status != AIMStatus.Measuring)
        {
            throw new AIMMeasurementSessionNotStartedException();
        }

        _stateManager.PerformTransition(ActivationEvent.EndMeasurementSession);

        var session = new MeasurementSession();
        await StopReadingAllSensorsAsync(session);
        await DisconnectAllSensorsAsync();

        return session;
    }
}
