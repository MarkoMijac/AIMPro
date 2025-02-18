using AIMCore.Communication;
using System.Device.I2c;

namespace AIMSmartScale.CommunicationStrategies
{
    internal class Mpu6050CommunicationStrategy : CommunicationStrategy<string>
    {
        private const double GRAVITY_MS2 = 9.80665;
        private I2cDevice? device;

        public enum AccelScaleModifier
        {
            SCALE_2G = 16384,
            SCALE_4G = 8192,
            SCALE_8G = 4096,
            SCALE_16G = 2048
        }

        public enum GyroScaleModifier
        {
            SCALE_250DEG = 131,
            SCALE_500DEG = 65,
            SCALE_1000DEG = 32,
            SCALE_2000DEG = 16
        }

        public enum AccelRange
        {
            RANGE_2G = 0x00,
            RANGE_4G = 0x08,
            RANGE_8G = 0x10,
            RANGE_16G = 0x18
        }

        public enum GyroRange
        {
            RANGE_250DEG = 0x00,
            RANGE_500DEG = 0x08,
            RANGE_1000DEG = 0x10,
            RANGE_2000DEG = 0x18
        }

        public enum Registers
        {
            GYRO_CONFIG = 0x1B,
            ACCEL_CONFIG = 0x1C,
            ACCEL_XOUT = 0x3B,
            TEMP_OUT = 0x41,
            GYRO_XOUT = 0x43,
            PWR_MGMT_1 = 0x6B
        }

        public override void Connect()
        {
            device ??= I2cDevice.Create(new I2cConnectionSettings(1, 0x68));

            WriteByte((byte)Registers.PWR_MGMT_1, 0x00);
        }

        public override void Disconnect()
        {
            device?.Dispose();
        }

        public override string Execute(string command)
        {
            var accel = GetAcceleration();
            var temp = GetTemperature();
            var gyro = GetGyroData();

            return $"time:{DateTime.Now};accelX:{accel["x"]};accelY:{accel["y"]};accelZ:{accel["z"]};gyroX:{gyro["x"]};gyroY:{gyro["y"]};gyroZ:{gyro["z"]};temp:{temp}";
        }

        private void WriteByte(byte register, byte value)
        {
            Span<byte> buffer = stackalloc byte[] { register, value };
            device?.Write(buffer);
        }

        private short ReadI2CWord(Registers register)
        {
            Span<byte> data = stackalloc byte[2];
            device?.WriteByte((byte)register);
            device?.Read(data);

            return (short)((data[0] << 8) | data[1]);
        }

        public double GetTemperature()
        {
            short rawTemp = ReadI2CWord(Registers.TEMP_OUT);
            return (rawTemp / 340.0) + 36.53;
        }

        public Dictionary<string, double> GetAcceleration(bool g = false)
        {
            short x = ReadI2CWord(Registers.ACCEL_XOUT);
            short y = ReadI2CWord(Registers.ACCEL_XOUT + 2);
            short z = ReadI2CWord(Registers.ACCEL_XOUT + 4);

            const double accelScale = (double)AccelScaleModifier.SCALE_2G;

            double xVal = x / accelScale;
            double yVal = y / accelScale;
            double zVal = z / accelScale;

            if (!g)
            {
                xVal *= GRAVITY_MS2;
                yVal *= GRAVITY_MS2;
                zVal *= GRAVITY_MS2;
            }

            return new Dictionary<string, double> {
                { "x", xVal },
                { "y", yVal },
                { "z", zVal }
            };
        }

        public Dictionary<string, double> GetGyroData()
        {
            short x = ReadI2CWord(Registers.GYRO_XOUT);
            short y = ReadI2CWord(Registers.GYRO_XOUT + 2);
            short z = ReadI2CWord(Registers.GYRO_XOUT + 4);

            const double gyroScale = (double)GyroScaleModifier.SCALE_250DEG;

            return new Dictionary<string, double> {
                { "x", x / gyroScale },
                { "y", y / gyroScale },
                { "z", z / gyroScale }
            };
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
