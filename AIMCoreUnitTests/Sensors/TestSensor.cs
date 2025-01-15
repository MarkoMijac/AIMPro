using System;
using AIMCore.Communication;
using AIMCore.Parsers;
using AIMCore.Sensors;

namespace AIMCoreUnitTests.Sensors;

public class TestSensor : SensorBase<string>
{
	public TestSensor(string name, string requestCommand, ICommunicationStrategy<string> communicationStrategy, IMeasurementParser<string> measurementParser)
		: base(name, requestCommand, communicationStrategy, measurementParser)
	{
        
	}
}
