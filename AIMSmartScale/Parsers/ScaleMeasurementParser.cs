using System;
using AIMCore;
using AIMCore.Parsers;

namespace AIMSmartScale.Parsers;

public class ScaleMeasurementParser : IMeasurementParser
{
    public string Name => "Scale Parser";

    public MeasurementData Parse(string data)
    {
        var split = data.Split(' ');
        if (split.Length != 2)
        {
            throw new Exception("Invalid data format");
        }

        if (!double.TryParse(split[0], out var weight))
        {
            throw new Exception("Invalid weight format");
        }

        return new MeasurementData(weight, DateTime.Now, "kg");
    }

    public override string ToString()
    {
        return Name;
    }
}
