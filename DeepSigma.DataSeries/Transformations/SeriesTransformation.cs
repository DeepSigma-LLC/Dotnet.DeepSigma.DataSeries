using DeepSigma.DataSeries.Enums;
using System;

namespace DeepSigma.DataSeries.Transformations;

/// <summary>
/// Represents a transformation applied to a data series.
/// </summary>
public class SeriesTransformation : ISeriesTransformation
{
    /// <inheritdoc cref="SeriesTransformation"/>
    public SeriesTransformation()
    {}

    /// <inheritdoc/>
    public decimal Scalar { get; set; } = 1;

    /// <inheritdoc/>
    public int? ObservationWindowCount { get; set; }

    /// <inheritdoc/>
    public TransformationDataInclusionType DataInclusionType { get; set; } = TransformationDataInclusionType.Point;

    /// <inheritdoc/>
    public required Transformation Transformation { get; set; } = Transformation.None;

}
