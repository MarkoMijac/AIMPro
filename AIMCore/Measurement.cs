using System;

namespace AIMCore;

public class SensorReading
{
    public string Name { get; private set; }
    public DateTime TimeStamp { get; set; }
    public Dictionary<string, float> Measurements { get; private set; }

    public SensorReading(string name)
    {
        Name = name;
        Measurements = new Dictionary<string, float>();
    }

    public void AddMeasurement(string name, float value)
    {
        Measurements[name] = value;
    }
}
