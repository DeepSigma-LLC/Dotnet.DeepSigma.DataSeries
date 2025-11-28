using DeepSigma.DataSeries.Interfaces;
using DeepSigma.General.Extensions;

namespace DeepSigma.DataSeries.DataSets;

/// <summary>
/// Represents a generic data set that can hold key-value pairs. 
/// The data is stored in a sorted dictionary to maintain order based on keys to facilitate efficient retrieval and manipulation.
/// If duplicate keys are added, an exception will be thrown. 
/// If duplicate keys are a possibility, consider using NonFunctionalDataSet instead.
/// </summary>
/// <typeparam name="TKeyDataType"></typeparam>
/// <typeparam name="TValueDataType"></typeparam>
public class FunctionalDataSet<TKeyDataType, TValueDataType> 
    where TKeyDataType : IComparable<TKeyDataType> 
    where TValueDataType : class, IImmutableDataModel<TValueDataType>
{
    /// <summary>
    /// A sorted dictionary to hold the data, where keys are of type TKey and values are of type TValue.
    /// </summary>
    private SortedDictionary<TKeyDataType, TValueDataType> Data { get; init; } = [];

    /// <summary>
    /// Adds data to the data set.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void Add(TKeyDataType key, TValueDataType value)
    {
        Data.Add(key, value);
    }

    /// <summary>
    /// Adds a collection of key-value pairs to the data set.
    /// </summary>
    /// <param name="data"></param>
    public void Add(SortedDictionary<TKeyDataType, TValueDataType> data)
    {
        foreach(var kvp in data)
        {
            Data.Add(kvp.Key, kvp.Value);
        }
    }

    /// <summary>
    /// Selects each element.
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public IEnumerable<Z> Select<Z>(Func<KeyValuePair<TKeyDataType, TValueDataType>, Z> expression)
    {
        return Data.Select(expression);
    }

    /// <summary>
    /// Filters the data set based on a provided expression.
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public FunctionalDataSet<TKeyDataType, TValueDataType> Where(Func<KeyValuePair<TKeyDataType, TValueDataType>, bool> expression)
    {
        FunctionalDataSet<TKeyDataType, TValueDataType> new_data_set = new();
        new_data_set.Add(Data.Where(expression).ToSortedDictionary());
        return new_data_set;
    }

    /// <summary>
    /// Counts the number of data points in the data set.
    /// </summary>
    /// <returns></returns>
    public int Count()
    {
        return Data.Count;
    }

    /// <summary>
    /// Retrieves a value from the data set by its key.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public TValueDataType? Get(TKeyDataType key)
    {
        if (Data.TryGetValue(key, out var value))
        {
            return value;
        }
        return default;
    }

    /// <summary>
    /// Retrieves all data from the data set as a sorted dictionary.
    /// </summary>
    /// <returns></returns>
    public SortedDictionary<TKeyDataType, TValueDataType> GetAllData()
    {
        return Data;
    }
}
