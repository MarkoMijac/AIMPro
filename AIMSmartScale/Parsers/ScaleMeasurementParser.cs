using System;
using System.Diagnostics.Contracts;
using System.Reflection.Metadata.Ecma335;
using AIMCore;
using AIMCore.Parsers;

namespace AIMSmartScale.Parsers;

public class ScaleMeasurementParser : MeasurementParser
{
    public ScaleMeasurementParser()
    {
        Name = "Scale Parser";
    }

    public override Measurement Parse(string data)
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

        return new Measurement(weight, DateTime.Now);
    }
}
