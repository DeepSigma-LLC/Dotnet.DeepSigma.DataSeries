using DeepSigma.DataSeries.Interfaces;

namespace DeepSigma.DataSeries.Series;

/// <summary>
/// Base class for data series.
/// </summary>
/// <typeparam name="TCollectionDataType">The data type of the underlying collection.</typeparam>
/// <typeparam name="TTransformation">The data type of the transformation.</typeparam>
public abstract class AbstractBaseSeries<TCollectionDataType, TTransformation> : ISeries<TCollectionDataType, TTransformation> 
    where TCollectionDataType : notnull 
    where TTransformation : class, new()
{
    /// <summary>
    /// Indicates whether the series is empty.
    /// </summary>
    public bool IsEmpty => Data.Count == 0;

    /// <summary>
    /// Indicates whether multiple sub-series are allowed.
    /// </summary>
    public bool AllowMultipleSubSeries { get; init; } = true;

    /// <summary>
    /// Indicates whether the series is an aggregated series.
    /// </summary>
    public bool IsAggregatedSeries => GetSubSeriesCount() > 1;

    /// <summary>
    /// Indicates whether duplicate data points are allowed.
    /// </summary>
    public bool AllowDuplicateDataPoints { get; init; } = false;

    /// <inheritdoc cref="AbstractBaseSeries{TValue, TTransformation}"/>
    protected AbstractBaseSeries()
    {
        Transformation = new();
    }

    /// <summary>
    /// Collection of data points in the series.
    /// </summary>
    protected ICollection<TCollectionDataType> Data { get; set; } = [];

    /// <inheritdoc/>
    public string SeriesName { get; set; } = string.Empty;

    /// <inheritdoc/>
    public TTransformation Transformation { get; set; }

    /// <summary>
    /// Returns the data points in the series.
    /// </summary>
    /// <returns></returns>
    public virtual ICollection<TCollectionDataType> GetSeriesData()
    {
        return Data;
    }

    /// <summary>
    /// Returns the transformed data points in the series.
    /// </summary>
    /// <returns></returns>
    public abstract ICollection<TCollectionDataType> GetTransformedSeriesData();

    /// <inheritdoc/>
    public abstract int GetSubSeriesCount();

    /// <inheritdoc/>
    public void Clear()
    {
        Data.Clear();
    }

}
