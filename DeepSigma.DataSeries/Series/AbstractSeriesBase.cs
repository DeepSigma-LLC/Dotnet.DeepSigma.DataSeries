using DeepSigma.DataSeries.Enums;
using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Transformations;
using DeepSigma.DataSeries.Utilities;
using DeepSigma.General.Extensions;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Security.AccessControl;

namespace DeepSigma.DataSeries.Series;

/// <summary>
/// Abstract base class for data series.
/// </summary>
/// <typeparam name="TCollectionKey"></typeparam>
/// <typeparam name="TCollectionDataType"></typeparam>
/// <typeparam name="TTransformation"></typeparam>
public abstract class AbstractSeriesBase<TCollectionKey, TCollectionDataType, TTransformation> 
    : AbstractSeries<TCollectionKey, TCollectionDataType, TTransformation>, 
    ISeries<TCollectionKey, TCollectionDataType, TTransformation>, IEnumerable<KeyValuePair<TCollectionKey, TCollectionDataType>>
    where TCollectionKey : notnull, IComparable<TCollectionKey>
    where TCollectionDataType : class, IDataModel<TCollectionDataType>
    where TTransformation : class, ISeriesTransformation<TCollectionDataType>, new()
{
    /// <summary>
    /// The collection of data points in the series.
    /// </summary>
    private protected readonly SortedDictionary<TCollectionKey, TCollectionDataType> Data = [];

    /// <inheritdoc cref="AbstractSeriesBase{TCollectionKey, TCollectionDataType, TTransformation}"/>
    protected AbstractSeriesBase() : base() { }

    /// <inheritdoc cref="AbstractSeriesBase{TCollectionKey, TCollectionDataType, TTransformation}"/>
    protected AbstractSeriesBase(SortedDictionary<TCollectionKey, TCollectionDataType> data) : this()
    {
        Data = data;
    }

    /// <inheritdoc/>
    public sealed override bool IsEmpty => Data.Count == 0;

    /// <inheritdoc/>
    public sealed override bool IsAggregatedSeries => false;

    /// <inheritdoc/>
    public sealed override int GetSubSeriesCount() => 1;

    /// <inheritdoc/>
    public sealed override SortedDictionary<TCollectionKey, TCollectionDataType> GetSeriesDataUnscaled() => Data;

    /// <inheritdoc/>
    public void Add(TCollectionKey key, TCollectionDataType value)
    {
        Data.Add(key, value);
    }

    /// <inheritdoc/>
    public void Add(IEnumerable<KeyValuePair<TCollectionKey, TCollectionDataType>> points)
    {
        points.ForEach(point => Data.Add(point.Key, point.Value));
    }

    /// <inheritdoc/>
    public sealed override void Clear()
    {
        Data.Clear();
    }

    /// <inheritdoc/>
    public sealed override TCollectionKey? GetMinimumKey()
    {
        return Data.FirstOrDefault().Key; // since SortedDictionary is sorted, the first key is the minimum
    }

    /// <inheritdoc/>
    public sealed override TCollectionKey? GetMaximumKey()
    {
        return Data.LastOrDefault().Key; // since SortedDictionary is sorted, the last key is the maximum
    }

    /// <inheritdoc/>
    public IEnumerator<KeyValuePair<TCollectionKey, TCollectionDataType>> GetEnumerator() => Data.GetEnumerator();

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    
}
