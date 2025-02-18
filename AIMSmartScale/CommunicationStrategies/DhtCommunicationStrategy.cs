using System.Device.Gpio;
using System.Diagnostics;
using AIMCore.Communication;

namespace AIMSmartScale.CommunicationStrategies
{
    public class DhtCommunicationStrategy : CommunicationStrategy<string>
    {
        private readonly GpioController controller = new GpioController();
        private readonly int pin = 26;
        private readonly TimeSpan MinTimeBetweenReads = TimeSpan.FromSeconds(2.5);
        private readonly uint loopCount = 10000;
        private const int BufferSize = 5;
        
        private Stopwatch stopwatch = new();
        private int lastMeasurement;
        private bool isLastReadSuccessful = false;
        private byte[] readBuffer = [];

        public override void Connect()
        {
            controller.OpenPin(pin);
            Thread.Sleep(1000); 
        }

        public override void Disconnect()
        {
            controller.ClosePin(pin);
        }

        public override string Execute(string command)
        {
            if (Environment.TickCount - lastMeasurement >= MinTimeBetweenReads.Milliseconds)
                readBuffer = ReadThroughOneWire();

            var temp = GetTemperature(readBuffer);
            var humidity = GetHumidity(readBuffer);

            return $"time:{DateTime.Now};temp:{temp};humidity:{humidity}";
        }

        private static float GetHumidity(Span<byte> readBuff) => (float)(readBuff[0] + readBuff[1] * 0.1);

        private static float GetTemperature(Span<byte> readBuff) => (float)(readBuff[2] + readBuff[3] * 0.1);

        private bool WaitForPinState(PinValue expectedState)
        {
            uint count = loopCount;
            while (controller.Read(pin) == expectedState)
            {
                if (count-- == 0)
                {
                    isLastReadSuccessful = false;
                    return false;
                }
            }
            return true;
        }

        private byte[] ReadThroughOneWire()
        {
            var pinMode = controller.IsPinModeSupported(pin, PinMode.InputPullUp) ? PinMode.InputPullUp : PinMode.Input;
            byte readVal = 0;
            byte[] dataBuffer = new byte[BufferSize];

            // Manual pullup
            controller.SetPinMode(pin, PinMode.Output);
            controller.Write(pin, PinValue.High);
            DelayHelper.DelayMilliseconds(20, true);

            // Pull low to signal start of read
            controller.Write(pin, PinValue.Low);
            DelayHelper.DelayMilliseconds(20, true);

            // Ensure HIGH remains for 20-40µs
            controller.Write(pin, PinValue.High);
            DelayHelper.DelayMicroseconds(30, true);

            // DHT takes control now, switch to input mode
            controller.SetPinMode(pin, pinMode);

            if (!WaitForPinState(PinValue.Low) || !WaitForPinState(PinValue.High))
                return dataBuffer;

            /* Read 40 bits
            * FORMAT:
                8bit integral RH data +
                8bit decimal RH data +
                8bit integral T data +
                8bit decimal T data +
                8bit check sum.
            */
            for (int i = 0; i < 40; i++)
            {
                if (!WaitForPinState(PinValue.Low)) return dataBuffer;

                stopwatch.Restart();
                if (!WaitForPinState(PinValue.High)) return dataBuffer;
                stopwatch.Stop();

                // Bit conversion: If pulse duration > 30µs, it's a 1 ( 0 i nominally 26-28µs)
                readVal = (byte)((readVal << 1) | (stopwatch.ElapsedTicks * 1_000_000F / Stopwatch.Frequency > 30 ? 1 : 0));

                // Store byte after every 8 bits
                if ((i + 1) % 8 == 0)
                {
                    dataBuffer[i / 8] = readVal;
                }
            }

            lastMeasurement = Environment.TickCount;
            isLastReadSuccessful = ValidateChecksum(dataBuffer);

            return dataBuffer;
        }

        private static bool ValidateChecksum(byte[] buffer)
        {
            return buffer[4] == ((buffer[0] + buffer[1] + buffer[2] + buffer[3]) & 0xFF) &&
                !(buffer[0] == 0 && buffer[1] == 0 && buffer[2] == 0 && buffer[3] == 0);
        }

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