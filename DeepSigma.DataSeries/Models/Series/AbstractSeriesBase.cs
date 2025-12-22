using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Models.Series;
using DeepSigma.DataSeries.Transformations;
using DeepSigma.DataSeries.Utilities;
using DeepSigma.General.Extensions;

namespace DeepSigma.DataSeries.Models.BaseSeries;

/// <summary>
/// Abstract base class for data series.
/// </summary>
/// <typeparam name="TCollectionKey"></typeparam>
/// <typeparam name="TCollectionDataType"></typeparam>
/// <typeparam name="TTransformation"></typeparam>
public abstract class AbstractSeriesBase<TCollectionKey, TCollectionDataType, TTransformation> 
    : AbstractSeries<TCollectionKey, TCollectionDataType, TTransformation>, 
    ISeries<TCollectionKey, TCollectionDataType, TTransformation>
    where TCollectionKey : notnull
    where TCollectionDataType : class, IDataModel<TCollectionDataType>
    where TTransformation : SeriesTransformation, new()
{
    /// <summary>
    /// The collection of data points in the series.
    /// </summary>
    private protected readonly SortedDictionary<TCollectionKey, TCollectionDataType> Data = [];

    /// <inheritdoc cref="AbstractSeriesBase{TCollectionKey, TCollectionDataType, TTransformation}"/>
    protected AbstractSeriesBase() : base()
    {
        this.AllowMultipleSubSeries = false;
        this.AllowDuplicateDataPoints = false;
    }

    /// <inheritdoc cref="AbstractSeriesBase{TCollectionKey, TCollectionDataType, TTransformation}"/>
    protected AbstractSeriesBase(SortedDictionary<TCollectionKey, TCollectionDataType> data) : this()
    {
        Data = data;
    }

    /// <inheritdoc/>
    public sealed override void Clear()
    {
        Data.Clear();
    }

    /// <inheritdoc/>
    public sealed override SortedDictionary<TCollectionKey, TCollectionDataType>? GetSeriesData()
    {
        return Data;
    }

    /// <inheritdoc/>
    public sealed override int GetSubSeriesCount() => 1;
    
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
    public sealed override SortedDictionary<TCollectionKey, TCollectionDataType>? GetSeriesDataTransformed()
    {
        SortedDictionary<TCollectionKey, TCollectionDataType>? Data = GetSeriesData()?.ToSortedDictionary();
        if (Data is null) return null;

        (var TransformedData, var Error) = SeriesUtilities.GetTransformedSeries(Data, Transformation);
        if (Error is not null || TransformedData is null) return null;
        return TransformedData;
    }
}
