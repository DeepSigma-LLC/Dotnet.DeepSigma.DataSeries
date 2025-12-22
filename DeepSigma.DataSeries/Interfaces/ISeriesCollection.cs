using DeepSigma.DataSeries.Models;
using DeepSigma.General.Enums;
using Microsoft.Extensions.Logging;

namespace DeepSigma.DataSeries.Interfaces;

/// <summary>
/// Interface for a collection of data series with associated mathematical operations and transformations.
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TDataType"></typeparam>
/// <typeparam name="TTransformation"></typeparam>
public interface ISeriesCollection<TKey, TDataType, TTransformation>
    where TKey : notnull
    where TDataType : notnull
    where TTransformation : class, new()
{

    /// <summary>
    /// Registers logger with class.
    /// </summary>
    /// <param name="logger"></param>
    void RegisterLogger(ILogger? logger);

    /// <summary>
    /// Retrieves all data series in the collection.
    /// </summary>
    /// <returns></returns>
    IEnumerable<SeriesCollectionPair<TKey, TDataType, TTransformation>> GetAllData();

    /// <summary>
    /// Gets the count of sub-series in the collection.
    /// </summary>
    /// <returns></returns>
    int GetSubSeriesCount();

    /// <summary>
    /// Adds a new data series with the specified mathematical operation to the collection.
    /// </summary>
    /// <param name="mathematical_operation"></param>
    /// <param name="data_series"></param>
    void Add(MathematicalOperation mathematical_operation, ISeries<TKey, TDataType, TTransformation> data_series);

    /// <summary>
    /// Clears all data from the collection.
    /// </summary>
    void Clear();

    /// <summary>
    /// Removes a data series from the collection by its series name.
    /// </summary>
    /// <param name="series_name"></param>
    void RemoveBySeriesName(string series_name);

    /// <summary>
    /// Retrieves the transformed data series in the collection.
    /// </summary>
    /// <returns></returns>
    SortedDictionary<TKey, TDataType>? GetCombinedAndTransformedSeriesData();
}