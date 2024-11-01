using System;

namespace AIMCore;

public class Measurement
{
    public double Value { get; private set; }
    public DateTime TimeStamp { get; private set; }

    public Measurement(double value, DateTime timeStamp)
    {
        Value = value;
        TimeStamp = timeStamp;
    }
}
