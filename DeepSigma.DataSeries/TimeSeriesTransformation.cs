using DeepSigma.DataSeries.Enums;
using DeepSigma.General.Enums;

namespace DeepSigma.DataSeries
{
    /// <summary>
    /// Represents a transformation applied to time series data.
    /// </summary>
    public class TimeSeriesTransformation : SeriesTransformation
    {
        /// <summary>
        /// Default observation window count. Used for moving averages and standard deviation counts.
        /// </summary>
        public int ObservationWindowCount { get; set; } = 20;

        /// <summary>
        /// Observations to lage the time series by.
        /// </summary>
        public int ObservationLag { get; set; } = 0;

        /// <summary>
        /// Controls how lags are handled over weekends. If "Anyday" then the lag does not care about weekdays (i.e., Monday data is lagged to Sunday). If "Weekdays" are selected then Monday values are lagged to Friday.
        /// </summary>
        public DaySelectionType DaySelectionTypeForLag { get; set; } = DaySelectionType.AnyDay;

        /// <summary>
        /// Data transformation of the time series data.
        /// </summary>
        public TimeSeriesDataTransformation DataTransformation { get; set; } = TimeSeriesDataTransformation.None;

    }
}
