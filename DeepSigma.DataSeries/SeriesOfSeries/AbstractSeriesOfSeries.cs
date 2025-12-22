using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Models.Series;
using DeepSigma.DataSeries.Transformations;
using DeepSigma.DataSeries.Utilities;
using DeepSigma.General.Enums;
using DeepSigma.General.Extensions;
using Microsoft.Extensions.Logging;

namespace DeepSigma.DataSeries.Series;

/// <summary>
/// Base class for data series.
/// </summary>
/// <typeparam name="TCollectionKey">The key type of the underlying collection.</typeparam>
/// <typeparam name="TCollectionDataType">The data type of the underlying collection.</typeparam>
/// <typeparam name="TTransformation">The data type of the transformation.</typeparam>
public abstract class AbstractSeriesOfSeries<TCollectionKey, TCollectionDataType, TTransformation> 
    : AbstractSeries<TCollectionKey, TCollectionDataType, TTransformation>, 
    ISeries<TCollectionKey, TCollectionDataType, TTransformation> 
    where TCollectionKey : notnull
    where TCollectionDataType : class, IDataModel<TCollectionDataType>
    where TTransformation : SeriesTransformation, new()
{
    /// <inheritdoc cref="AbstractSeriesOfSeries{TCollectionKey, TValue, TTransformation}"/>
    protected AbstractSeriesOfSeries(ILogger? logger = null) : base()
    {
        SubSeriesCollection = new();
        AllowMultipleSubSeries = true;
        SubSeriesCollection.RegisterLogger(logger);
    }

    /// <summary>
    /// Collection of sub-series within the series.
    /// </summary>
    public TSeriesCollection SubSeriesCollection { get; set; }

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
    public sealed override SortedDictionary<TCollectionKey, TCollectionDataType>? GetSeriesData()
    {
        SortedDictionary<TCollectionKey, TCollectionDataType>? series = SubSeriesCollection.GetCombinedAndTransformedSeriesData();
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
    public sealed override SortedDictionary<TCollectionKey, TCollectionDataType>? GetSeriesDataTransformed()
    {
        var (Data, Error) = SeriesUtilities.GetTransformedSeries(GetSeriesData()?.ToSortedDictionary() ?? [], Transformation);

        if (Error != null || Data is null) return null;

        return Data;
    }
}
