using DeepSigma.DataSeries.Transformations;

namespace DeepSigma.DataSeries.Interfaces;

/// <summary>
/// Represents a generic series interface with transformation capabilities.
/// </summary>
/// <typeparam name="TCollectionKey">Key type for the series data collection element.</typeparam>
/// <typeparam name="TCollectionDataType">Data type for the series data collection element.</typeparam>
/// <typeparam name="TTransformation">Transformation type.</typeparam>
public interface ISeries<TCollectionKey, TCollectionDataType, TTransformation>
    where TCollectionKey: notnull, IComparable<TCollectionKey>
    where TCollectionDataType: notnull 
    where TTransformation : class, ISeriesTransformation
{
    /// <summary>
    /// The name of the series.
    /// </summary>
    string SeriesName { get; set; }

    /// <summary>
    /// The transformation applied to the series.
    /// </summary>
    TTransformation Transformation { get; set; }

    /// <summary>
    /// Clears all data from the series.
    /// </summary>
    void Clear();

    /// <summary>
    /// Gets the data of the series.
    /// </summary>
    /// <returns></returns>
    SortedDictionary<TCollectionKey, TCollectionDataType>? GetSeriesDataScaled();

    /// <summary>
    /// Gets the transformed data of the series.
    /// </summary>
    /// <returns></returns>
    SortedDictionary<TCollectionKey, TCollectionDataType>? GetSeriesDataScaledAndTransformed();

    /// <summary>
    /// Gets the count of sub-series within the series.
    /// </summary>
    /// <returns></returns>
    int GetSubSeriesCount();
}