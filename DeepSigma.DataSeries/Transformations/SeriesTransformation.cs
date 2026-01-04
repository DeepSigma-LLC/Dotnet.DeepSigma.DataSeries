using DeepSigma.DataSeries.Enums;
using DeepSigma.DataSeries.Interfaces;
using System;

namespace DeepSigma.DataSeries.Transformations;

/// <summary>
/// Represents a transformation applied to a data series.
/// </summary>
public class SeriesTransformation<TData> : ISeriesTransformation<TData>
    where TData : class, IDataModel<TData>
{
    /// <inheritdoc cref="SeriesTransformation{TData}"/>
    public SeriesTransformation()
    {}

    /// <inheritdoc/>
    public decimal Scalar { get; set; } = 1;

    /// <inheritdoc/>
    public int? ObservationWindowCount { get; set; }

    /// <inheritdoc/>
    public TransformationDataInclusionType DataInclusionType { get; set; } = TransformationDataInclusionType.Point;

    /// <inheritdoc/>
    public Transformation Transformation { get; set; } = Transformation.None;

    /// <inheritdoc/>
    public Func<TData, TData>? CustomPointTransformationMethod { get; set; } = null;

    /// <inheritdoc/>
    public Func<TData, TData, TData>? CustomReferencePointTransformationMethod { get; set; } = null;

    /// <inheritdoc/>
    public Func<IEnumerable<TData>, TData?>? CustomReferencePointSelectionMethod { get; set; } = null;

    /// <inheritdoc/>
    public int RequiredPointsForReferencePointSelection { get; set; } = 2;

    /// <inheritdoc/>
    public Func<IEnumerable<TData>, TData>? CustomVectorTransformationMethod { get; set; } = null;
}
