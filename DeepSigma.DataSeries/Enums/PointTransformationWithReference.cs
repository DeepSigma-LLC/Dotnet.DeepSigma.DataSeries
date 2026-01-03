
namespace DeepSigma.DataSeries.Enums;

/// <summary>
/// Specifies the type of transformation applied to data points with reference to other points in the series.
/// </summary>
public enum PointTransformationWithReference
{
    /// <summary>
    /// No transformation applied.
    /// </summary>
    None,
    /// <summary>
    /// Difference between consecutive data series values.
    /// </summary>
    Difference,
    /// <summary>
    /// Computes the return between consecutive data series values.
    /// </summary>
    Return,
    /// <summary>
    /// Computes the cumulative return from data series and assumes a initial investment achieves the underlying return from a starting value.
    /// </summary>
    Wealth,
    /// <summary>
    /// Computes the reverse of the wealth transformation. Computes a wealth index that targets and end wealth value.
    /// </summary>
    WealthReverse,
    /// <summary>
    /// Drawdown is computed as the decline from the historical peak to the current value (not as a percentage).
    /// </summary>
    Drawdown,
    /// <summary>
    /// Drawdown percentage is computed as the decline from the historical peak to the current value expressed as a percentage of the peak value.
    /// </summary>
    DrawdownPercentage,

}
