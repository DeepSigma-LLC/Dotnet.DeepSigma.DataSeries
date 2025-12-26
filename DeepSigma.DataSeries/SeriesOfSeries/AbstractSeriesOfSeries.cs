using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Transformations;
using DeepSigma.DataSeries.Utilities;
using DeepSigma.General.Enums;
using DeepSigma.General.Extensions;
using Microsoft.Extensions.Logging;
using System.Collections;
using DeepSigma.DataSeries.Models;

namespace DeepSigma.DataSeries.Series;

/// <summary>
/// Base class for data series.
/// </summary>
/// <typeparam name="TCollectionKey">The key type of the underlying collection.</typeparam>
/// <typeparam name="TCollectionDataType">The data type of the underlying collection.</typeparam>
/// <typeparam name="TTransformation">The data type of the transformation.</typeparam>
public abstract class AbstractSeriesOfSeries<TCollectionKey, TCollectionDataType, TTransformation> 
    : AbstractSeries<TCollectionKey, TCollectionDataType, TTransformation>, 
    ISeries<TCollectionKey, TCollectionDataType, TTransformation>, 
    IEnumerable<SeriesCollectionPair<TCollectionKey, TCollectionDataType, TTransformation>>
    where TCollectionKey : notnull, IComparable<TCollectionKey>
    where TCollectionDataType : class, IDataModel<TCollectionDataType>
    where TTransformation : class, ISeriesTransformation, new()
{
    /// <inheritdoc cref="AbstractSeriesOfSeries{TCollectionKey, TValue, TTransformation}"/>
    protected AbstractSeriesOfSeries(ILogger? logger = null) : base()
    {
        SubSeriesCollection = [];
        AllowMultipleSubSeries = true;
        SubSeriesCollection.RegisterLogger(logger);
    }

    /// <summary>
    /// Collection of sub-series within the series.
    /// </summary>
    public SeriesCollection<TCollectionKey, TCollectionDataType, TTransformation> SubSeriesCollection { get; set; }

    /// <summary>
    /// Indicates whether the series is empty.
    /// </summary>
    public sealed override bool IsEmpty => SubSeriesCollection.GetSubSeriesCount() == 0;

    /// <summary>
    /// Indicates whether the series is an aggregated series.
    /// </summary>
    public sealed override bool IsAggregatedSeries => GetSubSeriesCount() > 1;

    /// <summary>
    /// Returns the data points in the series.
    /// </summary>
    /// <returns></returns>
    public sealed override SortedDictionary<TCollectionKey, TCollectionDataType>? GetSeriesDataScaled()
    {
        SortedDictionary<TCollectionKey, TCollectionDataType>? series = SubSeriesCollection.GetCombinedScaledAndTransformedSeriesData();
        return series;
    }

    /// <inheritdoc/>
    public sealed override int GetSubSeriesCount() => SubSeriesCollection.GetSubSeriesCount();
    
    /// <inheritdoc/>
    public sealed override void Clear()
    {
        SubSeriesCollection.Clear();
    }

    /// <inheritdoc/>
    public void Add(ISeries<TCollectionKey, TCollectionDataType, TTransformation> series, MathematicalOperation mathematicalOperation = MathematicalOperation.Add)
    {
        SubSeriesCollection.Add(mathematicalOperation, series);
    }

    /// <inheritdoc/>
    public IEnumerator<SeriesCollectionPair<TCollectionKey, TCollectionDataType, TTransformation>> GetEnumerator() => SubSeriesCollection.GetEnumerator();

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
