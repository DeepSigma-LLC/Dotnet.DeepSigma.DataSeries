using DeepSigma.DataSeries.Interfaces;
using DeepSigma.General.Extensions;
using System.Collections;

namespace DeepSigma.DataSeries.DataSets;

/// <summary>
/// Represents a generic data set. 
/// The data is stored in a sorted dictionary to maintain order based on keys to facilitate efficient retrieval and manipulation.
/// If duplicate keys are added, an exception will be thrown. 
/// </summary>
/// <typeparam name="TKeyDataType"></typeparam>
/// <typeparam name="TValueDataType"></typeparam>
/// <typeparam name="TValueAccumulatorDataType"></typeparam>
public class DataSet<TKeyDataType, TValueDataType, TValueAccumulatorDataType> 
    : IEnumerable<KeyValuePair<TKeyDataType, TValueDataType>>
    where TKeyDataType : IComparable<TKeyDataType>
    where TValueAccumulatorDataType : class, IAccumulator<TValueDataType>
    where TValueDataType : class, IDataModel<TValueDataType>
{
    /// <summary>
    /// A sorted dictionary to hold the data, where keys are of type TKey and values are of type TValue.
    /// </summary>
    public SortedDictionary<TKeyDataType, TValueDataType> Data { get; set; } = [];

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
        data.ForEach(x => Data.Add(x.Key, x.Value));
    }

    /// <summary>
    /// Retrieves a value from the data set by its key.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public TValueDataType? TryGet(TKeyDataType key) => Data.TryGetValue(key, out var value) ? value : null;

    /// <inheritdoc/>
    public IEnumerator<KeyValuePair<TKeyDataType, TValueDataType>> GetEnumerator() => Data.GetEnumerator();

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    
}
