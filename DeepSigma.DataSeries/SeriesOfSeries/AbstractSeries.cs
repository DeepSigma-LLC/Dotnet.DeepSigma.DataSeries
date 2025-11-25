using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Models.Collections;
using DeepSigma.General.Enums;

namespace DeepSigma.DataSeries.Series;

/// <summary>
/// Base class for data series.
/// </summary>
/// <typeparam name="TCollectionDataType">The data type of the underlying collection.</typeparam>
/// <typeparam name="TTransformation">The data type of the transformation.</typeparam>
/// <typeparam name="TSeriesCollection">The type of the series collection.</typeparam>
public abstract class AbstractSeries<TCollectionDataType, TTransformation, TSeriesCollection> : ISeries<TCollectionDataType, TTransformation> 
    where TCollectionDataType : notnull 
    where TTransformation : class, new()
    where TSeriesCollection : ISeriesCollection<TCollectionDataType, TTransformation>, new()
{
    /// <inheritdoc cref="AbstractSeries{TValue, TTransformation, TSeriesCollection}"/>
    protected AbstractSeries()
    {
        Transformation = new();
        SubSeriesCollection = new();
    }

    /// <summary>
    /// Collection of sub-series within the series.
    /// </summary>
    public TSeriesCollection SubSeriesCollection { get; set; }

    /// <summary>
    /// Indicates whether the series is empty.
    /// </summary>
    public bool IsEmpty => SubSeriesCollection.GetSubSeriesCount() == 0;

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

    /// <inheritdoc/>
    public string SeriesName { get; set; } = string.Empty;

    /// <inheritdoc/>
    public TTransformation Transformation { get; set; }

    /// <summary>
    /// Returns the data points in the series.
    /// </summary>
    /// <returns></returns>
    public ICollection<TCollectionDataType> GetSeriesData()
    {
        return SubSeriesCollection.GetSeriesData();
    }

    /// <summary>
    /// Returns the transformed data points in the series.
    /// </summary>
    /// <returns></returns>
    public abstract ICollection<TCollectionDataType> GetTransformedSeriesData();

    /// <inheritdoc/>
    public int GetSubSeriesCount()
    {
        return SubSeriesCollection.GetSubSeriesCount();
    }

    /// <inheritdoc/>
    public void Clear()
    {
        SubSeriesCollection.Clear();
    }

    /// <inheritdoc/>
    public void Add(ISeries<TCollectionDataType, TTransformation> series, MathematicalOperation mathematicalOperation = MathematicalOperation.Add)
    {
        SubSeriesCollection.Add(mathematicalOperation, series);
    }
}
