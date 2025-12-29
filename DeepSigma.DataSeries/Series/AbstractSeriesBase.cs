using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Transformations;
using DeepSigma.DataSeries.Utilities;
using DeepSigma.General.Extensions;
using System.Collections;

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
    where TTransformation : class, ISeriesTransformation, new()
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
    public sealed override SortedDictionary<TCollectionKey, TCollectionDataType> GetSeriesDataScaled()
    {
        return GenericTimeSeriesUtilities.GetScaledSeries(Data, Transformation.Scalar);
    }

    /// <summary>
    /// Gets the underlying series data as an unscaled sorted dictionary.
    /// </summary>
    /// <returns>A sorted dictionary containing the unscaled series data, where each key represents a collection key and each
    /// value represents the associated data.</returns>
    /// <remarks>
    /// Note: the collection is passed by reference to avoid reallocation of the data structure as a copy in memory.
    /// Modifying the collection will modify the objects data directly.
    /// </remarks>
    public SortedDictionary<TCollectionKey, TCollectionDataType> GetSeriesDataUnscaled() => Data;

    
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
    public IEnumerator<KeyValuePair<TCollectionKey, TCollectionDataType>> GetEnumerator() => Data.GetEnumerator();

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    
}
