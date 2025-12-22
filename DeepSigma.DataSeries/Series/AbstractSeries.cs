using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Transformations;

namespace DeepSigma.DataSeries.Series;

/// <summary>
/// Represents an abstract series.
/// </summary>
/// <typeparam name="TCollectionKey"></typeparam>
/// <typeparam name="TCollectionDataType"></typeparam>
/// <typeparam name="TTransformation"></typeparam>
public abstract class AbstractSeries<TCollectionKey, TCollectionDataType, TTransformation> : ISeries<TCollectionKey, TCollectionDataType, TTransformation>
    where TCollectionKey : notnull, IComparable<TCollectionKey>
    where TCollectionDataType : class, IDataModel<TCollectionDataType>
    where TTransformation : SeriesTransformation, new()
{

    /// <inheritdoc cref="AbstractSeries{TCollectionKey, TCollectionDataType, TTransformation}"/>
    protected AbstractSeries()
    {
        Transformation = new();
    }

    /// <summary>
    /// Indicates whether the series is empty.
    /// </summary>
    public virtual bool IsEmpty { get; }

    /// <summary>
    /// Indicates whether the series is an aggregated series.
    /// </summary>
    public virtual bool IsAggregatedSeries { get; }

    /// <summary>
    /// Indicates whether multiple sub-series are allowed.
    /// </summary>
    public bool AllowMultipleSubSeries { get; init; }

    /// <summary>
    /// Indicates whether duplicate data points are allowed.
    /// </summary>
    public bool AllowDuplicateDataPoints { get; init; }

    /// <inheritdoc/>
    public string SeriesName { get; set; } = string.Empty;

    /// <inheritdoc/>
    public TTransformation Transformation { get; set; }

    /// <summary>
    /// Returns the data points in the series.
    /// </summary>
    /// <returns></returns>
    public abstract SortedDictionary<TCollectionKey, TCollectionDataType>? GetSeriesData();

    /// <summary>
    /// Returns the transformed data points in the series.
    /// </summary>
    /// <returns></returns>
    public abstract SortedDictionary<TCollectionKey, TCollectionDataType>? GetSeriesDataTransformed();

    /// <inheritdoc/>
    public abstract int GetSubSeriesCount();

    /// <inheritdoc/>
    public abstract void Clear();

}
