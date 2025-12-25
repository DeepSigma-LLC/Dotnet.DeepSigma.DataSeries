namespace DeepSigma.DataSeries.Transformations;

/// <summary>
/// Represents a transformation applied to a data series.
/// </summary>
public class SeriesTransformation : ISeriesTransformation
{
  
    /// <inheritdoc/>
    public decimal Scalar { get; set; } = 1;
}
