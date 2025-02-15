using System;

namespace AIMCore;

public class Measurement
{
    public string Name { get; private set; }
    public double Value { get; private set; }
    public DateTime TimeStamp { get; private set; }

    public Measurement(double value, DateTime timeStamp)
    {
        Value = value;
        TimeStamp = timeStamp;
    }

    public Measurement(string name, double value, DateTime timeStamp)
    {
        Name = name;
        Value = value;
        TimeStamp = timeStamp;
    }
}
