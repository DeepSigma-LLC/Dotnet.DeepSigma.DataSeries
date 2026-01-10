using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Transformations;
using DeepSigma.DataSeries.Utilities;

namespace DeepSigma.DataSeries.Series;

/// <summary>
/// Represents an abstract series.
/// </summary>
/// <typeparam name="TCollectionKey"></typeparam>
/// <typeparam name="TCollectionDataType"></typeparam>
/// <typeparam name="TTransformation"></typeparam>
public abstract class AbstractSeries<TCollectionKey, TCollectionDataType, TTransformation> 
    : ISeries<TCollectionKey, TCollectionDataType, TTransformation>
    where TCollectionKey : notnull, IComparable<TCollectionKey>
    where TCollectionDataType : class, IDataModel<TCollectionDataType>
    where TTransformation : class, ISeriesTransformation<TCollectionDataType>, new()
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
    public bool AllowMultipleSubSeries { get; init; } = false;

    /// <inheritdoc/>
    public string SeriesName { get; set; } = string.Empty;

    /// <inheritdoc/>
    public TTransformation Transformation { get; set; }

    /// <inheritdoc/>
    public abstract SortedDictionary<TCollectionKey, TCollectionDataType> GetSeriesDataUnscaled();

    /// <inheritdoc/>
    public abstract SortedDictionary<TCollectionKey, TCollectionDataType> GetSeriesDataTransformed();

    /// <inheritdoc/>
    public abstract int GetSubSeriesCount();

    /// <inheritdoc/>
    public abstract void Clear();

    /// <inheritdoc/>
    public abstract TCollectionKey? GetMinimumKey();

    /// <inheritdoc/>
    public abstract TCollectionKey? GetMaximumKey();
}
