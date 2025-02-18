using System.Diagnostics;

static partial class DelayHelper
    {
        private const long TicksPerSecond = TimeSpan.TicksPerSecond;
        private const long TicksPerMillisecond = TimeSpan.TicksPerMillisecond;
        private const long TicksPerMicrosecond = TimeSpan.TicksPerMillisecond / 1000;

        /// <summary>A scale that normalizes the hardware ticks to <see cref="TimeSpan" /> ticks which are 100ns in length.</summary>
        private static readonly double s_tickFrequency = (double)TicksPerSecond / Stopwatch.Frequency;

        /// <summary>
        /// Delay for at least the specified <paramref name="time" />.
        /// </summary>
        /// <param name="time">The amount of time to delay.</param>
        /// <param name="allowThreadYield">
        /// True to allow yielding the thread. If this is set to false, on single-proc systems
        /// this will prevent all other code from running.
        /// </param>
        public static void Delay(TimeSpan time, bool allowThreadYield)
        {
            long start = Stopwatch.GetTimestamp();
            long delta = (long)(time.Ticks / s_tickFrequency);
            long target = start + delta;

            if (!allowThreadYield)
            {
                do
                {
                    Thread.SpinWait(1);
                }
                while (Stopwatch.GetTimestamp() < target);
            }
            else
            {
                SpinWait spinWait = new SpinWait();
                do
                {
                    spinWait.SpinOnce();
                }
                while (Stopwatch.GetTimestamp() < target);
            }
        }

        /// <summary>
        /// Delay for at least the specified <paramref name="microseconds"/>.
        /// </summary>
        /// <param name="microseconds">The number of microseconds to delay.</param>
        /// <param name="allowThreadYield">
        /// True to allow yielding the thread. If this is set to false, on single-proc systems
        /// this will prevent all other code from running.
        /// </param>
        public static void DelayMicroseconds(int microseconds, bool allowThreadYield)
        {
            var time = TimeSpan.FromTicks(microseconds * TicksPerMicrosecond);
            Delay(time, allowThreadYield);
        }

        /// <summary>
        /// Delay for at least the specified <paramref name="milliseconds"/>
        /// </summary>
        /// <param name="milliseconds">The number of milliseconds to delay.</param>
        /// <param name="allowThreadYield">
        /// True to allow yielding the thread. If this is set to false, on single-proc systems
        /// this will prevent all other code from running.
        /// </param>
        public static void DelayMilliseconds(int milliseconds, bool allowThreadYield)
        {
            var time = TimeSpan.FromTicks(milliseconds * TicksPerMillisecond);
            Delay(time, allowThreadYield);
        }
    }