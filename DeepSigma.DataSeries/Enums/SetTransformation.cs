
namespace DeepSigma.DataSeries.Enums;

/// <summary>
/// Represents the type of transformation applied to a set of data series values.
/// </summary>
public enum SetTransformation
{
    /// <summary>
    /// No transformation is applied.
    /// </summary>
    None,
    /// <summary>
    /// Sum of the data series values.
    /// </summary>
    Sum,
    /// <summary>
    /// Difference between consecutive data series values.
    /// </summary>
    Difference,
    /// <summary>
    /// Return between consecutive data series values.
    /// </summary>
    Return,
    /// <summary>
    /// Minimum value in the data series.
    /// </summary>
    Min,
    /// <summary>
    /// Maximum value in the data series.
    /// </summary>
    Max,
    /// <summary>
    /// Average of the data series values.
    /// </summary>
    Average,
    /// <summary>
    /// Standard deviation of the data series values.
    /// </summary>
    StandardDeviation,
    /// <summary>
    /// Standard deviation of the percentage changes between consecutive data series values.
    /// </summary>
    StandardDeviationOfPercentageChange,
    /// <summary>
    /// Variance of the data series values.
    /// </summary>
    Variance,
    /// <summary>
    /// Variance of the percentage changes between consecutive data series values.
    /// </summary>
    VarianceOfPercentageChange,
    /// <summary>
    /// Exponentially Weighted Moving Average of the data series values.
    /// </summary>
    EWMA,
    /// <summary>
    /// Computes the cumulative return from data series and assumes a initial investment achieves the underlying return from a starting value.
    /// </summary>
    Wealth,
    WealthReverse,
    /// <summary>
    /// Drawdown from observed peaks in the data series (no-look-ahead). Drawdown is computed as the decline from the historical peak to the current value (not as a percentage).
    /// </summary>
    Drawdown,
    /// <summary>
    /// Drawdown percentage from observed peaks in the data series (no-look-ahead). Drawdown percentage is computed as the decline from the historical peak to the current value expressed as a percentage of the peak value.
    /// </summary>
    DrawdownPercentage,
    StdDev_1_Band,
    StdDev_2_Band,
    StdDev_3_Band,
    StdDev_Negative_1_Band,
    StdDev_Negative_2_Band,
    StdDeV_Negative_3_Band,
}
