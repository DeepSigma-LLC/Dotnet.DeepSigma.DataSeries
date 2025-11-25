namespace DeepSigma.DataSeries.Interfaces;

/// <summary>
/// Represents a generic series interface with transformation capabilities.
/// </summary>
/// <typeparam name="TCollectionDataType">Data type for the series data collection element.</typeparam>
/// <typeparam name="TTransformation">Transformation type.</typeparam>
public interface ISeries<TCollectionDataType, TTransformation> 
    where TCollectionDataType: notnull 
    where TTransformation : class
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
    ICollection<TCollectionDataType> GetSeriesData();

    /// <summary>
    /// Gets the transformed data of the series.
    /// </summary>
    /// <returns></returns>
    ICollection<TCollectionDataType> GetTransformedSeriesData();

    /// <summary>
    /// Gets the count of sub-series within the series.
    /// </summary>
    /// <returns></returns>
    int GetSubSeriesCount();
}