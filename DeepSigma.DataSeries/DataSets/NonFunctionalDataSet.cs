using DeepSigma.DataSeries.Interfaces;

namespace DeepSigma.DataSeries.DataSets;

/// <summary>
/// Represents a generic data set that can hold key-value pairs.
/// </summary>
/// <typeparam name="TKeyDataType"></typeparam>
/// <typeparam name="TValueDataType"></typeparam>
public class NonFunctionalDataSet<TKeyDataType, TValueDataType> 
    where TKeyDataType : IComparable<TKeyDataType> 
    where TValueDataType : class, IImmutableDataModel<TValueDataType>
{
    /// <summary>
    /// A list to hold the data, where keys are of type TKey and values are of type TValue.
    /// </summary>
    private List<(TKeyDataType Key, TValueDataType Data)> Data { get;init; } = [];

    /// <summary>
    /// Adds data to the data set.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void Add(TKeyDataType key, TValueDataType value)
    {
        Data.Add((key, value));
    }

    /// <summary>
    /// Adds multiple data entries to the data set.
    /// </summary>
    /// <param name="data"></param>
    public void Add(List<(TKeyDataType, TValueDataType)> data)
    {
        Data.AddRange(data);
    }

    /// <summary>
    /// Selects each element.
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public IEnumerable<Z> Select<Z>(Func<(TKeyDataType Key, TValueDataType Data), Z> expression)
    {
        return Data.Select(expression);
    }

    /// <summary>
    /// Filters the data set based on a provided expression.
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    public NonFunctionalDataSet<TKeyDataType, TValueDataType> Where(Func<(TKeyDataType Key, TValueDataType Data), bool> expression)
    {
        NonFunctionalDataSet<TKeyDataType, TValueDataType> new_data_set = new();
        new_data_set.Add(Data.Where(expression).ToList());
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
    public TValueDataType[]? Get(TKeyDataType key)
    {
        return Data.Where(x => x.Key.Equals(key)).Select(x=> x.Data).ToArray();
    }

    /// <summary>
    /// Retrieves all data from the data set as a sorted dictionary.
    /// </summary>
    /// <returns></returns>
    public List<(TKeyDataType, TValueDataType)> GetAllData()
    {
        return Data;
    }
}
