using DeepSigma.General.Enums;
using DeepSigma.DataSeries.Interfaces;

namespace DeepSigma.DataSeries.Models.Collections;

/// <summary>
/// Represents a collection of time series data, allowing for mathematical operations on sub-series.
/// </summary>
/// <typeparam name="TDataType"></typeparam>
/// <typeparam name="TTransformation"></typeparam>
public abstract class AbstractSeriesCollection<TDataType, TTransformation> 
    : ISeries<TDataType, TTransformation>, ISeriesCollection<TDataType, TTransformation> 
    where TDataType : notnull
    where TTransformation : class, new()
{

    private protected int MaxCapacity { get; set; } = 1000;

    /// <summary>
    /// Collection of time series sub series.
    /// </summary>
    protected List<SeriesCollectionPair<TDataType, TTransformation>> SubSeriesCollection { get; private set; } = [];

    /// <inheritdoc/>
    public string SeriesName { get; set; } = string.Empty;

    /// <inheritdoc/>
    public TTransformation Transformation { get; set; } = new();

    /// <summary>
    /// Selects each element.
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public IEnumerable<Z> Select<Z>(Func<SeriesCollectionPair<TDataType, TTransformation>, Z> expression)
    {
        return SubSeriesCollection.Select(expression);
    }

    /// <summary>
    /// Filters the data set based on a provided expression.
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public IEnumerable<SeriesCollectionPair<TDataType, TTransformation>> Where(Func<SeriesCollectionPair<TDataType, TTransformation>, bool> expression)
    {
        return SubSeriesCollection.Where(expression);
    }

    /// <summary>
    /// Adds element to collection.
    /// </summary>
    /// <param name="mathematical_operation"></param>
    /// <param name="data_series"></param>
    public void Add(MathematicalOperation mathematical_operation, ISeries<TDataType, TTransformation> data_series)
    {
        if(SubSeriesCollection.Count >= MaxCapacity)
        {
            throw new InvalidOperationException($"Cannot add more than {MaxCapacity} sub-series to the collection.");
        }
        SeriesCollectionPair<TDataType, TTransformation> pair = new(mathematical_operation, data_series);
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
    internal SeriesCollectionPair<TDataType, TTransformation> ElementAt(int index)
    {
        return SubSeriesCollection.ElementAt(index);
    }

    /// <summary>
    /// Returns all collection data.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<SeriesCollectionPair<TDataType, TTransformation>> GetAllData()
    {
        return SubSeriesCollection;
    }

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
    public int GetSubSeriesCount()
    {
        return SubSeriesCollection.Count;
    }

    /// <summary>
    /// Returns combined series data from all sub series.
    /// </summary>
    /// <returns></returns>
    public abstract ICollection<TDataType>? GetSeriesData();

    /// <summary>
    /// Returns the transformed data series.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public ICollection<TDataType>? GetSeriesDataTransformed()
    {
        throw new NotImplementedException("Transformation logic is not implemented for SeriesCollection.");
    }
}
