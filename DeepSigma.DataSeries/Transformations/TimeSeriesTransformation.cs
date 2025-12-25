using DeepSigma.DataSeries.Enums;
using DeepSigma.General.Enums;

namespace DeepSigma.DataSeries.Transformations;

/// <summary>
/// Represents a transformation applied to time series.
/// </summary>
public class TimeSeriesTransformation : SeriesTransformation, ISeriesTransformation
{
    /// <summary>
    /// Default observation window count. Used for moving averages and standard deviation counts.
    /// </summary>
    public int ObservationWindowCount { get; set; } = 20;

    /// <summary>
    /// Observations to lage the time series by.
    /// </summary>
    /// <remarks>
    /// Observation lag shifts the time series data backwards by the specified number of observations. 
    /// For example, an observation lag of 1 shifts all data points back by one observation period, meaning that the value at time T now corresponds to the value that was originally at time T-1. 
    /// This is useful for aligning time series data with future events or for creating lagged features in time series analysis.
    /// </remarks>
    public int ObservationLag { get; set; } = 0;

    /// <summary>
    /// Controls how lags are handled over weekends. 
    /// If "Anyday" then the lag does not care about weekdays (i.e., Monday data is lagged to Sunday). 
    /// If "Weekdays" are selected then Monday values are lagged to Friday.
    /// </summary>
    public DaySelectionType DaySelectionTypeForLag { get; set; } = DaySelectionType.AnyDay;

    /// <summary>
    /// Data transformation of the time series data.
    /// </summary>
    public TimeSeriesDataTransformation DataTransformation { get; set; } = TimeSeriesDataTransformation.None;

}
