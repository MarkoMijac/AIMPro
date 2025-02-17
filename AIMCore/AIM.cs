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

    public async Task<IPredictionResult> PredictAsync(MeasurementSession session)
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

    public void Connect()
    {
        ValidateConfiguration(Configuration);

        var instrument = Configuration.BaseInstrument;
        instrument.Connect();

        foreach (var sensor in Configuration.Sensors)
        {
            sensor.Connect();
        }
    }

    public async Task ConnectAsync()
    {
        ValidateConfiguration(Configuration);

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

    public void Disconnect()
    {
        ValidateConfiguration(Configuration);
        var instrument = Configuration.BaseInstrument;
        instrument.Disconnect();

        foreach (var sensor in Configuration.Sensors)
        {
            sensor.Disconnect();
        }
    }

    public async Task DisconnectAsync()
    {
        ValidateConfiguration(Configuration);
        var tasks = new List<Task>();

        var instrument = Configuration.BaseInstrument;
        tasks.Add(instrument.DisconnectAsync());

        foreach (var sensor in Configuration.Sensors)
        {
            tasks.Add(sensor.DisconnectAsync());
        }

        await Task.WhenAll(tasks.ToArray());
    }

    public MeasurementSession GetData()
    {
        MeasurementSession session = new MeasurementSession();

        ValidateConfiguration(Configuration);

        var instrument = Configuration.BaseInstrument;
        var instrumentReading = instrument.Read();
        session.SetInstrumentReading(instrumentReading);

        foreach (var sensor in Configuration.Sensors)
        {
            var sensorReading = sensor.Read();
            session.AddSensorReading(sensorReading);
        }

        return session;
    }

    public async Task<MeasurementSession> GetDataAsync()
    {
        MeasurementSession session = new MeasurementSession();

        ValidateConfiguration(Configuration);

        var instrument = Configuration.BaseInstrument;
        var instrumentReading = await instrument.ReadAsync();
        session.SetInstrumentReading(instrumentReading);

        foreach (var sensor in Configuration.Sensors)
        {
            var sensorReading = await sensor.ReadAsync();
            session.AddSensorReading(sensorReading);
        }

        return session;
    }
}
