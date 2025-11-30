using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Models.Series;
using DeepSigma.DataSeries.Transformations;
using DeepSigma.General.Enums;
using DeepSigma.General.Extensions;

namespace DeepSigma.DataSeries.Series;

/// <summary>
/// Base class for data series.
/// </summary>
/// <typeparam name="TCollectionDataType">The data type of the underlying collection.</typeparam>
/// <typeparam name="TTransformation">The data type of the transformation.</typeparam>
/// <typeparam name="TSeriesCollection">The type of the series collection.</typeparam>
public abstract class AbstractSeriesOfSeries<TCollectionDataType, TTransformation, TSeriesCollection> : AbstractSeries<TCollectionDataType,TTransformation>, 
    ISeries<TCollectionDataType, TTransformation> 
    where TCollectionDataType : notnull 
    where TTransformation : SeriesTransformation, new()
    where TSeriesCollection : ISeriesCollection<TCollectionDataType, TTransformation>, new()
{
    /// <inheritdoc cref="AbstractSeriesOfSeries{TValue, TTransformation, TSeriesCollection}"/>
    protected AbstractSeriesOfSeries() : base()
    {
        SubSeriesCollection = new();
        AllowMultipleSubSeries = true;
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
    public sealed override ICollection<TCollectionDataType>? GetSeriesData()
    {
        ICollection<TCollectionDataType>? series = SubSeriesCollection.GetCombinedAndTransformedSeriesData();
        if (series is null) return null;
        return series;
    }

    /// <inheritdoc/>
    public sealed override int GetSubSeriesCount()
    {
        return SubSeriesCollection.GetSubSeriesCount();
    }

    /// <inheritdoc/>
    public sealed override void Clear()
    {
        SubSeriesCollection.Clear();
    }

    /// <inheritdoc/>
    public void Add(ISeries<TCollectionDataType, TTransformation> series, MathematicalOperation mathematicalOperation = MathematicalOperation.Add)
    {
        SubSeriesCollection.Add(mathematicalOperation, series);
    }
}
