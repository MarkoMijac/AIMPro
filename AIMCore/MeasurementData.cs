using System;

namespace AIMCore;

public class MeasurementData
{
    public double Value { get; private set; }
    public DateTime TimeStamp { get; private set; }
    public string Unit { get; private set; }

    public MeasurementData(double value, DateTime timeStamp, string unit)
    {
        Value = value;
        TimeStamp = timeStamp;
        Unit = unit;
    }
}
