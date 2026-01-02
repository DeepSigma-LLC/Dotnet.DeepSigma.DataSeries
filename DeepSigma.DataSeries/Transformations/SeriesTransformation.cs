using DeepSigma.DataSeries.Enums;

namespace DeepSigma.DataSeries.Transformations;

/// <summary>
/// Represents a transformation applied to a data series.
/// </summary>
public class SeriesTransformation : ISeriesTransformation
{
  
    /// <inheritdoc/>
    public decimal Scalar { get; set; } = 1;

    /// <inheritdoc/>
    public int? ObservationWindowCount { get; set; }

    /// <inheritdoc/>
    public TransformationDataInclusionType DataInclusionType { get; set; } = TransformationDataInclusionType.All;

    /// <Inheritdoc/>
    public Transformation Transformation { get; set; } = Transformation.None;
}
