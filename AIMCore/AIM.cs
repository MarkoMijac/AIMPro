using System;
using System.Runtime.InteropServices.Marshalling;
using AIMCore.Configurations;

namespace AIMCore;

public class AIM
{
    public Configuration Configuration { get; set; }
    private MeasurementSession _measurementSession;
    public AIM(Configuration configuration)
    {
        Configuration = configuration;
    }

    public IPredictionResult Predict()
    {
        if(_measurementSession == null)
        {
            throw new AIMException("No measurement session data available!");
        }

        var aiModel = Configuration.AIModel;
        var prediction = aiModel.Predict(_measurementSession);
        ClearMeasurementSession();

        return prediction;
    }

    private void ClearMeasurementSession()
    {
        _measurementSession = null;
    }

    public void StartMeasurementSession()
    {
        _measurementSession = new MeasurementSession();

        var instrument = Configuration.BaseInstrument;
        instrument.Connect();
        instrument.StartReading();

        foreach (var sensor in Configuration.Sensors)
        {
            sensor.Connect();
            sensor.StartReading();
        }
    }

    public MeasurementSession EndMeasurementSession()
    {
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

        return _measurementSession;
    }
}
