using DeepSigma.DataSeries.Interfaces;
using DeepSigma.DataSeries.Transformations;
using DeepSigma.DataSeries.Utilities;
using DeepSigma.General.Enums;
using DeepSigma.General.Extensions;
using Microsoft.Extensions.Logging;
using System.Collections;

namespace DeepSigma.DataSeries.Models;

/// <summary>
/// Represents a collection of time series data, allowing for mathematical operations on sub-series.
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TDataType"></typeparam>
/// <typeparam name="TTransformation"></typeparam>
public class SeriesCollection<TKey, TDataType, TTransformation> 
    : ISeriesCollection<TKey, TDataType, TTransformation>, IEnumerable<SeriesCollectionPair<TKey, TDataType, TTransformation>>
    where TKey : notnull, IComparable<TKey>
    where TDataType : class, IDataModel<TDataType>
    where TTransformation : class, ISeriesTransformation, new()
{

    /// <inheritdoc/>
    public void RegisterLogger(ILogger? logger)
    {
        Logger = logger;
    }

    /// <summary>
    /// Logger interface used to control logging logic injected from dependency injection.
    /// </summary>
    protected ILogger? Logger { get; private set; }

    /// <summary>
    /// Collection of time series sub series.
    /// </summary>
    protected List<SeriesCollectionPair<TKey, TDataType, TTransformation>> SubSeriesCollection { get; private set; } = [];

    /// <summary>
    /// Adds element to collection.
    /// </summary>
    /// <param name="mathematical_operation"></param>
    /// <param name="data_series"></param>
    public void Add(MathematicalOperation mathematical_operation, ISeries<TKey, TDataType, TTransformation> data_series)
    {
        SeriesCollectionPair<TKey, TDataType, TTransformation> pair = new(mathematical_operation, data_series);
        SubSeriesCollection.Add(pair);
    }

    /// <summary>
    /// Remove element by sub series name.
    /// </summary>
    /// <param name="series_name"></param>
    public void RemoveBySeriesName(string series_name)
    {
        SubSeriesCollection.RemoveAll(x => x.Series.SeriesName == series_name);
    }

    /// <summary>
    /// Returns element at specified index.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    internal SeriesCollectionPair<TKey, TDataType, TTransformation> ElementAt(int index) => SubSeriesCollection.ElementAt(index);
    
    /// <summary>
    /// Returns all collection data.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<SeriesCollectionPair<TKey, TDataType, TTransformation>> GetAllData() => SubSeriesCollection;

    /// <summary>
    /// Clears the collection of sub series.
    /// </summary>
    public void Clear()
    {
        SubSeriesCollection.Clear();
    }

    /// <summary>
    /// Returns the count of sub-series in the data series.
    /// </summary>
    /// <returns></returns>
    public int GetSubSeriesCount() => SubSeriesCollection.Count;

    /// <summary>
    /// Combines and transforms data from all sub-series into a single sorted dictionary, applying the specified
    /// mathematical operations to each series as part of the combination process.
    /// </summary>
    /// <remarks>If only one sub-series is present, its transformed data is returned directly. When multiple
    /// sub-series are present, each is transformed and combined using the associated mathematical operation. If an
    /// error occurs during the combination process, the method returns null and logs the error.</remarks>
    /// <returns>A sorted dictionary containing the combined and transformed data from all sub-series, or null if the combination
    /// fails or no data is available.</returns>
    public SortedDictionary<TKey, TDataType>? GetCombinedScaledAndTransformedSeriesData()
    {
        if (GetSubSeriesCount() == 1)
        {
            var selected_series = SubSeriesCollection.First();
            return selected_series.Series.GetSeriesDataScaledAndTransformed();
        }

        List<(SortedDictionary<TKey, TDataType>, MathematicalOperation)> Series = [];
        SubSeriesCollection.ForEach(x => Series.Add((x.Series.GetSeriesDataScaledAndTransformed() ?? [], x.MathematicalOperation)));

        return GenericTimeSeriesUtilities.GetCombinedSeries(Series);
    }

    /// <inheritdoc/>
    public IEnumerator<SeriesCollectionPair<TKey, TDataType, TTransformation>> GetEnumerator() => SubSeriesCollection.GetEnumerator();

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    
}
