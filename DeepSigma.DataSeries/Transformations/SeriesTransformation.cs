using DeepSigma.DataSeries.Enums;
using System;

namespace DeepSigma.DataSeries.Transformations;

/// <summary>
/// Represents a transformation applied to a data series.
/// </summary>
public class SeriesTransformation<T> : ISeriesTransformation<T> 
    where T : Enum
{
    /// <inheritdoc cref="SeriesTransformation{T}"/>
    public SeriesTransformation()
    {
        // Runtime check to ensure T is a valid enum type for transformations.
        Type[] allowed =
        [
            typeof(PointTransformation),
            typeof(VectorTransformation),
            typeof(ReferencePointTransformation),
        ];

        if (!allowed.Contains(typeof(T))) throw new NotSupportedException($"{typeof(T).Name} is not allowed");
    }

    /// <inheritdoc/>
    public decimal Scalar { get; set; } = 1;

    /// <inheritdoc/>
    public int? ObservationWindowCount { get; set; }

    /// <inheritdoc/>
    public TransformationDataInclusionType DataInclusionType { get; set; } = TransformationDataInclusionType.Point;

    /// <inheritdoc/>
    public required T Transformation { get; set; }

    /// <Inheritdoc/>
    public VectorTransformation SetTransformation { get; set; } = VectorTransformation.None;

    /// <Inheritdoc/>
    public PointTransformation PointTransformation { get; set; } = PointTransformation.None;
}
