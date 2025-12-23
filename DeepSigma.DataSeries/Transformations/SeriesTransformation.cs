namespace DeepSigma.DataSeries.Transformations;

/// <summary>
/// Represents a transformation applied to a data series.
/// </summary>
public class SeriesTransformation : ISeriesTransformation
{
    /// <summary>
    /// Data series scalar multiplier.
    /// </summary>
    public decimal Scalar { get; set; } = 1;
}
