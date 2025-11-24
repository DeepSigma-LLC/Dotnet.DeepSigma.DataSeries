using DeepSigma.DataSeries.Enums;
using System.Numerics;

namespace DeepSigma.DataSeries.Strategies
{
    /// <summary>
    /// Provides algorithms for time series strategies to determine trade indications based on signal and trigger values.
    /// </summary>
    public static class TimeSeriesStrategyAlgos
    {
        /// <summary>
        /// Returns a function that determines if a trade should be indicated based on the specified algorithm.
        /// </summary>
        /// <param name="Algo"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static Func<T, T, bool> GetTradeIndicatedAlgo<T>(TimeSeriesAlgo Algo) where T : INumber<T>
        {
            return Algo switch
            {
                TimeSeriesAlgo.SignalGreaterThanTrigger => SignalGreaterThanTrigger,
                TimeSeriesAlgo.SignalLessThanTrigger => SignalLessThanTrigger,
                TimeSeriesAlgo.SignalGreaterThanOrEqualToTrigger => SignalGreaterThanOrEqualToTrigger,
                TimeSeriesAlgo.SignalLessThanOrEqualToTrigger => SignalLessThanOrEqualToTrigger,
                TimeSeriesAlgo.SignalEqualToTrigger => SignalEqualToTrigger,
                _ => throw new NotImplementedException(),
            };
        }

        /// <summary>
        /// Determines if the signal value is greater than the trigger value.
        /// </summary>
        /// <param name="signal_value"></param>
        /// <param name="trigger_value"></param>
        /// <returns></returns>
        public static bool SignalGreaterThanTrigger<T>(T signal_value, T trigger_value) where T : INumber<T>
        {
            if (signal_value > trigger_value)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Determines if the signal value is less than the trigger value.
        /// </summary>
        /// <param name="signal_value"></param>
        /// <param name="trigger_value"></param>
        /// <returns></returns>
        public static bool SignalLessThanTrigger<T>(T signal_value, T trigger_value) where T : INumber<T>
        {
            if (signal_value < trigger_value)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Determines if the signal value is greater than or equal to the trigger value.
        /// </summary>
        /// <param name="signal_value"></param>
        /// <param name="trigger_value"></param>
        /// <returns></returns>
        public static bool SignalGreaterThanOrEqualToTrigger<T>(T signal_value, T trigger_value) where T : INumber<T>
        {
            if (signal_value >= trigger_value)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Determines if the signal value is less than or equal to the trigger value.
        /// </summary>
        /// <param name="signal_value"></param>
        /// <param name="trigger_value"></param>
        /// <returns></returns>
        public static bool SignalLessThanOrEqualToTrigger<T>(T signal_value, T trigger_value) where T : INumber<T>
        {
            if (signal_value <= trigger_value)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Determines if the signal value is equal to the trigger value.
        /// </summary>
        /// <param name="signal_value"></param>
        /// <param name="trigger_value"></param>
        /// <returns></returns>
        public static bool SignalEqualToTrigger<T>(T signal_value, T trigger_value) where T : INumber<T>
        {
            return (signal_value == trigger_value);
        }
    }
}
