
namespace DeepSigma.DataSeries.Enums;

/// <summary>
/// Represents the type of transformation applied to a set of data series values.
/// </summary>
public enum VectorTransformation
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
    /// Z-Score of the data series values. Z-Score indicates how many standard deviations a data point is from the mean.
    /// </summary>
    /// <remarks>
    /// Defined as (X - μ) / σ, where X is the data point, μ is the mean, and σ is the standard deviation.
    /// </remarks>
    ZScore,
    /// <summary>
    /// Exponentially Weighted Moving Average of the data series values.
    /// </summary>
    EWMA,
    /// <summary>
    /// Standard Deviation Bands - 1 Standard Deviation
    /// </summary>
    StandardDeviation_1_Band,
    /// <summary>
    /// Standard Deviation Bands - 2 Standard Deviations
    /// </summary>
    StandardDeviation_2_Band,
    /// <summary>
    /// Standard Deviation Bands - 3 Standard Deviations
    /// </summary>
    StandardDeviation_3_Band,
    /// <summary>
    /// Standard Deviation Bands - Negative 1 Standard Deviation
    /// </summary>
    StandardDeviation_Negative_1_Band,
    /// <summary>
    /// Standard Deviation Bands - Negative 2 Standard Deviations
    /// </summary>
    StandardDeviation_Negative_2_Band,
    /// <summary>
    /// Standard Deviation Bands - Negative 3 Standard Deviations
    /// </summary>
    StandardDeviation_Negative_3_Band,
}
