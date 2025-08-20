
namespace DeepSigma.DataSeries.Enums
{
    /// <summary>
    /// Enumeration for time series algorithms.
    /// </summary>
    public enum TimeSeriesAlgo
    {
        /// <summary>
        /// Signal is triggered when the value is greater than a specified threshold.
        /// </summary>
        SignalGreaterThanTrigger,
        /// <summary>
        /// Signal is triggered when the value is less than a specified threshold.
        /// </summary>
        SignalLessThanTrigger,
        /// <summary>
        /// Signal is triggered when the value is greater than or equal to a specified threshold.
        /// </summary>
        SignalGreaterThanOrEqualToTrigger,
        /// <summary>
        /// Signal is triggered when the value is less than or equal to a specified threshold.
        /// </summary>
        SignalLessThanOrEqualToTrigger,
        /// <summary>
        /// Signal is triggered when the value is equal to a specified threshold.
        /// </summary>
        SignalEqualToTrigger
    }
}
