using AIMCore.Communication;
using System.Device.Gpio;

namespace AIMSmartScale.CommunicationStrategies
{
    public class Hx711CommunicationStrategy : CommunicationStrategy<string>
    {
        private GpioController controller = new GpioController();
        private int clkPin = 21;
        private int dataPin = 20;
        private int lastVal = 0;
        private const int Gain = 64;

        public override void Connect()
        {
            controller.OpenPin(clkPin, PinMode.Output);
            controller.OpenPin(dataPin, PinMode.Input);
            Thread.Sleep(10);

            controller.Write(clkPin, false);
        }

        public override void Disconnect()
        {
            controller.Write(clkPin, false);
            controller.Write(clkPin, true);
            Thread.Sleep(100);

            controller.ClosePin(clkPin);
            controller.ClosePin(dataPin);
        }

        public override string Execute(string command)
        {
            return $"{DateTime.Now};{ReadAverage(3)}";
        }

        private bool IsReady()
        {
            var valueRead = controller.Read(dataPin);
            return valueRead == PinValue.Low;
        }

        private byte ReadNextBit()
        {
            controller.Write(clkPin, true);
            controller.Write(clkPin, false);

            var value = controller.Read(dataPin);
            return value == PinValue.High ? (byte)1 : (byte)0;
        }

        private byte ReadNextByte() // MSB by default
        {
            byte byteValue = 0;

            for (int x = 0; x < 8; x++)
            {
                byteValue <<= 1;
                byteValue |= ReadNextBit();
            }
            return byteValue;
        }

        private byte[] ReadRawBytes()
        {
            while (!IsReady())
                Thread.Yield();

            var firstByte = ReadNextByte();
            var secondByte = ReadNextByte();
            var thirdByte = ReadNextByte();

            // HX711 Channel and gain factor are set by number of bits read after 24 data bits.
            for (int i = 0; i < Gain; i++)
            {
                // Read and discard extra bits.
                ReadNextBit();
            }

            return [firstByte, secondByte, thirdByte];            
        }

        private int ReadAverage(int count = 3)
        {
            if (count == 1)
                return ReadInt();

            // If we're averaging across a low amount of values, just take the median
            if (count < 5)
                return ReadMedian(count);

            // If big N, remove the outliers, then take the mean of the remaining set
            var valueList = new List<int>(count);

            for (int x = 0; x < count; x++)
                valueList.Add(ReadInt());
            
            valueList.Sort();

            // Trim 20% of the lowest and highest values
            int trimAmount = Convert.ToInt32(Math.Round(valueList.Count * 0.2));

            // Trim the edge case values
            valueList = valueList.Skip(trimAmount).Take(valueList.Count - trimAmount * 2).ToList();

            return Convert.ToInt32(Math.Round(valueList.Average()));
        }

        private int ReadInt()
        {
            var dataBytes = ReadRawBytes();
            
            // Join the raw bytes into a single 24bit 2s complement value
            var twosComplementValue = ((dataBytes[0] << 16) |
                               (dataBytes[1] << 8) |
                               dataBytes[2]);
            int signedIntValue = ComplementToSigned(twosComplementValue);

            lastVal = signedIntValue;

            return signedIntValue;
        }


        private int ReadMedian(int count = 3)
        {
            if (count == 1) 
                return ReadInt();

            var valueList = new List<int>(count);

            for (int x = 0; x < count; x++)
                valueList.Add(ReadInt());
            
            valueList.Sort();

            if ((count & 0x1) == 0x0) {
                var midpoint = valueList.Count / 2;
                return (valueList[midpoint] + valueList[midpoint + 1]) / 2;
            }
            else
                return valueList[valueList.Count / 2];
        }

        
        private int ComplementToSigned(int inputValue) => -(inputValue & 0x800000) + (inputValue & 0x7fffff);

        public async override Task ConnectAsync()
        {
            await Task.Run(Connect);
        }

        public async override Task DisconnectAsync()
        {
            await Task.Run(Disconnect);
        }

        public async override Task<string> ExecuteAsync(string command)
        {
            return await Task.Run(() => Execute(command));
        }
    }
}